namespace System.Runtime.CompilerServices
{
	//http://msdn.microsoft.com/en-us/magazine/cc163317.aspx#S7
	//This exists to fool the compiler into thinking you can use extension method in .NET 3.0
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
	public class ExtensionAttribute : Attribute
	{
	}
}