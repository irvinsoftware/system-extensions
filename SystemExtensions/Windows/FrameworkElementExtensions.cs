using System.Linq.Expressions;
using Binding = System.Windows.Data.Binding;

namespace System.Windows
{
	public static class FrameworkElementExtensions
	{
		/// <summary>
		/// Creates a {Binding} expression without magic strings
		/// </summary>
		/// <remarks>
		/// http://handcraftsman.wordpress.com/2008/11/11/how-to-get-c-property-names-without-magic-strings/
		/// http://msdn.microsoft.com/en-us/library/ms742863.aspx?appId=Dev10IDEF1&l=EN-US&k=k%28SYSTEM.WINDOWS.DATA.BINDING%29;k%28VS.XAMLEDITOR%29;k%28TargetFrameworkMoniker-%22.NETFRAMEWORK&k=VERSION=V4.0%22%29&rd=true
		/// </remarks>
		public static void Bind<TProperty>(this FrameworkElement target, DependencyProperty property, Expression<Func<TProperty>> expression)
		{
			Binding binding = new Binding(GetPropertyName(expression));
			binding.Source = target.DataContext;
			target.SetBinding(property, binding);
		}

		public static string GetPropertyName<T>(Expression<Func<T>> expression)
		{
			MemberExpression body = (MemberExpression)expression.Body;
			return body.Member.Name;
		}
	}
}