using System;
using System.Text;
using static RekordboxDatabaseReader.ParserHelper;

namespace RekordboxDatabaseReader
{
    public ref struct UnknownHeaderDescriptor
    {
        public int HeadSize;
        public int TagSize;
        public ReadOnlySpan<byte> HeaderName;
        public ReadOnlySpan<byte> Header;
        public ReadOnlySpan<byte> Tag;

        public static UnknownHeaderDescriptor Parse(ref ReadOnlySpan<byte> buffer)
        {
            UnknownHeaderDescriptor descriptor = default;

            descriptor.HeaderName = buffer.Slice(0, 4);

            ReadOnlySpan<byte> tempSpan = buffer.Slice(4);
            descriptor.HeadSize = ParseInt(ref tempSpan);
            descriptor.TagSize = ParseInt(ref tempSpan);

            descriptor.Header = buffer.Slice(0, descriptor.HeadSize);
            descriptor.Tag = buffer.Slice(descriptor.HeadSize, descriptor.TagSize - descriptor.HeadSize);

            buffer = buffer.Slice(descriptor.TagSize);

            return descriptor;
        }

        public string GetHeaderName()
        {
            return Encoding.ASCII.GetString(HeaderName);
        }
    }
}
