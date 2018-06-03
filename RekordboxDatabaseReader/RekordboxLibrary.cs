using RekordboxDatabaseReader.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RekordboxDatabaseReader
{
    public class RekordboxLibrary : RekordboxFile
    {
        public const string DefaultRelativePath = @"PIONEER/rekordbox/export.pdb";

        ReadOnlyCollection<Internal.Block> blocks;

        public RekordboxLibrary(string path)
            : base(path)
        {
            ReadOnlyMemory<byte> memory = this.Memory;
            blocks = PdbParser.Parse(ref memory);
        }

        public IEnumerable<Track> ListTracks()
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];

                for (int j = 0; j < block.Rows.Length; j++)
                {
                    var row = block.Rows.Span[j];

                    if (row.TableId == Internal.Track.TableId)
                    {
                        yield return Track.FromRow(this, row);
                    }
                }
            }
        }
    }
}
