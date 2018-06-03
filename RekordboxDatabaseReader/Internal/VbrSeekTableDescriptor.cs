using System;
using System.Runtime.InteropServices;
using static RekordboxDatabaseReader.Internal.ParserHelper;

namespace RekordboxDatabaseReader.Internal
{
    public readonly ref struct VbrSeekTableDescriptor
    {
        public readonly int HeadSize;
        public readonly int PacketSize;
        public readonly int Unknown;
        public readonly ReadOnlySpan<SeekTableEntry> NetworkOrderSeekTable;
        public readonly int Unknown2;

        public readonly static ReadOnlyMemory<byte> Tag = GetHeaderBytes("PVBR");

        public VbrSeekTableDescriptor(int headSize, int packetSize, int unknown, int unknown2, ReadOnlySpan<SeekTableEntry> networkOrderSeekTable)
        {
            HeadSize = headSize;
            PacketSize = packetSize;
            Unknown = unknown;
            Unknown2 = unknown2;
            NetworkOrderSeekTable = networkOrderSeekTable;
        }

        public static VbrSeekTableDescriptor Parse(ref ReadOnlySpan<byte> buffer)
        {
            ParseHeader(ref buffer, Tag.Span);

            var headSize = ParseInt(ref buffer);
            var packetSize = ParseInt(ref buffer);
            var unknown = ParseInt(ref buffer);
            var networkOrderSeekTable = ParseSeekTable(ref buffer);
            var unknown2 = ParseInt(ref buffer);

            return new VbrSeekTableDescriptor(headSize, packetSize, unknown, unknown2, networkOrderSeekTable);
        }

        private static ReadOnlySpan<SeekTableEntry> ParseSeekTable(ref ReadOnlySpan<byte> buffer)
        {
            const int seekTableSize = 400 * sizeof(int);

            var table = buffer.Slice(0, seekTableSize);
            var result = MemoryMarshal.Cast<byte, SeekTableEntry>(table);
            buffer = buffer.Slice(seekTableSize);

            return result;
        }
    }
}
