using BatteryNotifier.iOS;
using Xamarin.Forms;

namespace BatteryNotifier
{
	public partial class MainPage : ContentPage
	{
		private Color BatteryColor
		{
			get
			{
				if (battery.Level >= 50 && battery.Level <= 60)
					return Tools.Green;

				if (battery.Level >= 25 && battery.Level <= 80)
					return Tools.Yellow;

				return Tools.Red;
			}
		}

		private readonly Battery battery;

		public MainPage()
		{
			InitializeComponent();

			battery = new Battery();

			FrameControls.BackgroundColor = Color.FromRgba(0.9, 0.9, 0.9, 0.95);

			LabelTitle.Text = $"{battery.Level}%";

			LabelStatus.Text = battery.IsCharging ? "Charging" : "Not charging";

			BackgroundColor = BatteryColor;

			SliderLowPercent.ValueChanged += (sender, args) => { LabelLowPercent.Text = $"{(int) args.NewValue * 5}%"; };

			SliderHighPercent.ValueChanged += (sender, args) => { LabelHighPercent.Text = $"{(int)args.NewValue * 5}%"; };
		}
	}
}
