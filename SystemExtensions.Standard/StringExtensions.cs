using System;
using System.Text;

namespace Irvin.Extensions
{
	public static class StringExtensions
	{
		public static bool EqualsIgnoreCase(this string thisString, string compareTo)
		{
			return thisString.Equals(compareTo, StringComparison.InvariantCultureIgnoreCase);
		}

	    public static bool ContainsIgnoreCase(this string thisString, string compareTo)
	    {
	        return thisString.ToLower().Contains(compareTo.ToLower());
	    }

	    public static StringBuilder Clear(this StringBuilder sb)
	    {
	        sb.Length = 0;
            return sb;
	    }
	}
}