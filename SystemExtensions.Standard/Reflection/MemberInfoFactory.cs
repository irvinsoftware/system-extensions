using System;
using System.Reflection;

namespace Irvin.Extensions.Reflection;

public class MemberInfoFactory
{
    public static IMemberInfo Get(MemberInfo memberInfo)
    {
        if (memberInfo is PropertyInfo)
        {
            return new PropertyInfoWrapper(memberInfo as PropertyInfo);
        }

        if (memberInfo is FieldInfo)
        {
            return new FieldInfoWrapper(memberInfo as FieldInfo);
        }

        throw new NotSupportedException();
    }
}