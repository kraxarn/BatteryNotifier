using System;
using Foundation;
using UIKit;

namespace BatteryNotifier.iOS
{
	public class Battery
	{
		private NSObject levelObserver;
		private NSObject stateObserver;

		public delegate void BatteryChargingEvent(bool charging);

		public delegate void BatteryLevelEvent(int level);

		public BatteryChargingEvent OnCharging;

		public BatteryLevelEvent OnLevel;

		public int Level => (int) Math.Round(UIDevice.CurrentDevice.BatteryLevel * 100);

		public bool IsCharging
		{
			get
			{
				switch (UIDevice.CurrentDevice.BatteryState)
				{
					case UIDeviceBatteryState.Full:
					case UIDeviceBatteryState.Charging:
						return true;

					case UIDeviceBatteryState.Unplugged:
						return false;

					default:
						throw new InvalidOperationException($"Unknown battery state: {UIDevice.CurrentDevice.BatteryState}");
				}
			}
		}

		public Battery()
		{
			UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;

			levelObserver = UIDevice.Notifications.ObserveBatteryLevelDidChange(BatteryLevelChanged);
			stateObserver = UIDevice.Notifications.ObserveBatteryStateDidChange(BatteryStateChanged);
		}

		private void Stop()
		{
			UIDevice.CurrentDevice.BatteryMonitoringEnabled = false;


			levelObserver?.Dispose();
			levelObserver = null;

			stateObserver?.Dispose();
			stateObserver = null;
		}

		private void BatteryStateChanged(object sender, NSNotificationEventArgs e) => OnCharging?.Invoke(IsCharging);

		private void BatteryLevelChanged(object sender, NSNotificationEventArgs e) => OnLevel?.Invoke(Level);
	}
}