using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Irvin.Core
{
	public class Path
	{
		private const string BOTH_DELIMITERS = "[\\\\/]";

		private List<string> _parts;
		private char _queryDelimiter;

		public Path()
		{
			_parts = new List<string>();
			QueryParts = new Dictionary<string, string>();
		}

		public PathStyle Style { get; set; }

		private char PartDelimiter
		{
			get { return (Style == PathStyle.Windows) ? '\\' : '/'; }
		}

		public string Scheme { get; private set; }
		public string Host { get; private set; }
		public int? Port { get; private set; }

		public int NumberOfSubPathParts
		{
			get
			{
				if (Style == PathStyle.Standard && _parts.Count == 2 && _parts[1] == string.Empty)
				{
					return 0;
				}
				return FullSubPath.Split(PartDelimiter).Length - 1;
			}
		}

		public string FullSubPath
		{
			get
			{
				switch (Style)
				{
					case PathStyle.Standard:
					case PathStyle.Uri:
						return string.Join(PartDelimiter.ToString(), _parts.ToArray());
					case PathStyle.Windows:
						List<string> parts = new List<string>();
						parts.Add(string.Empty);
						for (int i = 1; i < _parts.Count; i++)
						{
							parts.Add(_parts[i]);
						}
						return string.Join(PartDelimiter.ToString(), parts.ToArray());
					default:
						throw new NotSupportedException();
				}
			}
		}

		public string ItemAndExtension
		{
			get
			{
				string value = _parts[_parts.Count - 1];
				return EmptyIsNull(value);
			}
		}

		public string ItemOnly
		{
			get
			{
				if (ItemAndExtension != null)
				{
					string[] smallerParts = ItemAndExtension.Split('.');
					return smallerParts[0];
				}
				return null;
			}
		}

		public string Extension
		{
			get
			{
				if (ItemAndExtension != null)
				{
					string[] smallerParts = ItemAndExtension.Split('.');
					if (smallerParts.Length > 1)
					{
						return smallerParts[smallerParts.Length - 1];
					}
				}
				return null;
			}
		}

		public string Anchor { get; set; }
		public string Query { get; set; }
		public Dictionary<string, string> QueryParts { get; private set; }

		public Path GetParent(int backtracks)
		{
			if (backtracks > NumberOfSubPathParts)
			{
				return null;
			}
			if (Style == PathStyle.Standard && _parts.Count == 2 && _parts[1] == string.Empty)
			{
				return null;
			}
			if (backtracks == 0)
			{
				return ToString();
			}

			int partsToTake = _parts.Count - backtracks;
			string[] parts = new string[partsToTake + 1];
			_parts.CopyTo(0, parts, 0, partsToTake);
			parts[partsToTake] = string.Empty;

			Path parent = string.Join(PartDelimiter.ToString(), parts);
			parent.Scheme = Scheme;
			parent.Host = Host;
			parent.Port = Port;
			parent.Style = Style;
			return parent;
		}

		public Path GetSubPath(int backtracks)
		{
			if (backtracks > NumberOfSubPathParts)
			{
				throw new ArgumentException("Cannot backtrack that far.");
			}

			Path subPath = null;

			string value = string.Join("/", _parts.ToArray(), _parts.Count - backtracks, backtracks);
			if (!string.IsNullOrEmpty(value))
			{
				subPath = value;
				subPath.Style = Style;
			}

			return subPath;
		}

		public override string ToString()
		{
			switch (Style)
			{
				case PathStyle.Uri:
					if (string.IsNullOrEmpty(Scheme) && string.IsNullOrEmpty(Host))
					{
						return FullSubPath;
					}
					return string.Format("{0}://{1}{2}", Scheme, Host, FullSubPath);
				case PathStyle.Windows:
					return string.Join(PartDelimiter.ToString(), _parts.ToArray());
				case PathStyle.Standard:
					return FullSubPath;
				default:
					throw new NotSupportedException();
			}
		}

		public static implicit operator string(Path a)
		{
			return a == null ? null : a.ToString();
		}

		public static implicit operator Path(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return null;
			}

			Path p = new Path();

			if (Regex.IsMatch(s, "^([a-z]+)?://"))
			{
				p.Style = PathStyle.Uri;
				Uri uri = new Uri(s);
				p.Scheme = uri.Scheme;
				p.Host = uri.Host;
				p.Port = uri.Port;
				p.Query = EmptyIsNull(uri.Query);
				p._parts = new List<string>(uri.AbsolutePath.Split('/'));
			}
			else if (Regex.IsMatch(s, "^([a-zA-Z]:)?\\\\"))
			{
				p.Style = PathStyle.Windows;
				p._parts = new List<string>(s.Split('\\'));
				p.Host = p._parts[0];
			}
			else if (s.Contains("\\"))
			{
				p.Style = PathStyle.Windows;
				p._parts = new List<string>(s.Split('\\'));
			}
			else 
			{
				p.Style = PathStyle.Standard;
				p._parts = new List<string>(s.Split('/'));
			}

			return p;
		}

		public static Path operator +(Path a, Path b)
		{
			throw new NotImplementedException();
		}

		public static Path operator +(Path sourcePath, string newContent)
		{
			if (sourcePath == null)
			{
				return newContent;
			}

			Path p = sourcePath.ToString();
			
			if (!string.IsNullOrEmpty(newContent))
			{
				if (Regex.IsMatch(newContent[0].ToString(), BOTH_DELIMITERS))
				{
					newContent = newContent.Remove(0, 1);
				}

				if (newContent.Length != 0)
				{
					int lastPartIndex = p._parts.Count - 1;
					if (string.IsNullOrEmpty(p._parts[lastPartIndex]))
					{
						p._parts.RemoveAt(lastPartIndex);
					}
					string[] newParts = Regex.Split(newContent, BOTH_DELIMITERS);
					p._parts.AddRange(newParts);
				}
			}

			return p;
		}

		public static Path operator -(Path a, int i)
		{
			return a.GetParent(i);
		}

		public static bool operator ==(Path a, Path b)
		{
			return AreEqual(a, b);
		}

		public static bool operator !=(Path a, Path b)
		{
			return !AreEqual(a, b);
		}

		private static bool AreEqual(Path a, Path b)
		{
			if (ReferenceEquals(null, a) && ReferenceEquals(null, b))
			{
				return true;
			}
			if (ReferenceEquals(null, a))
			{
				return false;
			}
			if (ReferenceEquals(null, b))
			{
				return false;
			}
			throw new NotImplementedException();
		}

		private static string EmptyIsNull(string value)
		{
			return value == string.Empty ? null : value;
		}
	}
}