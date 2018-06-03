using System.Runtime.InteropServices;

namespace RekordboxDatabaseReader.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct StringPointerShort : IStringPointer
    {
        internal readonly byte Location;

        public int RelativeLocation => Location;
    }
}
