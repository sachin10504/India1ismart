using IPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;

namespace Reports
{
    public class ClsISOReader
 
    {
        private string Hex2Binary(string DE)
        {

            string myBinary = "";
            switch (DE.ToUpper())
            {
                case "0":
                    myBinary = "0000";
                    break;

                case "1":
                    myBinary = "0001";
                    break;

                case "2":
                    myBinary = "0010";
                    break;

                case "3":
                    myBinary = "0011";
                    break;

                case "4":
                    myBinary = "0100";
                    break;

                case "5":
                    myBinary = "0101";
                    break;

                case "6":
                    myBinary = "0110";
                    break;

                case "7":
                    myBinary = "0111";
                    break;

                case "8":
                    myBinary = "1000";
                    break;

                case "9":
                    myBinary = "1001";
                    break;

                case "A":
                    myBinary = "1010";
                    break;

                case "B":
                    myBinary = "1011";
                    break;

                case "C":
                    myBinary = "1100";
                    break;

                case "D":
                    myBinary = "1101";
                    break;

                case "E":
                    myBinary = "1110";
                    break;

                case "F":
                    myBinary = "1111";
                    break;


            }


            return myBinary;

        }

        private string DEtoBinary(string HexDE)
        {
            string deBinary = "";
            for (int I = 0; I <= 15; I++)
            {
                deBinary = deBinary + Hex2Binary(HexDE.Substring(I, 1));

            }

            return deBinary;

        }

        private string GetAsciiVal(byte[] data)
        {
            return ASCIIEncoding.ASCII.GetString(data);
        }

        private string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private string GetBinaryBitMap(string BitMap)
        {
            string BinaryBitMapPositions = "";

            for (int i = 0; i < BitMap.Length; i++)
            {
                BinaryBitMapPositions += Hex2Binary(BitMap.Substring(i, 1));
            }
                return BinaryBitMapPositions;
        }

        private bool IsVariableDE(int position)
        {
            if (DEVarLen[position] != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int[] DELen = new int[130];
        int[] DEVarLen = new int[130];

        public ClsISOReader()
        {
            //Initialize BitMap Var Length Indicator

             //Initialize BitMap Var Length Indicator
            DEVarLen[2] = 2; DEVarLen[31] = 2; DEVarLen[32] = 2; DEVarLen[33] = 2; DEVarLen[43] = 2;
             DEVarLen[48] = 3; DEVarLen[54] = 3;
            DEVarLen[55] = 3; DEVarLen[62] = 3; DEVarLen[63] = 3; DEVarLen[72] = 3; DEVarLen[95] = 2; 
            DEVarLen[100] = 2;DEVarLen[123] = 3; DEVarLen[124] = 3; DEVarLen[125] = 2;  DEVarLen[127] = 3;
            DEVarLen[93] = 2;
            DEVarLen[94] = 2;

            //DEVarLen[34] = 2; DEVarLen[35] = 2; DEVarLen[36] = 3;
            //DEVarLen[44] = 2; DEVarLen[45] = 2; DEVarLen[46] = 3; DEVarLen[47] = 3;
            //DEVarLen[56] = 3; DEVarLen[57] = 3; DEVarLen[58] = 3; DEVarLen[59] = 3;
            //DEVarLen[60] = 1; DEVarLen[61] = 3; DEVarLen[99] = 2;
            //DEVarLen[102] = 2; DEVarLen[103] = 2; DEVarLen[104] = 3; DEVarLen[105] = 3;
            //DEVarLen[106] = 3; DEVarLen[107] = 3; DEVarLen[108] = 3; DEVarLen[109] = 3; DEVarLen[110] = 3;
            //DEVarLen[111] = 3; DEVarLen[112] = 3; DEVarLen[113] = 2; DEVarLen[114] = 3; DEVarLen[115] = 3;
            //DEVarLen[116] = 3; DEVarLen[117] = 3; DEVarLen[118] = 3; DEVarLen[119] = 3; DEVarLen[120] = 3; DEVarLen[121] = 3;
            //DEVarLen[122] = 3; DEVarLen[126] = 1;


            DELen[2] = 2; DELen[32] = 2; DELen[33] = 2; DELen[34] = 2; DELen[35] = 2; DELen[36] = 3;
            DELen[44] = 2; DELen[45] = 2; DELen[46] = 3; DELen[47] = 3; DELen[48] = 3; DELen[54] = 3;
            DELen[55] = 3; DELen[56] = 3; DELen[57] = 3; DELen[58] = 3; DELen[59] = 3;
            DELen[60] = 1; DELen[61] = 3; DELen[62] = 3; DELen[63] = 3; DELen[72] = 3; DELen[95] = 2; DELen[99] = 2;
            DELen[100] = 2; DELen[102] = 2; DELen[103] = 2; DELen[104] = 3; DELen[105] = 3;
            DELen[106] = 3; DELen[107] = 3; DELen[108] = 3; DELen[109] = 3; DELen[110] = 3;
            DELen[111] = 8; DELen[112] = 3; DELen[113] = 2; DELen[114] = 3; DELen[115] = 3;
            DELen[116] = 3; DELen[117] = 3; DELen[118] = 3; DELen[119] = 3; DELen[120] = 3; DELen[121] = 3;
            DELen[122] = 3; DELen[123] = 3; DELen[124] = 3; DELen[125] = 2; DELen[126] = 1; DELen[127] = 3;
            DELen[93] = 2; DELen[43] = 2;
            DELen[94] = 2;

            // "-" means not numeric.

            DELen[0] = 16; DELen[1] = 16; DELen[3] = 6; DELen[4] = 12;
            DELen[5] = 12; DELen[6] = 12; DELen[7] = 10; DELen[8] = 8;
            DELen[9] = 8; DELen[10] = 8; DELen[11] = 6; DELen[12] = 12;
            DELen[13] = 4; DELen[14] = 4; DELen[15] = 4; DELen[16] = 4;
            DELen[17] = 4; DELen[18] = 4; DELen[19] = 3; DELen[20] = 3;
            DELen[21] = 3; DELen[22] = 12; DELen[23] = 3; DELen[24] = 3;
            DELen[25] = 4; DELen[26] = 4; DELen[27] = 1; DELen[28] = 8;
            DELen[29] = 8; DELen[30] = 24; DELen[31] = 2; DELen[37] = 12;
            DELen[38] = 6; DELen[39] = 2; DELen[40] = 3; DELen[41] = 8;
            DELen[42] = 15;  DELen[49] = 3; DELen[50] = 3;
            DELen[51] = 3; DELen[52] = 16; DELen[53] = 18; DELen[64] = 4;
            DELen[65] = 16; DELen[66] = 1; DELen[67] = 2; DELen[68] = 3;
            DELen[69] = 3; DELen[70] = 3; DELen[71] = 8; DELen[73] = 6;
            DELen[74] = 10; DELen[75] = 10; DELen[76] = 10; DELen[77] = 10;
            DELen[78] = 10; DELen[79] = 10; DELen[80] = 10; DELen[81] = 10;
            DELen[82] = 12; DELen[83] = 12; DELen[84] = 12; DELen[85] = 12;
            DELen[86] = 15; DELen[87] = 15; DELen[88] = 15; DELen[89] = 15;
            DELen[90] = 42; DELen[91] = 1; DELen[92] = 2;   DELen[96] = 8; DELen[97] = 16;
            DELen[98] = 25; DELen[101] = 17; DELen[128] = 16;

        }

        private byte[] strip64(byte[] data)
        {
            int len = 0;
            byte[] clearData = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != 64)
                {
                    clearData[len] = data[i];
                    len++;
                }
                else
                {

                }
            }
            return clearData;
        }


