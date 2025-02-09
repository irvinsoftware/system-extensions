using System;

namespace Irvin.Extensions.Reflection
{
    internal class FieldInfoWrapper : IMemberInfo
    {
        private readonly System.Reflection.FieldInfo _fieldInfo;

        public FieldInfoWrapper(System.Reflection.FieldInfo fieldInfo)
        {
            _fieldInfo = fieldInfo;
        }

        public bool Equals(IMemberInfo other)
        {
            FieldInfoWrapper otherField = other as FieldInfoWrapper;

            if (otherField == null)
            {
                return false;
            }

            return _fieldInfo.Equals(otherField._fieldInfo);
        }

        public string Name => _fieldInfo.Name;
        public Type MemberType => _fieldInfo.FieldType;
        public IMemberContainer Container => new TypeInfo(_fieldInfo.ReflectedType);

        public bool IsPublic => _fieldInfo.IsPublic;
        private bool IsPrivate => _fieldInfo.IsPrivate;
        private bool IsInternal => _fieldInfo.IsFamilyOrAssembly;
        private bool IsProtected => _fieldInfo.IsFamily;
        public bool IsInternalNotProtected => IsInternal && !IsProtected;
        public bool IsProtectedInternal => IsProtected && IsInternal;
        public bool IsProtectedNotPrivate => IsProtected && !IsPrivate;
        public bool IsPrivateNotProtected => IsPrivate && !IsProtected;

        public bool SetValue(object target, object value)
        {
            _fieldInfo.SetValue(target, value);
            return true;
        }

        public object GetValue(object source)
        {
            return _fieldInfo.GetValue(source);
        }
    }
}