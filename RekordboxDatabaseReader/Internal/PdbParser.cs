using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using static RekordboxDatabaseReader.Internal.ParserHelper;

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

    public class Block
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

        public override string ToString()
        {
            return $"Block ID: {BlockId}, Number of rows: {NumberOfRows}, Table Id: {(Rows.IsEmpty ? -1 : Rows.Span[0].TableId)}";
        }
    }

    public struct RowHeader
    {
        public short TableId;
        public short RowIndex;
        ReadOnlyMemory<byte> RowData;

        internal static RowHeader Parse(ReadOnlyMemory<byte> data)
        {
            var intRef = data;
            RowHeader row = default;
            row.TableId = ParsePrimitive<short>(ref intRef);
            row.RowIndex = ParsePrimitive<short>(ref intRef);
            row.RowData = intRef;

            return row;
        }

        public T ParseColumnData<T>()
            where T : struct
        {
            return MemoryMarshal.Read<T>(RowData.Span);
        }

        public ReadOnlySpan<byte> ReadString(StringPointer pointer)
        {
            if (pointer.Location == 0)
            {
                return default;
            }

            var location = RowData.Span.Slice(pointer.Location - 4);

            int length = ParsePrimitive<byte>(ref location);
            length = (length - 1) / 2 - 1;
            return location.Slice(0, length);
        }

        public ReadOnlySpan<byte> ReadString(StringPointerShort pointer)
        {
            if (pointer.Location == 0)
            {
                return default;
            }

            var location = RowData.Span.Slice(pointer.Location - 4);

            int length = ParsePrimitive<byte>(ref location);
            length = (length - 1) / 2 - 1;
            return location.Slice(0, length);
        }
    }

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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct Artist
    {
        public readonly uint ArtistId;
        public readonly byte Unknown1; // Was 3
        public readonly StringPointerShort ArtistName;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct StringPointer
    {
        internal readonly ushort Location;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct StringPointerShort
    {
        internal readonly byte Location;
    }
}
