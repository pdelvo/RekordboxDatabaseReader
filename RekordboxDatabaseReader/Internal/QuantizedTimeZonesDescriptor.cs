using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static RekordboxDatabaseReader.Internal.ParserHelper;

namespace RekordboxDatabaseReader.Internal
{
    public ref struct QuantizedTimeZonesDescriptor
    {
        public int HeadSize;
        public int PacketSize;
        public int Unknown1;
        public int Unknown2;
        public ReadOnlySpan<QuantizedPoint> QuantizedPoints;
        public static readonly ReadOnlyMemory<byte> Tag = GetHeaderBytes("PQTZ");

        public static QuantizedTimeZonesDescriptor Parse(ref ReadOnlySpan<byte> buffer)
        {
            QuantizedTimeZonesDescriptor descriptor = default;

            ParseHeader(ref buffer, Tag.Span);

            descriptor.HeadSize = ParseInt(ref buffer);
            descriptor.PacketSize = ParseInt(ref buffer);
            descriptor.Unknown1 = ParseInt(ref buffer);
            descriptor.Unknown2 = ParseInt(ref buffer);
            descriptor.QuantizedPoints = ParseQuantizedList(ref buffer);

            return descriptor;
        }

        private static ReadOnlySpan<QuantizedPoint> ParseQuantizedList(ref ReadOnlySpan<byte> buffer)
        {
            int tableSize = ParseInt(ref buffer) * Marshal.SizeOf<QuantizedPoint>();

            var table = buffer.Slice(0, tableSize);
            var result = MemoryMarshal.Cast<byte, QuantizedPoint>(table);
            buffer = buffer.Slice(tableSize);

            return result;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct QuantizedPoint
    {
        private readonly short beatPhase;
        private readonly ushort bpmValue;
        private readonly int timeIndexInMs;

        public short BeatPhase => ToHostOrder(beatPhase);
        public ushort BpmValue => ToHostOrder(bpmValue);
        public int TimeIndexInMs => ToHostOrder(timeIndexInMs);
    }
}
