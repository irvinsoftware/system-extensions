using System;

namespace Irvin.Extensions
{
    public struct UInt8
    {
        private byte Value { get; set; }
        
        public static implicit operator UInt8(int source)
        {
            if (source > byte.MaxValue || source < 0)
            {
                throw new InvalidCastException();
            }

            return new UInt8 { Value = (byte)source };
        }
    }
}