using Irvin.Core;
using NUnit.Framework;

namespace Irvin.Framework.Tests
{
	[TestFixture]
	public class PathTests
	{
		[Test]
		public void implictToPath_ReturnsNull_IfInputIsNull()
		{
			string address = null;

			Path classUnderTest = address;

			Assert.IsNull(classUnderTest);
		}

		[Test]
		public void implicitToString_ReturnsNull_IfTargetIsNull()
		{
			Path path = null;

			string actual = path;

			Assert.IsNull(actual);
		}

		[Test]
		public void implictToPath_BreaksUrlIntoCorrectParts_ForPlainUrl()
		{
			Path classUnderTest = "http://msdn.microsoft.com/en-us/library/53k8ybth.aspx";

			Assert.AreEqual(PathStyle.Uri, classUnderTest.Style);
			Assert.AreEqual("http", classUnderTest.Scheme);
			Assert.AreEqual("msdn.microsoft.com", classUnderTest.Host);
			Assert.AreEqual(80, classUnderTest.Port);
			Assert.AreEqual("/en-us/library/53k8ybth.aspx", classUnderTest.FullSubPath);
			Assert.AreEqual("53k8ybth.aspx", classUnderTest.ItemAndExtension);
			Assert.AreEqual("53k8ybth", classUnderTest.ItemOnly);
			Assert.IsNull(classUnderTest.Query);
			Assert.AreEqual(0, classUnderTest.QueryParts.Count);
			Assert.IsNull(classUnderTest.Anchor);
			Assert.AreEqual("aspx", classUnderTest.Extension);
		}

		[Test]
		public void add_operator_CreatesAppendedPath_ForAbsolutePlusRelativeUrl()
		{
			Path left = "http://msdn.microsoft.com/en-us/library/53k8ybth.aspx";
			string right = "/zuko";

			Path actual = left + right;

			Assert.AreEqual("http://msdn.microsoft.com/en-us/library/53k8ybth.aspx/zuko", actual.ToString());
		}

		[Test]
		public void GetSubPath_ReturnsFragments_ForPlainUrl()
		{
			Path classUnderTest = "http://msdn.microsoft.com/en-us/library/53k8ybth.aspx";

			Assert.IsNull(classUnderTest.GetSubPath(0));
			Assert.AreEqual("53k8ybth.aspx", classUnderTest.GetSubPath(1).ToString());
			Assert.AreEqual("library/53k8ybth.aspx", classUnderTest.GetSubPath(2).ToString());
			Assert.AreEqual("en-us/library/53k8ybth.aspx", classUnderTest.GetSubPath(3).ToString());
		}

		[Test]
		public void GetParent_ReturnsValues_ForPlainUrl()
		{
			string URL = "http://msdn.microsoft.com/en-us/library/53k8ybth.aspx";

			Path classUnderTest = URL;

			Assert.AreEqual(URL, classUnderTest.GetParent(0).ToString());
			Assert.AreEqual("http://msdn.microsoft.com/en-us/library/", classUnderTest.GetParent(1).ToString());
			Assert.AreEqual("http://msdn.microsoft.com/en-us/", classUnderTest.GetParent(2).ToString());
			Assert.AreEqual("http://msdn.microsoft.com/", classUnderTest.GetParent(3).ToString());
			Assert.IsNull(classUnderTest.GetParent(4));
			Assert.IsNull(classUnderTest.GetParent(5));
		}

		[Test]
		public void get_NumberOfSubPathParts_ReturnsValue_ForPlainUrl()
		{
			Path classUnderTest = "http://msdn.microsoft.com/en-us/library/53k8ybth.aspx";

			Assert.AreEqual(3, classUnderTest.NumberOfSubPathParts);
		}

		[Test]
		[ExpectedException]
		public void GetSubPath_ThrowsException_ForPlainUrl_IfBacktrackedTooFar()
		{
			Path classUnderTest = "http://msdn.microsoft.com/en-us/library/53k8ybth.aspx";

			classUnderTest.GetSubPath(4);
		}

		[Test]
		public void ToString_ReturnsAddress_ForPlainUrl()
		{
			const string URL = "http://msdn.microsoft.com/en-us/library/53k8ybth.aspx";

			Path classUnderTest = URL;

			Assert.AreEqual(URL, classUnderTest.ToString());
		}

