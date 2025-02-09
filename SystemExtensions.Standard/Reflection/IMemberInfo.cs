using System;

namespace Irvin.Extensions.Reflection
{
    public interface IMemberInfo : IEquatable<IMemberInfo>
    {
        string Name { get; }
        Type MemberType { get; }
        IMemberContainer Container { get; }
        
        bool IsPublic { get; }
        bool IsInternalNotProtected { get; }
        bool IsProtectedInternal { get; }
        bool IsProtectedNotPrivate { get; }
        bool IsPrivateNotProtected { get; }

        object GetValue(object source);
        bool SetValue(object target, object value);
    }
}