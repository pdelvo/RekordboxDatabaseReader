using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RekordboxDatabaseReader
{
    public sealed class RekordboxLibrary : RekordboxFile
    {
        public const string DefaultRelativePath = @"PIONEER/rekordbox/export.pdb";

        ReadOnlyCollection<Internal.Block> blocks;

        ReadOnlyCollection<Track> tracks;
        ReadOnlyCollection<Artist> artists;

        public RekordboxLibrary(string path)
            : base(path)
        {
            ReadOnlyMemory<byte> memory = this.Memory;
            blocks = Internal.PdbParser.Parse(ref memory);

            LoadData();
        }

        private void LoadData()
        {
            var tracks = new List<Track>();
            var artists = new List<Artist>();

            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];

                for (int j = 0; j < block.Rows.Length; j++)
                {
                    var row = block.Rows.Span[j];
                    // Console.WriteLine("Row Table Id: " + row.TableId);

                    if (row.TableId == Internal.Track.TableId)
                    {
                        tracks.Add(Track.FromRow(this, row));
                    }

                    if (row.TableId == Internal.Artist.TableId)
                    {
                        artists.Add(Artist.FromRow(this, row));
                    }
                }
            }

            this.tracks = tracks.AsReadOnly();
            this.artists = artists.AsReadOnly();
        }

        public ReadOnlyCollection<Track> Tracks
        {
            get => tracks;
        }

        public ReadOnlyCollection<Artist> Artists
        {
            get => artists;
        }
    }
}