        public List<Record> FunParseData(byte[] data)
        {
            int position,length;
            List<Record> lstRecords = new List<Record>();

            //Converting to String and stripping first 4 bytes
            string RawDataString = System.Text.ASCIIEncoding.UTF8.GetString(data).Substring(4);
            
            byte[] RawData = data.Skip(4).ToArray();
            RawData = strip64(RawData);
            length = RawData.Length;
            position = 0;


            while (position < length)
            {
                Record rec = new Record();

                string MTI = GetAsciiVal(RawData.Skip(position).Take(4).ToArray());
                position += 4;
                rec.messageHeader = MTI;
                //Primary BitMap
                string PrimaryBitMap = ByteArrayToString(RawData.Skip(position).Take(8).ToArray());
                position += 8;

                //Secondary BitMap
                string SecondaryBitMap = ByteArrayToString(RawData.Skip(position).Take(8).ToArray());
                position += 8;

                //Binary BitMap
                string BinaryBitmap = GetBinaryBitMap(PrimaryBitMap + SecondaryBitMap);
                
                //Get All the data 
                for (int i = 0; i < BinaryBitmap.Length; i++)
                {
                   

                    if (BinaryBitmap.Substring(i, 1) == "1")
                    {
                        if (i == 0)
                        {
                            //secondary bitmap
                            rec.setField(i + 1, SecondaryBitMap);
                        }
                        else
                        {
                            int pickupCount = 0;
                            //string data = "";
                            if (IsVariableDE(i + 1))
                            {
                                try
                                {
                                    pickupCount = int.Parse(GetAsciiVal(RawData.Skip(position).Take(DELen[i + 1]).ToArray()));
                                }
                                catch (Exception ex)
                                { }
                                position += DELen[i + 1];
                                
                            }
                            else
                            {
                                pickupCount = DELen[i + 1];
                            }
                            rec.setField(i + 1, GetAsciiVal(RawData.Skip(position).Take(pickupCount).ToArray()));
                            if (i + 1 == 24)
                            {
                                rec.FunctionCode = GetAsciiVal(RawData.Skip(position).Take(pickupCount).ToArray());
                            }
                            position += pickupCount;
                        }
                        
                    }
                    
                }

                lstRecords.Add(rec);
                position += 4;

                if (rec.messageHeader == "1644" && rec.FunctionCode == "695")
                {
                    //END OF FOOTER
                    break;
                }
            }

            return lstRecords;
        }

    }
}
