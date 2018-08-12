using System.Diagnostics;
using BatteryNotifier.iOS;
using UserNotifications;
using Xamarin.Forms;

namespace BatteryNotifier
{
	public partial class MainPage
	{
		private readonly Battery battery;

		public MainPage()
		{
			// XAML UI stuff
			InitializeComponent();

			// Create battery
			battery = new Battery();

			// Subscribe to events
			battery.OnCharging += charging => UpdateBatteryImage();
			battery.OnLevel    += level    => UpdateBatteryStatus();

			// Set settings background color
			FrameControls.BackgroundColor = Color.FromRgba(0.9, 0.9, 0.9, 0.7);

			// Set default settings
			var lowEnabled  = Preferences.Get("lowEnabled",  true);
			var highEnabled = Preferences.Get("highEnabled", true);

			var lowValue  = Preferences.Get("lowValue",  50);
			var highValue = Preferences.Get("highValue", 60);

			SwitchLow.IsToggled  = lowEnabled;
			SwitchHigh.IsToggled = highEnabled;

			SliderLowPercent.Value  = lowValue  / 5f;
			SliderHighPercent.Value = highValue / 5f;

			LabelLowPercent.Text  = $"{lowValue}%";
			LabelHighPercent.Text = $"{highValue}%";

			// Set battery image
			UpdateBatteryImage();

			// Update labels depending on setting sliders
			SliderLowPercent.ValueChanged += (sender, args) =>
			{
				var p = (int) args.NewValue * 5;
				LabelLowPercent.Text  = $"{p}%";
				Preferences.Set("lowValue", p);
			};
			SliderHighPercent.ValueChanged += (sender, args) =>
			{
				var p = (int) args.NewValue * 5;
				LabelHighPercent.Text = $"{p}%";
				Preferences.Set("highValue", p);
			};
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, error) =>
			{
				if (!approved)
					Tools.ShowAlert("Error", "Notifications are required for the app to work");
			});
		}

		private void UpdateBatteryStatus()
		{
			// Check if charge is any set value
			var level = battery.Level;

			Debug.WriteLine($"UpdateBatteryStatus: {level}%");

			Tools.ShowNotification("title", "subtitle", $"Battery is at {level}%");

			// Also update image
			UpdateBatteryImage();
		}

		private void UpdateBatteryImage()
		{
			var level    = battery.Level / 10 * 10;
			var charging = battery.IsCharging;

			var fileName = $"{(charging ? "charging-" : "")}{level}.png";

			ImageBattery.Source = ImageSource.FromFile($"images/{fileName}");
		}
	}
}