using System.Collections.Generic;

namespace System
{
	public static class EnumHelper
	{
		public static T Parse<T>(string value, bool ignoreCase = true)
		{
			return (T)Enum.Parse(typeof(T), value, ignoreCase);
		}

		public static List<T> GetAll<T>()
		{
			List<T> items = new List<T>();
			string[] names = Enum.GetNames(typeof(T));
			foreach (string name in names)
			{
				items.Add(Parse<T>(name));
			}
			return items;
		}
	}
}