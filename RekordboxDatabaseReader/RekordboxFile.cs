using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace RekordboxDatabaseReader
{
    public abstract class RekordboxFile : IDisposable
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
