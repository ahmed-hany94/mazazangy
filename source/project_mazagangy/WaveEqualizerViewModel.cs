using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_mazagangy
{
    class WaveEqualizerViewModel
    {
        private AudioFileReader reader;
        private IWavePlayer player;
        private WaveEqualizer EQ;
        public WaveEqualizer GETEQ { get { return EQ; } }
        private string FileName;
        private readonly EqualizerBand[] bands;

        public EqualizerBand[] GetBands { get { return bands; } }

        public WaveEqualizerViewModel()
        {

            bands = new EqualizerBand[]
            {
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 50, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 100, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 200, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 300, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 500, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 1000, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 2000, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 3000, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 5000, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 10000, Gain = 0},
                new EqualizerBand {Bandwidth = 0.8f, Frequency = 20000, Gain = 0},
            };
        }

        public float Band1
        {
            get { return bands[0].Gain; }
            set
            {
                if (bands[0].Gain != value)
                {
                    bands[0].Gain = value;
                }
            }
        }

        public float Band2
        {
            get { return bands[1].Gain; }
            set
            {
                if (bands[1].Gain != value)
                {
                    bands[1].Gain = value;
                }
            }
        }

        public float Band3
        {
            get { return bands[2].Gain; }
            set
            {
                if (bands[2].Gain != value)
                {
                    bands[2].Gain = value;
                }
            }
        }

        public float Band4
        {
            get { return bands[3].Gain; }
            set
            {
                if (bands[3].Gain != value)
                {
                    bands[3].Gain = value;
                }
            }
        }

        public float Band5
        {
            get { return bands[4].Gain; }
            set
            {
                if (bands[4].Gain != value)
                {
                    bands[4].Gain = value;
                }
            }
        }

        public float Band6
        {
            get { return bands[5].Gain; }
            set
            {
                if (bands[5].Gain != value)
                {
                    bands[5].Gain = value;
                }
            }
        }


        public float Band7
        {
            get { return bands[6].Gain; }
            set
            {
                if (bands[6].Gain != value)
                {
                    bands[6].Gain = value;
                }
            }
        }

        public float Band8
        {
            get { return bands[7].Gain; }
            set
            {
                if (bands[7].Gain != value)
                {
                    bands[7].Gain = value;
                }
            }
        }
        public float Band9
        {
            get { return bands[8].Gain; }
            set
            {
                if (bands[8].Gain != value)
                {
                    bands[8].Gain = value;
                }
            }
        }
        public float Band10
        {
            get { return bands[9].Gain; }
            set
            {
                if (bands[9].Gain != value)
                {
                    bands[9].Gain = value;
                }
            }
        }
        public float Band11
        {
            get { return bands[10].Gain; }
            set
            {
                if (bands[10].Gain != value)
                {
                    bands[10].Gain = value;
                }
            }
        }

    }
}
