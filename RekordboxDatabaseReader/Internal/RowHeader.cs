using System;
using System.Runtime.InteropServices;
using static RekordboxDatabaseReader.Internal.ParserHelper;

namespace RekordboxDatabaseReader.Internal
{
    public struct RowHeader
    {
        public short TableId;
        public short RowIndex;
        ReadOnlyMemory<byte> RowData;

        internal static RowHeader Parse(ReadOnlyMemory<byte> data)
        {
            var intRef = data;
            RowHeader row = default;
            row.TableId = ParsePrimitive<short>(ref intRef);
            row.RowIndex = ParsePrimitive<short>(ref intRef);
            row.RowData = intRef;

            return row;
        }

        public T ParseColumnData<T>()
            where T : struct
        {
            return MemoryMarshal.Read<T>(RowData.Span);
        }

        public ReadOnlySpan<byte> ReadString<T>(T pointer)
            where T : struct, IStringPointer
        {
            if (pointer.RelativeLocation == 0)
            {
                return default;
            }

            var location = RowData.Span.Slice(pointer.RelativeLocation - 4);

            int length = ParsePrimitive<byte>(ref location);
            length = (length - 1) / 2 - 1;
            return location.Slice(0, length);
        }
    }
}
