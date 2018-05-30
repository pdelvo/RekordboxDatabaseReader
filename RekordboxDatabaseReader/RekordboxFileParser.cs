using System;
using System.Collections.Generic;
using System.Text;

namespace RekordboxDatabaseReader
{
    public static class RekordboxFileParser
    {
        public static void ParseFile(ref ReadOnlySpan<byte> file,
            Action<MainFileDescriptor> mainFileDescriptorAction = null,
            FilePathDescriptorAction filePathDescriptorAction = null,
            CueObjectDescriptorAction cueObjectDescriptorAction = null,
            QuantizedTimeZonesDescriptorAction quantizedTimeZonesDescriptorAction = null,
            VbrSeekTableDescriptorAction vbrSeekTableDescriptorAction = null,
            WaveDisplayDataDescriptorAction<ColoredWaveDataPoint> coloredWaveDisplayDataDescriptorAction = null,
            WaveDisplayDataDescriptorAction<NonColoredWaveDataPoint> nonColoredWaveDisplayDataDescriptorAction = null,
            UnknownHeaderDescriptorAction unknownHeaderDescriptorAction = null)
        {
            while (!file.IsEmpty)
            {
                var type = PeekType(file);

                if (type.SequenceEqual(MainFileDescriptor.Tag.Span))
                {
                    MainFileDescriptor mainFileDescriptor = MainFileDescriptor.Parse(ref file);
                    mainFileDescriptorAction?.Invoke(mainFileDescriptor);
                }
                else if (type.SequenceEqual(FilePathDescriptor.Tag.Span))
                {
                    FilePathDescriptor filePathDescriptor = FilePathDescriptor.Parse(ref file);
                    filePathDescriptorAction?.Invoke(filePathDescriptor);
                }
                else if (type.SequenceEqual(CueObjectDescriptor.Tag.Span))
                {
                    CueObjectDescriptor cueObjectDescriptor = CueObjectDescriptor.Parse(ref file);
                    cueObjectDescriptorAction?.Invoke(cueObjectDescriptor);
                }
                else if (type.SequenceEqual(QuantizedTimeZonesDescriptor.Tag.Span))
                {
                    QuantizedTimeZonesDescriptor quantizedTimeZonesDescriptor = QuantizedTimeZonesDescriptor.Parse(ref file);
                    quantizedTimeZonesDescriptorAction?.Invoke(quantizedTimeZonesDescriptor);
                }
                else if (type.SequenceEqual(VbrSeekTableDescriptor.Tag.Span))
                {
                    VbrSeekTableDescriptor vbrSeekTableDescriptor = VbrSeekTableDescriptor.Parse(ref file);
                    vbrSeekTableDescriptorAction?.Invoke(vbrSeekTableDescriptor);
                }
                else if (type.StartsWith(WaveDisplayDataDescriptor<bool>.Tag.Span))
                {
                    if (WaveDisplayDataDescriptor<bool>.IsColored(type))
                    {
                        WaveDisplayDataDescriptor<ColoredWaveDataPoint> coloredWaveDisplayDataDescriptor = WaveDisplayDataDescriptor<ColoredWaveDataPoint>.Parse(ref file);
                        coloredWaveDisplayDataDescriptorAction?.Invoke(coloredWaveDisplayDataDescriptor);
                    }
                    else
                    {
                        WaveDisplayDataDescriptor<NonColoredWaveDataPoint> nonColoredWaveDisplayDataDescriptor = WaveDisplayDataDescriptor<NonColoredWaveDataPoint>.Parse(ref file);
                        nonColoredWaveDisplayDataDescriptorAction?.Invoke(nonColoredWaveDisplayDataDescriptor);
                    }
                }
                else
                {
                    UnknownHeaderDescriptor unknownHeaderDescriptor = UnknownHeaderDescriptor.Parse(ref file);
                    Console.WriteLine("Unknown header: " + unknownHeaderDescriptor.GetHeaderName());
                    unknownHeaderDescriptorAction?.Invoke(unknownHeaderDescriptor);
                }
            }
        }

        private static ReadOnlySpan<byte> PeekType(ReadOnlySpan<byte> file)
        {
            return file.Slice(0, 4);
        }

        public delegate void FilePathDescriptorAction(FilePathDescriptor data);
        public delegate void UnknownHeaderDescriptorAction(UnknownHeaderDescriptor data);
        public delegate void CueObjectDescriptorAction(CueObjectDescriptor data);
        public delegate void QuantizedTimeZonesDescriptorAction(QuantizedTimeZonesDescriptor data);
        public delegate void VbrSeekTableDescriptorAction(VbrSeekTableDescriptor data);
        public delegate void WaveDisplayDataDescriptorAction<T>(WaveDisplayDataDescriptor<T> data)
            where T: struct;
    }
}
