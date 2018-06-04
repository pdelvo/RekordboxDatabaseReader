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
            Path = System.IO.Path.GetFullPath(path);

            long size = new FileInfo(Path).Length;
            memoryMappedFile = MemoryMappedFile.CreateFromFile(File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.Read), null, size, MemoryMappedFileAccess.Read, HandleInheritability.None, false);
            memoryManager = new MemoryMappedFileMemoryManager(memoryMappedFile, size);
        }

        public void Dispose()
        {
            memoryMappedFile.Dispose();
        }
    }
}
