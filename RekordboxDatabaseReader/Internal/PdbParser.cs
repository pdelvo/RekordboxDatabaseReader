using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RekordboxDatabaseReader.Internal
{
    public static class PdbParser
    {
        public static ReadOnlyCollection<Block> Parse(ref ReadOnlyMemory<byte> data)
        {
            List<Block> blocks = new List<Block>(data.Length / 4096);
            while (!data.IsEmpty)
            {
                var block = Block.Parse(ref data);
                blocks.Add(block);
            }

            return blocks.AsReadOnly();
        }
    }
}