		[Test]
		public void implictToPath_BreaksPathIntoCorrectParts_ForSmallestStandardPath()
		{
			Path classUnderTest = "/";

			Assert.AreEqual(PathStyle.Standard, classUnderTest.Style);
			Assert.IsNull(classUnderTest.Scheme);
			Assert.IsNull(classUnderTest.Host);
			Assert.IsFalse(classUnderTest.Port.HasValue);
			Assert.AreEqual("/", classUnderTest.FullSubPath);
			Assert.IsNull(classUnderTest.ItemAndExtension);
			Assert.IsNull(classUnderTest.ItemOnly);
			Assert.IsNull(classUnderTest.Query);
			Assert.AreEqual(0, classUnderTest.QueryParts.Count);
			Assert.IsNull(classUnderTest.Anchor);
			Assert.IsNull(classUnderTest.Extension);
		}

		[Test]
		public void add_operator_ReturnsAppendedPath_ForTwoStandardPaths()
		{
			Path left = "/";
			string right = "zulu";

			Path actual = left + right;

			Assert.AreEqual("/zulu", actual.ToString());
		}

		[Test]
		public void add_operator_ReturnsAppendedPath_ForTwoStandardRootPaths()
		{
			Path left = "/zarg";
			string right = "/zulu";

			Path actual = left + right;

			Assert.AreEqual("/zarg/zulu", actual.ToString());
		}

		[Test]
		public void ToString_ReturnsValue_ForSmallestStandardPath()
		{
			const string standardPath = "/";

			Path classUnderTest = standardPath;

			Assert.AreEqual(standardPath, classUnderTest.ToString());
		}

		[Test]
		public void GetParent_ReturnsNull_ForSmallestStandardPath()
		{
			Path classUnderTest = "/";

			Assert.IsNull(classUnderTest.GetParent(0));
			Assert.IsNull(classUnderTest.GetParent(1));
		}

		[Test]
		public void GetSubPath_ReturnsFragments_SmallestStandardPath()
		{
			Path classUnderTest = "/";

			Assert.IsNull(classUnderTest.GetSubPath(0));
		}

		[Test]
		public void get_NumberOfSubPathParts_ReturnsValue_SmallestStandardPath()
		{
			Path classUnderTest = "/";

			Assert.AreEqual(0, classUnderTest.NumberOfSubPathParts);
		}

		[Test]
		[ExpectedException]
		public void GetSubPath_ThrowsException_SmallestStandardPath_IfBacktrackedTooFar()
		{
			Path classUnderTest = "/";

			classUnderTest.GetSubPath(1);
		}

		[Test]
		public void implictToPath_BreaksPathIntoCorrectParts_ForUnextendedStandardPath()
		{
			Path classUnderTest = "/a/b/c";

			Assert.AreEqual(PathStyle.Standard, classUnderTest.Style);
			Assert.IsNull(classUnderTest.Scheme);
			Assert.IsNull(classUnderTest.Host);
			Assert.IsFalse(classUnderTest.Port.HasValue);
			Assert.AreEqual("/a/b/c", classUnderTest.FullSubPath);
			Assert.AreEqual("c", classUnderTest.ItemAndExtension);
			Assert.AreEqual("c", classUnderTest.ItemOnly);
			Assert.IsNull(classUnderTest.Query);
			Assert.AreEqual(0, classUnderTest.QueryParts.Count);
			Assert.IsNull(classUnderTest.Anchor);
			Assert.IsNull(classUnderTest.Extension);
		}

		[Test]
		public void ToString_ReturnsValue_ForUnextendedStandardPath()
		{
			string standardPath = "/a/b/c";

			Path classUnderTest = standardPath;

			Assert.AreEqual(standardPath, classUnderTest.ToString());
		}

		[Test]
		public void GetSubPath_ReturnsFragments_ForUnextendedStandardPath()
		{
			Path classUnderTest = "/a/b/c";

			Assert.IsNull(classUnderTest.GetSubPath(0));
			Assert.AreEqual("c", classUnderTest.GetSubPath(1).ToString());
			Assert.AreEqual("b/c", classUnderTest.GetSubPath(2).ToString());
			Assert.AreEqual("a/b/c", classUnderTest.GetSubPath(3).ToString());
		}

		[Test]
		public void GetParent_ReturnsValue_ForUnextendedStandardPath()
		{
			const string ADDRESS = "/a/b/c";

			Path classUnderTest = ADDRESS;

			Assert.AreEqual(ADDRESS, classUnderTest.GetParent(0).ToString());
			Assert.AreEqual("/a/b/", classUnderTest.GetParent(1).ToString());
			Assert.AreEqual("/a/", classUnderTest.GetParent(2).ToString());
			Assert.AreEqual("/", classUnderTest.GetParent(3).ToString());
			Assert.IsNull(classUnderTest.GetParent(4));
			Assert.IsNull(classUnderTest.GetParent(5));
		}

