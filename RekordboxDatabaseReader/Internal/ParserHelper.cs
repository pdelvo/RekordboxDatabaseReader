﻿using System;
using System.Buffers.Binary;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace RekordboxDatabaseReader.Internal
{
    internal static class ParserHelper
    {
        internal static T ParsePrimitive<T>(ref ReadOnlySpan<byte> buffer)
            where T : unmanaged
        {
            var i = Unsafe.ReadUnaligned<T>(ref Unsafe.AsRef(buffer[0]));
            buffer = buffer.Slice(Unsafe.SizeOf<T>());

            return i;
        }

        internal static T ParsePrimitive<T>(ref ReadOnlyMemory<byte> buffer)
            where T : unmanaged
        {
            var i = Unsafe.ReadUnaligned<T>(ref Unsafe.AsRef(buffer.Span[0]));
            buffer = buffer.Slice(Unsafe.SizeOf<T>());

            return i;
        }

        internal static T ParsePrimitive<T>(ReadOnlySpan<byte> buffer)
            where T : unmanaged
        {
            var i = Unsafe.ReadUnaligned<T>(ref Unsafe.AsRef(buffer[0]));
            return i;
        }

        internal static T ParsePrimitive<T>(ReadOnlyMemory<byte> buffer)
            where T : unmanaged
        {
            var i = Unsafe.ReadUnaligned<T>(ref Unsafe.AsRef(buffer.Span[0]));
            return i;
        }

        internal static int ParseInt(ref ReadOnlySpan<byte> buffer)
        {
            int i = BinaryPrimitives.ReadInt32BigEndian(buffer);
            buffer = buffer.Slice(4);

            return i;
        }

        internal static uint ParseUInt(ref ReadOnlySpan<byte> buffer)
        {
            int i = BinaryPrimitives.ReadInt32BigEndian(buffer);
            buffer = buffer.Slice(4);

            return (uint)i;
        }

        internal static short ParseShort(ref ReadOnlySpan<byte> buffer)
        {
            short i = BinaryPrimitives.ReadInt16BigEndian(buffer);
            buffer = buffer.Slice(2);

            return i;
        }

        internal static bool ParseBoolean(ref ReadOnlySpan<byte> buffer)
        {
            return ParseInt(ref buffer) != 0;
        }

        internal static int ToHostOrder(int networkOrder)
        {
            return IPAddress.NetworkToHostOrder(networkOrder);
        }

        internal static short ToHostOrder(short networkOrder)
        {
            return IPAddress.NetworkToHostOrder(networkOrder);
        }

        internal static ushort ToHostOrder(ushort networkOrder)
        {
            return (ushort)IPAddress.NetworkToHostOrder((short)networkOrder);
        }

        internal static void ParseHeader(ref ReadOnlySpan<byte> buffer, ReadOnlySpan<byte> header)
        {
            if (!buffer.StartsWith(header))
            {
                throw new ParseException($"header '{Encoding.ASCII.GetString(header)}' expected but '{Encoding.ASCII.GetString(buffer.Slice(0, 4))}' found.");
            }

            buffer = buffer.Slice(header.Length);
        }

        internal static ReadOnlySpan<byte> ParseString(ref ReadOnlySpan<byte> buffer)
        {
            int size = ParseInt(ref buffer);

            var result = buffer.Slice(0, size);
            buffer = buffer.Slice(size);

            return result;
        }

        internal static byte[] GetHeaderBytes(string header) => Encoding.ASCII.GetBytes(header);

        internal static ReadOnlySpan<T> ParseWaveDataPoints<T>(ref ReadOnlySpan<byte> buffer, out int unknown2)
            where T: struct
        {
            int tableSize = ParseInt(ref buffer);

            unknown2 = ParseInt(ref buffer);

            var table = buffer.Slice(0, tableSize);
            var result = MemoryMarshal.Cast<byte, T>(table);
            buffer = buffer.Slice(tableSize);

            return result;
        }
    }
}
