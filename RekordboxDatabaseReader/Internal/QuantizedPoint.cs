using System.Runtime.InteropServices;
using static RekordboxDatabaseReader.Internal.ParserHelper;

namespace RekordboxDatabaseReader.Internal
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct QuantizedPoint
    {
        private readonly short beatPhase;
        private readonly ushort bpmValue;
        private readonly int timeIndexInMs;

        public short BeatPhase => ToHostOrder(beatPhase);
        public ushort BpmValue => ToHostOrder(bpmValue);
        public int TimeIndexInMs => ToHostOrder(timeIndexInMs);
    }
}