		[Test]
		public void get_NumberOfSubPathParts_ReturnsValue_ForUnextendedStandardPath()
		{
			Path classUnderTest = "/a/b/c";

			Assert.AreEqual(3, classUnderTest.NumberOfSubPathParts);
		}

		[Test]
		[ExpectedException]
		public void GetSubPath_ThrowsException_ForPlainUrl_ForUnextendedStandardPath()
		{
			Path classUnderTest = "/a/b/c";

			classUnderTest.GetSubPath(4);
		}

		[Test]
		public void implictToPath_BreaksPathIntoCorrectParts_ForWindowsDirectoryPath()
		{
			Path classUnderTest = @"C:\Personal\Entertainment\Music\Comedy Sketches";

			Assert.AreEqual(PathStyle.Windows, classUnderTest.Style);
			Assert.IsNull(classUnderTest.Scheme);
			Assert.AreEqual("C:", classUnderTest.Host);
			Assert.IsFalse(classUnderTest.Port.HasValue);
			Assert.AreEqual(@"\Personal\Entertainment\Music\Comedy Sketches", classUnderTest.FullSubPath);
			Assert.AreEqual("Comedy Sketches", classUnderTest.ItemAndExtension);
			Assert.AreEqual("Comedy Sketches", classUnderTest.ItemOnly);
			Assert.IsNull(classUnderTest.Query);
			Assert.AreEqual(0, classUnderTest.QueryParts.Count);
			Assert.IsNull(classUnderTest.Anchor);
			Assert.IsNull(classUnderTest.Extension);
		}

		[Test]
		public void add_operator_ReturnsAppendedPath_ForWindowsFullPlusRelative()
		{
			Path left = @"C:\Personal\Entertainment\Music\Comedy Sketches";
			string right = "02 Working at the Burger King.m4a";

			Path actual = left + right;

			Assert.AreEqual(@"C:\Personal\Entertainment\Music\Comedy Sketches\02 Working at the Burger King.m4a", actual.ToString());
		}

		[Test]
		public void ToString_ReturnsValid_ForWindowsDirectoryPath()
		{
			const string FILE_PATH = @"C:\Personal\Entertainment\Music\Comedy Sketches";

			Path classUnderTest = FILE_PATH;

			Assert.AreEqual(FILE_PATH, classUnderTest.ToString());
		}

		[Test]
		public void GetSubPath_ReturnsFragments_ForWindowsDirectoryPath()
		{
			Path classUnderTest = @"C:\Personal\Entertainment\Music\Comedy Sketches";

			Assert.IsNull(classUnderTest.GetSubPath(0));
			Assert.AreEqual("Comedy Sketches", classUnderTest.GetSubPath(1).ToString());
			Assert.AreEqual(@"Music\Comedy Sketches", classUnderTest.GetSubPath(2).ToString());
			Assert.AreEqual(@"Entertainment\Music\Comedy Sketches", classUnderTest.GetSubPath(3).ToString());
			Assert.AreEqual(@"Personal\Entertainment\Music\Comedy Sketches", classUnderTest.GetSubPath(4).ToString());
		}

