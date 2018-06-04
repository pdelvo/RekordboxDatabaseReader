using System;

namespace RekordboxDatabaseReader
{
    public sealed class RekordboxDevice : IDisposable
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
