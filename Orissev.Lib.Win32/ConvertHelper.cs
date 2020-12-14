using Orissev.Win32.Enums;
using System;

namespace Orissev.Win32
{
    public static class ConvertHelper
    {
        public static WS ToWS(this IntPtr self) => (WS)unchecked((uint)self.ToInt32());

        public static WS_EX ToWS_EX(this IntPtr self) => (WS_EX)unchecked((uint)self.ToInt32());

        public static long ToLong<T>(this T self) where T : struct, IConvertible => Convert.ToInt64(self);

        public static string ToEnumName<T>(this T self) where T : struct, IConvertible => Enum.GetName(typeof(T), self);

        public static string ToEnumTypeName<T>(this T self) where T : struct, IConvertible => typeof(T).Name;

        public static string ToRepr<T>(this T self) where T : struct, IConvertible => string.Format("({2}:{0}/0x{0:x}) [{1}]", self.ToLong(), self.ToEnumName(), self.ToEnumTypeName());

        public static string ToRepr(this IntPtr self) => string.Format("[0x{0:x16}]", self.ToInt64());

        public static uint ToUIntMilliseconds(this TimeSpan timeSpan) => (uint)unchecked(timeSpan.TotalMilliseconds.ToLong());

        public static int ToInt32(this IntPtr intPtr) => unchecked((int)intPtr.ToInt64());

        public static string ToRepr(this bool self) => self ? "true" : "false";
    }
}