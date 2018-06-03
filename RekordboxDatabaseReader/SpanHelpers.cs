using System;
using System.Collections.Generic;
using System.Text;

namespace RekordboxDatabaseReader
{
    internal static class SpanHelpers
    {
        public static string ReadAsUtf8(this ReadOnlySpan<byte> span)
        {
            return Encoding.UTF8.GetString(span);
        }
    }
}
