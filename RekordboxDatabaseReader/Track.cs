using System;
using IOPath = System.IO.Path;

namespace RekordboxDatabaseReader
{
    public class Track : RekordboxFile
    {
        public string Name { get; }

        public Track(string name, string datPath)
            : base (datPath)
        {
            Name = name;

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
                IOPath.Combine(IOPath.GetPathRoot(library.Path), row.ReadString(internalTrack.DatFile).Slice(1).ReadAsUtf8()));
        }
    }
}
