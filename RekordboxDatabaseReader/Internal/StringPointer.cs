using System.Runtime.InteropServices;

namespace RekordboxDatabaseReader.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct StringPointer : IStringPointer
    {
        internal readonly ushort Location;

        public int RelativeLocation => Location;
    }
}
