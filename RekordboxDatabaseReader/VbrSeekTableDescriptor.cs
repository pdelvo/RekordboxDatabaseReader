using System;
using System.Runtime.InteropServices;
using static RekordboxDatabaseReader.ParserHelper;

namespace RekordboxDatabaseReader
{
    public ref struct VbrSeekTableDescriptor
    {
        public int HeadSize;
        public int PacketSize;
        public int Unknown;
        public int Unknown2;
        public ReadOnlySpan<SeekTableEntry> NetworkOrderSeekTable;
        public static ReadOnlyMemory<byte> Tag = GetHeaderBytes("PVBR");

        public static VbrSeekTableDescriptor Parse(ref ReadOnlySpan<byte> buffer)
        {
            VbrSeekTableDescriptor descriptor = default;

            ParseHeader(ref buffer, Tag.Span);

            descriptor.HeadSize = ParseInt(ref buffer);
            descriptor.PacketSize = ParseInt(ref buffer);
            descriptor.Unknown = ParseInt(ref buffer);
            descriptor.NetworkOrderSeekTable = ParseSeekTable(ref buffer);
            descriptor.Unknown = ParseInt(ref buffer);

            return descriptor;
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

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct SeekTableEntry
    {
        private readonly int entry;

        public int Entry => ToHostOrder(entry);
    }
}
