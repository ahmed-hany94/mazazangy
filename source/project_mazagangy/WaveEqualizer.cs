using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Dsp;

namespace project_mazagangy
{
    class WaveEqualizer : ISampleProvider
    {
        private readonly ISampleProvider Source;
        private readonly EqualizerBand[] bands;
        private readonly BiQuadFilter[,] filters;
        private readonly int channels;
        private readonly int bandCount;
        private bool updated;

        public WaveEqualizer(ISampleProvider s, EqualizerBand[] bands)
        {
            this.Source = s;
            this.bands = bands;
            this.channels = Source.WaveFormat.Channels;
            this.bandCount = bands.Length;
            this.filters = new BiQuadFilter[channels, bands.Length];
            CreateFilters();
        }


        WaveFormat ISampleProvider.WaveFormat{ get{return Source.WaveFormat;}}

        private void CreateFilters()
        {
            for (int bandIndex = 0; bandIndex < bandCount; bandIndex++)
            {
                var band = bands[bandIndex];
                for (int n = 0; n < channels; n++)
                {
                    if (filters[n, bandIndex] == null)
                        filters[n, bandIndex] = BiQuadFilter.PeakingEQ(Source.WaveFormat.SampleRate, band.Frequency, band.Bandwidth, band.Gain);
                    else
                        filters[n, bandIndex].SetPeakingEq(Source.WaveFormat.SampleRate, band.Frequency, band.Bandwidth, band.Gain);
                }
            }
        }

        public void Update()
        {
            updated = true;
            CreateFilters();
        }

        public int Read(float[] buffer, int offset, int count)
        {

            int read = Source.Read(buffer, offset, count);

            if (updated)
            {
                CreateFilters();
                updated = false;
            }


            for (int i = 0; i < read; i++)
            {
                int ch = i % channels;

                for (int band = 0; band < bandCount; band++)
                {
                    buffer[offset + i] = filters[ch, band].Transform(buffer[offset + i]);
                }
            }


            return read;
        }


        }
}
