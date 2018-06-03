using System.Runtime.InteropServices;

namespace RekordboxDatabaseReader.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColoredWaveDataPoint
    {
        private readonly byte data;

        private const byte colorMap = 0b11100000;
        private const byte heightMap = 0b00011111;

        public byte ColorIndex => (byte)((data & colorMap) >> 5);
        public byte Height => (byte)(data & heightMap);
    }
}
