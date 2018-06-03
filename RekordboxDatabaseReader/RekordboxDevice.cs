using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace RekordboxDatabaseReader
{
    public partial class RekordboxDevice : IDisposable
    {
        public string Path { get; }
        public RekordboxLibrary Library { get; }

        public RekordboxDevice(string path)
        {
            Path = path;
            Library = new RekordboxLibrary(System.IO.Path.Combine(Path, RekordboxLibrary.DefaultRelativePath));
        }

        public void Dispose()
        {
            Library.Dispose();
        }
    }
}
