using System;
using System.Reflection;

namespace Irvin.Extensions.Reflection
{
    public class PropertyInfoWrapper : IMemberInfo
    {
        private readonly PropertyInfo _propertyInfo;

        public PropertyInfoWrapper(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
            Getter = new MethodInfoWrapper(_propertyInfo.GetGetMethod());
            Setter = new MethodInfoWrapper(_propertyInfo.GetSetMethod());
        }
        
        private MethodInfoWrapper Getter { get; }
        private MethodInfoWrapper Setter { get; }
        
        public override int GetHashCode()
        {
            return _propertyInfo.GetHashCode();
        }

        public bool Equals(IMemberInfo other)
        {
            PropertyInfoWrapper other2 = other as PropertyInfoWrapper;
            if (other2 == null)
            {
                return false;
            }

            return _propertyInfo.Equals(other2._propertyInfo);
        }

        public string Name => _propertyInfo.Name;
        public Type MemberType => _propertyInfo.PropertyType;
        public IMemberContainer Container => new TypeInfo(_propertyInfo.ReflectedType);

        public bool IsPublic => Getter?.IsPublic == true || Setter?.IsPublic == true;
        
        public bool IsInternalNotProtected => throw new NotImplementedException();
        public bool IsProtectedInternal => throw new NotImplementedException();
        public bool IsProtectedNotPrivate => throw new NotImplementedException();

        public bool IsPrivateNotProtected => Getter?.IsPrivateNotProtected == true &&
                                             Setter?.IsPrivateNotProtected == true;

        public bool SetValue(object target, object value)
        {
            if (_propertyInfo.GetSetMethod(nonPublic: true) != null)
            {
                _propertyInfo.SetValue(target, value, null);
                return true;
            }

            return false;
        }

        public object GetValue(object source)
        {
            return _propertyInfo.GetValue(source, null);
        }
    }
}