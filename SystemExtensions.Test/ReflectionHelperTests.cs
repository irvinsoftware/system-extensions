using System;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Metadata;
using NUnit.Framework;

namespace SystemExtensions.Test
{
	[TestFixture]
	public class ReflectionHelperTests
	{
		[Test]
		public void GetAttributeInstance_ReturnsSerializableAttribute_FromCookie()
		{
			SerializableAttribute actual = ReflectionExtensions.GetAttributeInstance<SerializableAttribute>(typeof(Cookie));

			Assert.IsNotNull(actual);
		}

		[Test]
		public void GetAttributeInstance_ReturnsNull_IfAttributeIsNotPresentOnClass()
		{
			TestFixtureAttribute actual = ReflectionExtensions.GetAttributeInstance<TestFixtureAttribute>(typeof(Cookie));

			Assert.IsNull(actual);
		}

		[Test]
		public void GetAttributeInstance_ReturnsAttribute_FromParameterInfo()
		{
			MethodInfo methodInfo = GetType().GetMethod("TheTestMethod", BindingFlags.NonPublic | BindingFlags.Instance);
			ParameterInfo parameterInfo = methodInfo.GetParameters()[0];

			SoapParameterAttribute actual = ReflectionExtensions.GetAttributeInstance<SoapParameterAttribute>(parameterInfo);

			Assert.IsNotNull(actual);
		}

		[Test]
		public void GetAttributeInstance_ReturnsNull_IfAttributeNotPresent()
		{
			MethodInfo methodInfo = GetType().GetMethod("TheTestMethod", BindingFlags.NonPublic | BindingFlags.Instance);
			ParameterInfo parameterInfo = methodInfo.GetParameters()[0];

			TestFixtureAttribute actual = ReflectionExtensions.GetAttributeInstance<TestFixtureAttribute>(parameterInfo);

			Assert.IsNull(actual);
		}

		[Test]
		public void IsPrimitiveType_ReturnsTrue_ForDateTime()
		{
			Assert.IsTrue(ReflectionExtensions.IsPrimitive(typeof(DateTime)));
		}

		[Test]
		public void IsPrimitiveType_ReturnsTrue_ForBoolean()
		{
			Assert.IsTrue(ReflectionExtensions.IsPrimitive(typeof(bool)));
		}

		[Test]
		public void IsPrimitiveType_ReturnsTrue_ForChar()
		{
			Assert.IsTrue(ReflectionExtensions.IsPrimitive(typeof(char)));
		}

		[Test]
		public void IsPrimitiveType_ReturnsTrue_ForDecimal()
		{
			Assert.IsTrue(ReflectionExtensions.IsPrimitive(typeof(decimal)));
		}

		[Test]
		public void IsPrimitiveType_ReturnsTrue_ForDouble()
		{
			Assert.IsTrue(ReflectionExtensions.IsPrimitive(typeof(double)));
		}

		[Test]
		public void IsPrimitiveType_ReturnsTrue_ForFloat()
		{
			Assert.IsTrue(ReflectionExtensions.IsPrimitive(typeof(float)));
		}

		[Test]
		public void IsPrimitiveType_ReturnsTrue_ForInt()
		{
			Assert.IsTrue(ReflectionExtensions.IsPrimitive(typeof(int)));
			Assert.IsTrue(ReflectionExtensions.IsPrimitive(typeof(uint)));
		}

		[Test]
		public void IsPrimitiveType_ReturnsTrue_ForLong()
		{
			Assert.IsTrue(ReflectionExtensions.IsPrimitive(typeof(long)));
			Assert.IsTrue(ReflectionExtensions.IsPrimitive(typeof(ulong)));
		}

		[Test]
		public void IsPrimitiveType_ReturnsTrue_ForShort()
		{
			Assert.IsTrue(ReflectionExtensions.IsPrimitive(typeof(short)));
			Assert.IsTrue(ReflectionExtensions.IsPrimitive(typeof(ushort)));
		}

		[Test]
		public void IsPrimitiveType_ReturnsTrue_ForByte()
		{
			Assert.IsTrue(ReflectionExtensions.IsPrimitive(typeof(byte)));
			Assert.IsTrue(ReflectionExtensions.IsPrimitive(typeof(sbyte)));
		}

		[Test]
		public void IsPrimitiveType_ReturnsFalse_ForVoidType()
		{
			Assert.IsFalse(ReflectionExtensions.IsPrimitive(typeof(void)));
		}

		[Test]
		public void IsPrimitiveType_ReturnsFalse_ForClass()
		{
			Assert.IsFalse(ReflectionExtensions.IsPrimitive(typeof(Cookie)));
		}

		[Test]
		public void IsPrimitiveType_ReturnsFalse_ForCommonStruct()
		{
			Assert.IsFalse(ReflectionExtensions.IsPrimitive(typeof(ConsoleKeyInfo)));
		}

		[Test]
		public void IsPrimitiveType_ReturnsTrue_ForString()
		{
			Assert.IsTrue(ReflectionExtensions.IsPrimitive(typeof(string)));
		}

		private void TheTestMethod([SoapParameter] string a)
		{
		}
	}
}