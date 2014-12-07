using System;

namespace Personadex.Utils
{
    internal struct Range
    {
        private readonly uint _start;
        public uint Start 
        {
            get
            {
                return _start;
            }
        }

        private readonly uint _end;
        public uint End
        {
            get
            {
                return _end;
            }
        }

        public Range(uint start, uint end)
        {
            if (end < start)
            {
                throw new ArgumentException("end must be greater than or equal to start");
            }

            _start = start;
            _end   = end;
        }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            hashCode     = hashCode * 23 + _start.GetHashCode();
            hashCode     = hashCode * 23 + _end.GetHashCode();

            return hashCode;
        }

        public override string ToString()
        {
            return "Range: " + _start + "," + _end;
        }
    }
}
