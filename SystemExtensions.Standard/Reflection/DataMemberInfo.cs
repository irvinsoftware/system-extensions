using System;
using System.Reflection;

namespace Irvin.Extensions.Reflection;

public class DataMemberInfo
{
    public DataMemberInfo(MemberInfo memberInfo)
    {
        UnderlyingMember = memberInfo;
    }

    private MemberInfo UnderlyingMember { get; }
    private IMemberInfo Member => MemberInfoFactory.Get(UnderlyingMember);
    public string ParentName => (UnderlyingMember.ReflectedType ?? UnderlyingMember.DeclaringType)?.Name;
    public string Name => UnderlyingMember.Name;
    public Type DataType => (UnderlyingMember as FieldInfo)?.FieldType ?? (UnderlyingMember as PropertyInfo)?.PropertyType;
    public MemberTypes MemberKind => UnderlyingMember.MemberType;
    public object Value { get; set; }

    public bool CanSet
    {
        get
        {
            PropertyInfo propertyInfo = UnderlyingMember as PropertyInfo;
            if (propertyInfo != null)
            {
                return propertyInfo.CanWrite;
            }

            return true;
        }
    }

    public bool IsPublic => Member.IsPublic;
    public bool IsInternalNotProtected => Member.IsInternalNotProtected;
    public bool IsProtectedNotPrivate => Member.IsProtectedNotPrivate;
    public bool IsPrivateNotProtected => Member.IsPrivateNotProtected;
    public bool IsProtectedInternal => Member.IsProtectedInternal;

    public void SetOn(object target)
    {
        PropertyInfo propertyInfo = UnderlyingMember as PropertyInfo;
        if (propertyInfo != null)
        {
            try
            {
                object convertedValue = DataType == typeof(decimal) ? Convert.ToDecimal(Value) : Value;
                propertyInfo.SetValue(target, convertedValue);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not bind '{propertyInfo.Name}' on '{ParentName}'", exception);
            }
        }
        else
        {
            FieldInfo fieldInfo = UnderlyingMember as FieldInfo;
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(target, Value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}