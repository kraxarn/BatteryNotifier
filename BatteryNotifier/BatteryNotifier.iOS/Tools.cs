using Foundation;
using UIKit;

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
	}
}