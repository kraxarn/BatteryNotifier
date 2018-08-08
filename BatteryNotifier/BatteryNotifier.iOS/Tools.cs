using Xamarin.Forms;

namespace BatteryNotifier.iOS
{
	public static class Tools
	{
		/// <summary>
		/// Perfect battery (50-60%)
		/// </summary>
		public static Color Green = Color.FromHex("#43a047");

		/// <summary>
		/// Good battery (25-80% except 50-60%)
		/// </summary>
		public static Color Yellow = Color.FromHex("#fdd835");

		/// <summary>
		/// Bad battery (0-100% except 25-80%)
		/// </summary>
		public static Color Red = Color.FromHex("#e53935");
	}
}