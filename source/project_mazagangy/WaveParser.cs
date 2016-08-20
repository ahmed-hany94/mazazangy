using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace project_mazagangy
{
    class WaveParser
    {
        private byte[] byteDataArray1;
        public byte[] ByteDataArray1 { get { return byteDataArray1; } }
        private byte[,] byteDataArray;
        private int[] intDataArray1;
        private int[,] intDataArray;
        private short[] shortDataArray1;
        private short[,] shortDataArray;
        private int length;
        private int sampleRate;
        public int SampleRate { get { return sampleRate; } }
        private int DataLength;


        private string ChunkId;
        private int ChunkSize;
        private string Format;

        private string subChunkId;
        private int subChunkSize;
        private short AudioFormat;
        private short channels;
        public short Channels { get { return channels; } }

        private int ByteRate;
        private short BlockAlign;
        private short BitsPerSample;
        public short GETBitsPerSample { get { return BitsPerSample; } }

        private string subChunk2ID;
        private int subChunk2Size;







        public void WaveHeaderIN(string spath)
        {
            FileStream fs = new FileStream(spath, FileMode.Open, FileAccess.Read);

            BinaryReader br = new BinaryReader(fs);




            fs.Position = 0;
            ChunkId = string.Concat(br.ReadChars(4));
            fs.Position = 4;
            ChunkSize = br.ReadInt32();
            fs.Position = 8;
            Format = string.Concat(br.ReadChars(4));

            fs.Position = 12;

            subChunkId = string.Concat(br.ReadChars(4));
            fs.Position = 16;
            subChunkSize = br.ReadInt32();
            fs.Position = 20;
            AudioFormat = br.ReadInt16();
            fs.Position = 22;
            channels = br.ReadInt16();
            fs.Position = 24;
            sampleRate = br.ReadInt32();
            fs.Position = 28;
            ByteRate = br.ReadInt32();
            fs.Position = 32;
            BlockAlign = br.ReadInt16();
            fs.Position = 34;
            BitsPerSample = br.ReadInt16();


            fs.Position = 36;
            subChunk2ID = String.Concat(br.ReadChars(4));

            fs.Position = 40;
            subChunk2Size = br.ReadInt32();

            DataLength = (int)((fs.Length - 44));



            switch (BitsPerSample)
            {
                case 8:
                    int k = 0;
                    byteDataArray = new byte[DataLength, channels];
                    byteDataArray1 = new byte[DataLength * channels * 2];
                    for (int i = 0; i < DataLength; i++)
                    {
                        for (int j = 0; j < channels; j++)
                        {
                            if (fs.Position >= DataLength)
                            {
                                break;
                            }
                            byteDataArray[i, j] = br.ReadByte();
                            fs.Position += 1;

                        }

                    }
                    k = 0;

                    for (int i = 0; i < byteDataArray.GetLength(0); i++)
                    {
                        for (int j = 0; j < byteDataArray.GetLength(1); j++)
                        {
                            byteDataArray1[k] = byteDataArray[i, j];
                            k += 2;

                        }
                    }

                    break;
                case 16:
                    shortDataArray = new short[DataLength, channels];
                    byteDataArray1 = new byte[DataLength * channels * 2];
                    shortDataArray1 = new short[DataLength * channels * 2];

                    for (int i = 0; i < DataLength; i++)
                    {
                        for (int j = 0; j < channels; j++)
                        {

                            if (fs.Position >= DataLength)
                            { break; }
                            shortDataArray[i, j] = br.ReadInt16();
                            fs.Position += 2;

                        }

                    }


                    k = 0;
                    for (int i = 0; i < shortDataArray.GetLength(0); i++)
                    {
                        for (int j = 0; j < shortDataArray.GetLength(1); j++)
                        {
                            shortDataArray1[k] = shortDataArray[i, j];
                            k += 2;

                        }
                    }


                    Buffer.BlockCopy(shortDataArray1, 0, byteDataArray1, 0, byteDataArray1.Length);
                    break;
                case 32:
                    intDataArray = new int[DataLength, channels];
                    intDataArray1 = new int[DataLength * channels * 2];
                    byteDataArray1 = new byte[DataLength * channels * 2];
                    for (int i = 0; i < DataLength; i++)
                    {
                        for (int j = 0; j < channels; j++)
                        {
                            if (fs.Position >= DataLength)
                            {
                                break;
                            }
                            intDataArray[i, j] = br.ReadInt32();
                            fs.Position += 4;

                        }

                    }

                    k = 0;

                    for (int i = 0; i < intDataArray.GetLength(0); i++)
                    {
                        for (int j = 0; j < intDataArray.GetLength(1); j++)
                        {
                            intDataArray1[k] = intDataArray[i, j];
                            k += 2;

                        }
                    }

                    Buffer.BlockCopy(intDataArray1, 0, byteDataArray1, 0, byteDataArray1.Length);
                    byteDataArray1 = Array.ConvertAll<int, byte>(intDataArray1, delegate (int item) { return (byte)item; });

                    break;
            }


            br.Close();
            fs.Close();
        }


    }
}
