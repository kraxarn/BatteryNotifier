﻿using BatteryNotifier.iOS;
using Xamarin.Forms;

namespace BatteryNotifier
{
	public partial class MainPage
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
			// XAML UI stuff
			InitializeComponent();

			// Create battery
			battery = new Battery();

			// Subscribe to events
			battery.OnCharging += charging => LabelStatus.Text = charging ? "Charging" : "Not charging";
			battery.OnLevel    += level    => LabelTitle.Text  = $"{level}%";

			// Set settings background color
			FrameControls.BackgroundColor = Color.FromRgba(0.9, 0.9, 0.9, 0.95);

			// Set default labels
			LabelTitle.Text = $"{battery.Level}%";
			LabelStatus.Text = battery.IsCharging ? "Charging" : "Not charging";

			// Set backgrond color depending on battery level
			BackgroundColor = BatteryColor;

			// Update labels depending on setting sliders
			SliderLowPercent.ValueChanged  += (sender, args) => { LabelLowPercent.Text  = $"{(int) args.NewValue * 5}%"; };
			SliderHighPercent.ValueChanged += (sender, args) => { LabelHighPercent.Text = $"{(int) args.NewValue * 5}%"; };
		}
	}
}
