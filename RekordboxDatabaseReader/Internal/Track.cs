using System.Runtime.InteropServices;

namespace RekordboxDatabaseReader.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct Track
    {
        internal const short TableId = 0x24;
        public readonly uint Unknown1;
        public readonly uint SampleRate;
        public readonly uint ComposerId;
        public readonly uint FileSize;
        public readonly uint TrackId;
        public readonly uint Unknown2;
        public readonly uint Unknown3;
        public readonly uint Unknown4;
        public readonly uint OriginalArtistId;
        public readonly uint Unknown5;
        public readonly uint RemixerId;
        public readonly uint Bitrate;
        public readonly uint TrackNumber;
        public readonly uint Unknown6;
        public readonly uint Unknown7;
        public readonly uint AlbumId;
        public readonly uint ArtistId;
        public readonly uint Unknown8;
        public readonly ushort DiscNumber;
        public readonly ushort PlayCount;
        public readonly uint Unknown9;
        public readonly ushort DurationInSeconds;
        public readonly int Unknown10;
        public readonly int Unknown11;
        public readonly StringPointer UnknownString1;
        public readonly StringPointer Lyricist;
        public readonly StringPointer UnknownString2;
        public readonly StringPointer UnknownString3;
        public readonly StringPointer UnknownString4;
        public readonly StringPointer KUVO;
        public readonly StringPointer Public;
        public readonly StringPointer AutoloadHotCue;
        public readonly StringPointer UnknownString5;
        public readonly StringPointer UnknownString6;
        public readonly StringPointer Date;
        public readonly StringPointer UnknownString7;
        public readonly StringPointer MixName;
        public readonly StringPointer UnknownString8;
        public readonly StringPointer DatFile;
        public readonly StringPointer Date2;
        public readonly StringPointer Comment;
        public readonly StringPointer TrackName;
        public readonly StringPointer UnknownString9;
        public readonly StringPointer FileName;
        public readonly StringPointer FilePath;
    }
}
