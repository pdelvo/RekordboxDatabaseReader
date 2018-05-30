using RekordboxDatabaseReader;
using System;
using System.Buffers;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string datPath = @"C:\Users\Dennis\Desktop\ANLZ0000.DAT";
            var datBuffer = GetBufferFromPath(datPath);
            var extBuffer = GetBufferFromPath(Path.ChangeExtension(datPath, "ext"));

            RekordboxFileParser.ParseFile(ref datBuffer,
                filePathDescriptorAction: p =>
                {
                    Console.WriteLine(p.GetPathString());
                },
                quantizedTimeZonesDescriptorAction: q =>
                {
                    foreach (var item in q.QuantizedPoints)
                    {
                        Console.WriteLine(item.BpmValue / 100f);
                    }
                },
                cueObjectDescriptorAction: q=>
                {
                    foreach (var item in q.DataPoints)
                    {
                        Console.WriteLine($"{item.HotCueNumber} Cue point at {item.StartTimeMs / 1000f}s");
                    }
                });
        }

        private static ReadOnlySpan<byte> GetBufferFromPath(string path)
        {
            MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(path, FileMode.Open);
            var mma = mmf.CreateViewAccessor(0, new FileInfo(path).Length).SafeMemoryMappedViewHandle;
            ReadOnlySpan<byte> buffer;
            unsafe
            {
                byte* ptrMemMap = (byte*)0;
                mma.AcquirePointer(ref ptrMemMap);
                buffer = new Span<byte>(ptrMemMap, (int)mma.ByteLength);
            }
            return buffer;
        }
    }
}
