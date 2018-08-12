using System;
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

		public static void ShowNotification(string title, string body)
		{
			var notif = new UILocalNotification
			{
				FireDate = NSDate.Now,
				AlertTitle = title,
				AlertBody = body,
				SoundName = UILocalNotification.DefaultSoundName
			};

			UIApplication.SharedApplication.ScheduleLocalNotification(notif);
		}
	}
}