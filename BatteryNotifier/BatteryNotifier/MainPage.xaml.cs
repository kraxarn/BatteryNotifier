using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using BatteryNotifier.iOS;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace BatteryNotifier
{
	public partial class MainPage
	{
		private readonly Battery battery;

		private readonly bool lowEnabled, highEnabled;

		private readonly int lowValue, highValue;

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
			lowEnabled  = Preferences.Get("lowEnabled",  true);
			highEnabled = Preferences.Get("highEnabled", true);

			lowValue  = Preferences.Get("lowValue",  50);
			highValue = Preferences.Get("highValue", 60);

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
			
			/*
			 * TODO
			 * This is a VERY temporary workaround
			 * for forcing the app to always be
			 * running. Remake this with proper
			 * backgrounding at some point.
			 */
			Task.Run(() =>
			{
				UIApplication.SharedApplication.BeginBackgroundTask(() => { });

				while (true)
					Thread.Sleep(5000);
			});
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
			
			if (lowEnabled && level == lowValue)
				Tools.ShowNotification("Battery Discharged", $"Battery is at {level}%");
			else if (highEnabled && level == highValue)
				Tools.ShowNotification("Battery Charged", $"Battery is at {level}%");

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