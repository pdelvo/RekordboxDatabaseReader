using System.Runtime.InteropServices;

namespace RekordboxDatabaseReader.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct Artist
    {
        public readonly uint ArtistId;
        public readonly byte Unknown1; // Was 3
        public readonly StringPointerShort ArtistName;
    }
}
