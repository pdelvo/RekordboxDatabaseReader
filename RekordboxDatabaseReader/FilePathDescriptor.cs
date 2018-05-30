using System;
using System.Text;
using static RekordboxDatabaseReader.ParserHelper;

namespace RekordboxDatabaseReader
{
    public ref struct FilePathDescriptor
    {
        public int HeadSize;
        public int PacketSize;
        public ReadOnlySpan<byte> FilePath;
        private string? filePathString;
        public static readonly ReadOnlyMemory<byte> Tag = GetHeaderBytes("PPTH");

        public static FilePathDescriptor Parse(ref ReadOnlySpan<byte> buffer)
        {
            FilePathDescriptor descriptor = default;

            ParseHeader(ref buffer, Tag.Span);

            descriptor.HeadSize = ParseInt(ref buffer);
            descriptor.PacketSize = ParseInt(ref buffer);
            descriptor.FilePath = ParseString(ref buffer);

            return descriptor;
        }

        public string GetPathString()
        {
            // Strip trailing 00
            return filePathString ?? (filePathString = Encoding.BigEndianUnicode.GetString(FilePath.Slice(0, FilePath.Length - 2)))!;
        }
    }
}
