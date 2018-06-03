using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static RekordboxDatabaseReader.Internal.ParserHelper;

namespace RekordboxDatabaseReader.Internal
{
    public ref struct WaveDisplayDataDescriptor<T>
        where T: struct
    {
        public int HeadSize;
        public int TagSize;
        public int Unknown1;
        public int Unknown2;
        public ReadOnlySpan<T> DataPoints;
        public static ReadOnlyMemory<byte> Tag = GetHeaderBytes("PW");
        public short Kind;

        public static WaveDisplayDataDescriptor<T> Parse(ref ReadOnlySpan<byte> buffer)
        {
            WaveDisplayDataDescriptor<T> descriptor = default;

            ParseHeader(ref buffer, Tag.Span);
            descriptor.Kind = ParseShort(ref buffer);

            descriptor.HeadSize = ParseInt(ref buffer);
            descriptor.TagSize = ParseInt(ref buffer);
            if (descriptor.HeadSize == 24)
            {
                descriptor.Unknown1 = ParseInt(ref buffer);
            }
            descriptor.DataPoints = ParseWaveDataPoints<T>(ref buffer, out descriptor.Unknown2);

            return descriptor;
        }

        public static bool IsColored(ReadOnlySpan<byte> data)
        {
            return data[3] == '2';
        }
    }
}
