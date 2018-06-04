using System;
using IOPath = System.IO.Path;

namespace RekordboxDatabaseReader
{
    public sealed class Track : RekordboxFile
    {
        public string Name { get; }
        public uint ArtistId { get; }

        public Track(string name, uint artistId, string datPath)
            : base (datPath)
        {
            Name = name;
            ArtistId = artistId;

            ReadOnlySpan<byte> span = Memory.Span;

            Internal.RekordboxFileParser.ParseFile(ref span, filePathDescriptorAction: p =>
            {
                Console.WriteLine(p.GetPathString());
            });
        }

        internal static Track FromRow(RekordboxLibrary library, Internal.RowHeader row)
        {
            var internalTrack = row.ParseColumnData<Internal.Track>();

            return new Track(row.ReadString(internalTrack.TrackName).ReadAsUtf8(),
                internalTrack.ArtistId,
                IOPath.Combine(IOPath.GetDirectoryName(library.Path), "../../",
                row.ReadString(internalTrack.DatFile).Slice(1).ReadAsUtf8()));
        }
    }
}
