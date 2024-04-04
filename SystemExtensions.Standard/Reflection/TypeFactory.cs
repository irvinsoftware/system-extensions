using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Irvin.Extensions.Reflection;

public static class TypeFactory
{
    private static readonly ConcurrentDictionary<Type, IEnumerable<MemberInfo>> _typeMemberCache = new ConcurrentDictionary<Type, IEnumerable<MemberInfo>>();

    public static IEnumerable<MemberInfo> GetTypeMembers(Type elementType)
    {
        return _typeMemberCache.GetOrAdd(elementType, type =>
        {
            List<MemberInfo> typeMembers = new List<MemberInfo>();

            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            typeMembers.AddRange(type.GetFields(bindingFlags));
            typeMembers.AddRange(type.GetProperties(bindingFlags));

            return typeMembers.Where(member => member.CustomAttributes.All(x => x.AttributeType != typeof(CompilerGeneratedAttribute)));
        });
    }
}