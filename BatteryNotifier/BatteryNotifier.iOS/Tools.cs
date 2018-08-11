using System.Diagnostics;
using Foundation;
using UIKit;
using UserNotifications;

namespace BatteryNotifier.iOS
{
	public static class Tools
	{
		public static void ShowAlert(string title, string message)
		{
			void Show()
			{
				var alert = new UIAlertView
				{
					Title   = title,
					Message = message
				};
				alert.AddButton("Ok");
				alert.Show();
			}

			if (NSThread.Current.IsMainThread)
				Show();
			else
				NSRunLoop.Main.BeginInvokeOnMainThread(Show);
		}

		public static void ShowNotification(string title, string subtitle, string body)
		{
			var content = new UNMutableNotificationContent
			{
				Title = title,
				Subtitle = subtitle,
				Body = body
			};

			var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(5, false);
			var request = UNNotificationRequest.FromIdentifier("batteryRequest", content, trigger);

			UNUserNotificationCenter.Current.AddNotificationRequest(request, err =>
			{
				if (err != null)
					Debug.WriteLine($"NotifError: {err.LocalizedDescription}");
				else
					Debug.WriteLine("NotifOk");
			});
		}
	}
}