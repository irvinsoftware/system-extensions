using System;
using System.Data;
using System.Reflection;

namespace Irvin.Extensions.Reflection;

public class DataMemberInfo
{
    public DataMemberInfo(MemberInfo memberInfo)
    {
        UnderlyingMember = memberInfo;
    }

    private MemberInfo UnderlyingMember { get; }
    public string ParentName => (UnderlyingMember.ReflectedType ?? UnderlyingMember.DeclaringType)?.Name;
    public string Name => UnderlyingMember.Name;
    public Type DataType => (UnderlyingMember as FieldInfo)?.FieldType ?? (UnderlyingMember as PropertyInfo)?.PropertyType;
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
    
    public void SetOn(object target)
    {
        PropertyInfo propertyInfo = UnderlyingMember as PropertyInfo;
        if (propertyInfo != null)
        {
            try
            {
                propertyInfo.SetValue(target, Value);
            }
            catch (Exception exception)
            {
                throw new TargetInvocationException($"Could not bind '{propertyInfo.Name}' on '{ParentName}'", exception);
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