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
    public string Name => UnderlyingMember.Name;
    public Type DataType => (UnderlyingMember as FieldInfo)?.FieldType ?? (UnderlyingMember as PropertyInfo)?.PropertyType;
    public object Value { get; set; }

    public void SetOn(object target)
    {
        PropertyInfo propertyInfo = UnderlyingMember as PropertyInfo;
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(target, Value);
        }
        else
        {
            FieldInfo fieldInfo = UnderlyingMember as FieldInfo;
            if (fieldInfo != null)
            {
                fieldInfo.SetValueDirect(__makeref(target), Value); //TODO: find better implementation
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}