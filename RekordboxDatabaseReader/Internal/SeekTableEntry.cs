using System.Runtime.InteropServices;
using static RekordboxDatabaseReader.Internal.ParserHelper;

namespace RekordboxDatabaseReader.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct SeekTableEntry
    {
        private readonly int entry;

        public int Entry => ToHostOrder(entry);
    }
}
