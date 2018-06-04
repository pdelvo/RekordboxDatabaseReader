using System;
using static RekordboxDatabaseReader.Internal.ParserHelper;

namespace RekordboxDatabaseReader.Internal
{
    public sealed class Block
    {
        public uint Zero;
        public uint BlockId;
        public long Unknown1;
        public long Unknown2;
        public byte NumberOfRows;
        public byte NextRowId;
        public ushort Unknown3;
        public ushort RemainingBytes;
        public ushort DataSize;
        public long Unknown4;

        public int Unknown5;
        public ReadOnlyMemory<RowHeader> Rows;

        internal static Block Parse(ref ReadOnlyMemory<byte> data)
        {
            var intRef = data;
            Block block = new Block();
            block.Zero = ParsePrimitive<uint>(ref intRef);
            block.BlockId = ParsePrimitive<uint>(ref intRef);
            block.Unknown1 = ParsePrimitive<long>(ref intRef);
            block.Unknown2 = ParsePrimitive<long>(ref intRef);
            block.NumberOfRows = ParsePrimitive<byte>(ref intRef);
            block.NextRowId = ParsePrimitive<byte>(ref intRef);
            block.Unknown3 = ParsePrimitive<ushort>(ref intRef);
            block.RemainingBytes = ParsePrimitive<ushort>(ref intRef);
            block.DataSize = ParsePrimitive<ushort>(ref intRef);
            block.Unknown4 = ParsePrimitive<long>(ref intRef);
            var rowData = new RowHeader[block.NumberOfRows];

            for (int i = 0; i < block.NumberOfRows; i++)
            {
                rowData[i] = RowHeader.Parse(intRef.Slice(ParsePrimitive<ushort>(data.Slice(4096 - 6 - i * 2))));
            }

            data = data.Slice(4096);

            block.Rows = rowData;

            return block;
        }

        public override string ToString() =>
            $"Block ID: {BlockId}, Number of rows: {NumberOfRows}, Table Id: {(Rows.IsEmpty ? -1 : Rows.Span[0].TableId)}";
    }
}
