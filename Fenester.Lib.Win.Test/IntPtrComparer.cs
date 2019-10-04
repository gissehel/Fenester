using System;
using System.Collections.Generic;

namespace Fenester.Lib.Win.Test
{
    public class IntPtrComparer : IComparer<IntPtr>
    {
        public int Compare(IntPtr x, IntPtr y) => x.ToInt64().CompareTo(y.ToInt64());
    }
}