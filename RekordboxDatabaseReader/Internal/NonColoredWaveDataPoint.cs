using System.Runtime.InteropServices;

namespace RekordboxDatabaseReader.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NonColoredWaveDataPoint
    {
        private readonly byte data;

        private const byte heightMap = 0b00001111;

        public byte Height => (byte)(data & heightMap);
    }
}
