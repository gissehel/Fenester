using Fenester.Lib.Core.Domain.Os;
using System;
using System.Collections.Generic;

namespace Fenester.Lib.Win.Domain.Os
{
    public class WindowId : IWindowId
    {
        internal WindowId(IntPtr handle)
        {
            Handle = handle;
        }

        public IntPtr Handle { get; private set; }

        public string Canonical => string.Format("0x{0:x16}", Handle.ToInt64());

        public class WindowIdEqualityComparer : IEqualityComparer<IWindowId>
        {
            public bool Equals(IWindowId x, IWindowId y)
            {
                var xWindowId = x as WindowId;
                var yWindowId = y as WindowId;

                if (x != null && y != null)
                {
                    return xWindowId.Handle == yWindowId.Handle;
                }

                if ((x == null && y != null) || (x != null && y == null))
                {
                    return false;
                }

                return x == y;
            }

            public int GetHashCode(IWindowId obj)
            {
                var objWindowId = obj as WindowId;

                if (objWindowId != null)
                {
                    return objWindowId.Handle.GetHashCode();
                }

                return obj.GetHashCode();
            }
        }
    }
}