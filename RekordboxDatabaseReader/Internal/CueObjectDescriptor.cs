using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static RekordboxDatabaseReader.Internal.ParserHelper;

namespace RekordboxDatabaseReader.Internal
{
    public ref struct CueObjectDescriptor
    {
        public int HeadSize;
        public int PacketSize;
        public bool IsHotCue;
        public int NumberOfCuePoints;
        public int Memories;
        public ReadOnlySpan<CuePoint> DataPoints;
        public static readonly ReadOnlyMemory<byte> Tag = GetHeaderBytes("PCOB");

        public static CueObjectDescriptor Parse(ref ReadOnlySpan<byte> buffer)
        {
            CueObjectDescriptor descriptor = default;

            ParseHeader(ref buffer, Tag.Span);

            descriptor.HeadSize = ParseInt(ref buffer);
            descriptor.PacketSize = ParseInt(ref buffer);
            descriptor.IsHotCue = ParseBoolean(ref buffer);
            descriptor.NumberOfCuePoints = ParseInt(ref buffer);
            descriptor.Memories = ParseInt(ref buffer);
            descriptor.DataPoints = ParseDataPoints(ref buffer, descriptor.NumberOfCuePoints);

            return descriptor;
        }

        private static ReadOnlySpan<CuePoint> ParseDataPoints(ref ReadOnlySpan<byte> buffer, int numberOfCuePoints)
        {
            var table = buffer.Slice(0, numberOfCuePoints * Marshal.SizeOf<CuePoint>());
            var result = MemoryMarshal.Cast<byte, CuePoint>(table);
            buffer = buffer.Slice(table.Length);
            return result;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CuePoint
    {
        private readonly int header;
        private readonly int headSize;
        private readonly int tagSize;
        private readonly int hotCueNumber;
        private readonly int active;
        private readonly int unknown1;
        private readonly int unknown2;

        // Datas
        private readonly byte cueType;
        private readonly byte unknown3;
        private readonly short unknown4;
        private readonly int startTimeMs;
        private readonly int loopEndInMs;
        private readonly long unknown5;
        private readonly long unknown6;

        public int Header => header;

        public int HeadSize => ToHostOrder(headSize);

        public int TagSize => ToHostOrder(tagSize);

        public int HotCueNumber => ToHostOrder(hotCueNumber);

        public int Active => ToHostOrder(active);

        public int Unknown1 => unknown1;

        public int Unknown2 => unknown2;

        public byte CueType => cueType;

        public byte Unknown3 => unknown3;

        public short Unknown4 => unknown4;

        public int StartTimeMs => ToHostOrder(startTimeMs);

        public int LoopEndInMs => ToHostOrder(loopEndInMs);

        public long Unknown5 => unknown5;

        public long Unknown6 => unknown6;
    }
}
