using Foundation;

namespace BatteryNotifier.iOS
{
	public static class Preferences
	{
		private static NSUserDefaults UserDefaults => NSUserDefaults.StandardUserDefaults;

		public static bool ContainsKey(string key) => UserDefaults[key] != null;

		public static void Remove(string key)
		{
			using (var defaults = UserDefaults)
			{
				if (defaults[key] != null)
					defaults.RemoveObject(key);
			}
		}

		public static void Clear()
		{
			using (var defaults = UserDefaults)
			{
				var items = defaults.ToDictionary();

				foreach (var key in items.Keys)
				{
					if (key is NSString str)
						defaults.RemoveObject(str);
				}
			}
		}

		public static void Set<T>(string key, T value)
		{
			using (var defaults = UserDefaults)
			{
				if (value == null)
				{
					if (defaults[key] != null)
						defaults.RemoveObject(key);

					return;
				}

				switch (value)
				{
					case string s:
						defaults.SetString(s, key);
						break;

					case int i:
						defaults.SetInt(i, key);
						break;

					case bool b:
						defaults.SetBool(b, key);
						break;

					case double d:
						defaults.SetDouble(d, key);
						break;

					case float f:
						defaults.SetFloat(f, key);
						break;
				}
			}
		}

		public static T Get<T>(string key, T fallback)
		{
			object value = null;

			using (var defaults = UserDefaults)
			{
				if (defaults[key] == null)
					return fallback;
				
				switch (fallback)
				{
					case int _:
						value = (int) defaults.IntForKey(key);
						break;
					
					case bool _:
						value = defaults.BoolForKey(key);
						break;
					
					case double _:
						value = defaults.DoubleForKey(key);
						break;
					
					case float _:
						value = defaults.FloatForKey(key);
						break;

					case string _:
						value = defaults.StringForKey(key);
						break;
					}
				}

			return (T) value;
		}
	}
}