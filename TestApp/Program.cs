using Microsoft.Win32.SafeHandles;
using RekordboxDatabaseReader;
using System;
using System.Buffers;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var device = new RekordboxDevice(@"I:"))
            {
                var library = device.Library;

                foreach (var item in library.ListTracks())
                {
                    Console.WriteLine($"Name: {item.Name}, Path: {item.Path}");
                }
            }
        }
    }
}
