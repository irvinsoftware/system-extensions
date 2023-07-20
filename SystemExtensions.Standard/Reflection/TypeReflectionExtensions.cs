using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Irvin.Extensions.Collections;

namespace Irvin.Extensions.Reflection;

public static class TypeReflectionExtensions
{
    public static IReadOnlyCollection<DataMemberInfo> GetBinders(this Type type)
    {
        return type
                .GetMembers()
                .Where(memberInfo => memberInfo.MemberType == MemberTypes.Property || 
                                     memberInfo.MemberType == MemberTypes.Field)
                .Select(memberInfo => new DataMemberInfo(memberInfo))
                .ToList();
    }

    public static T Build<T>(this IEnumerable<DataMemberInfo> dataMemberInfos)
    {
        Type targetType = typeof(T);

        // is a struct or a POCO ('plain-old-c#-object')
        if (!targetType.IsClass || targetType.HasParameterlessConstructor())
        {
            return BuildDefaultInitObject<T>(dataMemberInfos);
        }

        // is a record, or a class with a constructor that fills all properties
        T dataRecord = BuildDataRecordObject<T>(dataMemberInfos);
        if (dataRecord == null)
        {
            throw new MissingMethodException("Could not find a way to initialize this type.");
        }

        return dataRecord;
    }

    public static bool HasParameterlessConstructor(this Type type)
    {
        return type.GetConstructors().Any(x => x.GetParameters().None());
    }

    private static T BuildDefaultInitObject<T>(IEnumerable<DataMemberInfo> dataMemberInfos)
    {
        //for some reason SetOn() works for "object" but not "T"
        object obj = Activator.CreateInstance<T>();
        
        foreach (DataMemberInfo dataMemberInfo in dataMemberInfos.Where(x => x.CanSet))
        {
            dataMemberInfo.SetOn(obj);
        }

        return (T) obj;
    }

    private static T BuildDataRecordObject<T>(IEnumerable<DataMemberInfo> dataMemberInfos)
    {
        object[] parameterValues = null;
        
        ConstructorInfo potentialConstructor =
            typeof(T).GetConstructors()
                     .FirstOrDefault(constructorInfo => constructorInfo.GetParameters().Length == dataMemberInfos.Count());
        if (potentialConstructor != null)
        {
            List<ParameterInfo> parameterInfos = potentialConstructor.GetParameters().ToList();
            parameterValues = new object[parameterInfos.Count];
            foreach (DataMemberInfo dataMemberInfo in dataMemberInfos.Where(x => x.CanSet))
            {
                if (parameterValues != null)
                {
                    int matchingParameterIndex = parameterInfos.FindIndex(x => x.Name == dataMemberInfo.Name);
                    if (matchingParameterIndex >= 0)
                    {
                        parameterValues[matchingParameterIndex] = dataMemberInfo.Value;
                    }
                    else
                    {
                        parameterValues = null;
                    }
                }
            }
        }

        return (T)(parameterValues != null ? potentialConstructor.Invoke(parameterValues) : null);
    }
}