		[Test]
		public void GetParent_ReturnsValues_ForWindowsDirectoryPath()
		{
			Path classUnderTest = @"C:\Personal\Entertainment\Music\Comedy Sketches";

			Assert.AreEqual(@"C:\Personal\Entertainment\Music\Comedy Sketches", classUnderTest.GetParent(0).ToString());
			Assert.AreEqual(@"C:\Personal\Entertainment\Music\", classUnderTest.GetParent(1).ToString());
			Assert.AreEqual(@"C:\Personal\Entertainment\", classUnderTest.GetParent(2).ToString());
			Assert.AreEqual(@"C:\Personal\", classUnderTest.GetParent(3).ToString());
			Assert.AreEqual(@"C:\", classUnderTest.GetParent(4).ToString());
			Assert.IsNull(classUnderTest.GetParent(5));
			Assert.IsNull(classUnderTest.GetParent(6));
		}

		[Test]
		public void get_NumberOfSubPathParts_ReturnsValue_ForWindowsDirectoryPath()
		{
			Path classUnderTest = @"C:\Personal\Entertainment\Music\Comedy Sketches";

			Assert.AreEqual(4, classUnderTest.NumberOfSubPathParts);
		}

		[Test]
		[ExpectedException]
		public void GetSubPath_ThrowsException_ForPlainUrl_ForWindowsDirectoryPath()
		{
			Path classUnderTest = @"C:\Personal\Entertainment\Music\Comedy Sketches";

			classUnderTest.GetSubPath(5);
		}

		[Test]
		public void implictToPath_BreaksPathIntoCorrectParts_ForWindowsFullPath()
		{
			Path classUnderTest = @"C:\Personal\Entertainment\Music\Comedy Sketches\02 Working at the Burger King.m4a";

			Assert.AreEqual(PathStyle.Windows, classUnderTest.Style);
			Assert.IsNull(classUnderTest.Scheme);
			Assert.AreEqual("C:", classUnderTest.Host);
			Assert.IsFalse(classUnderTest.Port.HasValue);
			Assert.AreEqual(@"\Personal\Entertainment\Music\Comedy Sketches\02 Working at the Burger King.m4a", classUnderTest.FullSubPath);
			Assert.AreEqual(@"02 Working at the Burger King.m4a", classUnderTest.ItemAndExtension);
			Assert.AreEqual(@"02 Working at the Burger King", classUnderTest.ItemOnly);
			Assert.IsNull(classUnderTest.Query);
			Assert.AreEqual(0, classUnderTest.QueryParts.Count);
			Assert.IsNull(classUnderTest.Anchor);
			Assert.AreEqual(@"m4a", classUnderTest.Extension);
		}

		[Test]
		public void ToString_ReturnsValue_ForWindowsFullPath()
		{
			const string FILE_PATH = @"C:\Personal\Entertainment\Music\Comedy Sketches\02 Working at the Burger King.m4a";

			Path classUnderTest = FILE_PATH;

			Assert.AreEqual(FILE_PATH, classUnderTest.ToString());
		}

		[Test]
		public void GetSubPath_ReturnsFragments_ForWindowsFullPath()
		{
			Path classUnderTest = @"C:\Personal\Entertainment\Music\Comedy Sketches\02 Working at the Burger King.m4a";

			Assert.IsNull(classUnderTest.GetSubPath(0));
			Assert.AreEqual("02 Working at the Burger King.m4a", classUnderTest.GetSubPath(1).ToString());
			Assert.AreEqual(@"Comedy Sketches\02 Working at the Burger King.m4a", classUnderTest.GetSubPath(2).ToString());
			Assert.AreEqual(@"Music\Comedy Sketches\02 Working at the Burger King.m4a", classUnderTest.GetSubPath(3).ToString());
			Assert.AreEqual(@"Entertainment\Music\Comedy Sketches\02 Working at the Burger King.m4a", classUnderTest.GetSubPath(4).ToString());
			Assert.AreEqual(@"Personal\Entertainment\Music\Comedy Sketches\02 Working at the Burger King.m4a", classUnderTest.GetSubPath(5).ToString());
		}

		[Test]
		public void GetParent_ReturnsValues_ForWindowsFullPath()
		{
			Path classUnderTest = @"C:\Personal\Entertainment\Music\Comedy Sketches\02 Working at the Burger King.m4a";

			Assert.AreEqual(@"C:\Personal\Entertainment\Music\Comedy Sketches\02 Working at the Burger King.m4a", classUnderTest.GetParent(0).ToString());
			Assert.AreEqual(@"C:\Personal\Entertainment\Music\Comedy Sketches\", classUnderTest.GetParent(1).ToString());
			Assert.AreEqual(@"C:\Personal\Entertainment\Music\", classUnderTest.GetParent(2).ToString());
			Assert.AreEqual(@"C:\Personal\Entertainment\", classUnderTest.GetParent(3).ToString());
			Assert.AreEqual(@"C:\Personal\", classUnderTest.GetParent(4).ToString());
			Assert.AreEqual(@"C:\", classUnderTest.GetParent(5).ToString());
			Assert.IsNull(classUnderTest.GetParent(6));
			Assert.IsNull(classUnderTest.GetParent(7));
		}

		[Test]
		public void get_NumberOfSubPathParts_ReturnsValue_ForWindowsFullPath()
		{
			Path classUnderTest = @"C:\Personal\Entertainment\Music\Comedy Sketches\02 Working at the Burger King.m4a";

			Assert.AreEqual(5, classUnderTest.NumberOfSubPathParts);
		}

		[Test]
		[ExpectedException]
		public void GetSubPath_ThrowsException_ForPlainUrl_ForWindowsFullPath()
		{
			Path classUnderTest = @"C:\Personal\Entertainment\Music\Comedy Sketches\02 Working at the Burger King.m4a";

			classUnderTest.GetSubPath(6);
		}
	}
}