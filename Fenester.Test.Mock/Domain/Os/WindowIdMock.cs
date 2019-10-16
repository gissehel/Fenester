using Fenester.Lib.Core.Domain.Os;
using System.Collections.Generic;

namespace Fenester.Test.Mock.Domain.Os
{
    public class WindowIdMock : IWindowId
    {
        public WindowIdMock(string rawId)
        {
            RawId = rawId;
        }

        public string RawId { get; }

        public string Canonical => RawId;

        public class WindowIdMockEqualityComparer : IEqualityComparer<IWindowId>
        {
            public bool Equals(IWindowId x, IWindowId y)
            {
                if ((x is WindowIdMock xMock) && (y is WindowIdMock yMock))
                {
                    return xMock.RawId == yMock.RawId;
                }
                return false;
            }

            public int GetHashCode(IWindowId obj)
            {
                if (obj is WindowIdMock objMock)
                {
                    return objMock.RawId.GetHashCode();
                }
                return obj.GetHashCode();
            }
        }
    }
}