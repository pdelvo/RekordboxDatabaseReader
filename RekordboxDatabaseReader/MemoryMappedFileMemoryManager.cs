using Microsoft.Win32.SafeHandles;
using System;
using System.Buffers;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;

namespace RekordboxDatabaseReader
{
    internal class MemoryMappedFileMemoryManager : MemoryManager<byte>
    {
        private readonly MemoryMappedFile file;
        private SafeMemoryMappedViewHandle mma;

        public MemoryMappedFileMemoryManager(MemoryMappedFile file, long size)
        {
            this.file = file;
            this.mma = file.CreateViewAccessor(0, size).SafeMemoryMappedViewHandle;
        }

        public unsafe override Span<byte> GetSpan()
        {
            unsafe
            {
                byte* ptrMemMap = (byte*)0;
                mma.AcquirePointer(ref ptrMemMap);
                return new Span<byte>(ptrMemMap, (int)mma.ByteLength);
            }
        }

        public unsafe override MemoryHandle Pin(int elementIndex = 0)
        {
            return new MemoryHandle(Unsafe.AsPointer(ref GetSpan()[0]));
        }

        public override void Unpin()
        {
            // Nothing to do
        }

        protected override void Dispose(bool disposing)
        {
            mma.Dispose();
        }
    }
}
