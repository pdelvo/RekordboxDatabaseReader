using System;
using System.Collections.Generic;
using System.Text;
using IOPath = System.IO.Path;

namespace RekordboxDatabaseReader
{
    public sealed class Artist
    {
        public uint ArtistId { get; }
        public string Name { get; }

        public Artist(uint artistId, string name)
        {
            ArtistId = artistId;
            Name = name;
        }

        internal static Artist FromRow(RekordboxLibrary library, Internal.RowHeader row)
        {
            var internalArtist = row.ParseColumnData<Internal.Artist>();

            return new Artist(internalArtist.ArtistId, row.ReadString(internalArtist.ArtistName).ReadAsUtf8());
        }
    }
}
