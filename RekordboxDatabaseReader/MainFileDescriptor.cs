using System;
using static RekordboxDatabaseReader.ParserHelper;

namespace RekordboxDatabaseReader
{
    public struct MainFileDescriptor
    {
        public int HeadSize;
        public int TotalFileSize;
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;

        public static ReadOnlyMemory<byte> Tag = GetHeaderBytes("PMAI");

        public static MainFileDescriptor Parse(ref ReadOnlySpan<byte> buffer)
        {
            MainFileDescriptor descriptor = default;

            ParseHeader(ref buffer, Tag.Span);

            descriptor.HeadSize = ParseInt(ref buffer);
            descriptor.TotalFileSize = ParseInt(ref buffer);
            descriptor.Unknown1 = ParseInt(ref buffer);
            descriptor.Unknown2 = ParseInt(ref buffer);
            descriptor.Unknown3 = ParseInt(ref buffer);
            descriptor.Unknown4 = ParseInt(ref buffer);

            return descriptor;
        }
    }
}
