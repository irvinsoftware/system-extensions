using System;
using System.Reflection;

namespace Irvin.Extensions.Reflection;

internal class MethodInfoWrapper : IMemberInfo
{
    public MethodInfoWrapper(MethodInfo methodInfo)
    {
        Underlying = methodInfo;
    }

    private MethodInfo Underlying { get; }

    public string Name => Underlying.Name;
    public Type MemberType => Underlying.ReturnType;
    public IMemberContainer Container => throw new NotImplementedException();

    public bool IsPublic => Underlying.IsPublic;
    private bool IsPrivate => Underlying.IsPrivate;
    private bool IsInternal => Underlying.IsFamilyOrAssembly;
    private bool IsProtected => Underlying.IsFamily;
    public bool IsInternalNotProtected => IsInternal && !IsProtected;
    public bool IsProtectedInternal => IsProtected && IsInternal;
    public bool IsProtectedNotPrivate => IsProtected && !IsPrivate;
    public bool IsPrivateNotProtected => IsPrivate && !IsProtected;
    
    public object GetValue(object source)
    {
        throw new NotSupportedException();
    }

    public bool SetValue(object target, object value)
    {
        throw new NotSupportedException();
    }
    
    public bool Equals(IMemberInfo other)
    {
        throw new NotImplementedException();
    }
}