using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace RekordboxDatabaseReader
{
    public class RekordboxFile : IDisposable
    {
        public string Path { get; }

        MemoryMappedFile memoryMappedFile;
        MemoryMappedFileMemoryManager memoryManager;

        protected Memory<byte> Memory => memoryManager.Memory;

        protected RekordboxFile(string path)
        {
            Path = path;

            memoryMappedFile = MemoryMappedFile.CreateFromFile(System.IO.Path.GetFullPath(path), FileMode.Open);
            memoryManager = new MemoryMappedFileMemoryManager(memoryMappedFile, new FileInfo(path).Length);
        }

        public void Dispose()
        {
            memoryMappedFile.Dispose();
        }
    }
}
