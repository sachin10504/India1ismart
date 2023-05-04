using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPM
{
    public static class Converter
    {
        public static string byteArrayToHexString(byte[] ba)
        {
            try
            {
                return BitConverter.ToString(ba).Replace("-", "");
            }
            catch (Exception xObj)
            {
                return "";
            }
        }

        public static string hexStringToBinaryString(string hexstring)
        {
            try
            {
                return String.Join(String.Empty,
                  hexstring.Select(
                    c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                  )
                );
            }
            catch (Exception xObj)
            {
                return "";
            }
        }

        public static string binaryStringToHexString(string binary)
        {
            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);

            int mod4Len = binary.Length % 8;
            if (mod4Len != 0)
            {
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }

            for (int i = 0; i < binary.Length; i += 8)
            {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }

            return result.ToString();
        }

        public static byte[] hexStringToByteArray(String hex)
        {
            int NumberChars = hex.Length / 2;
            byte[] bytes = new byte[NumberChars];
            using (var sr = new System.IO.StringReader(hex))
            {
                for (int i = 0; i < NumberChars; i++)
                    bytes[i] =
                      Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
            }
            return bytes;
        }

        public static string byteArrayToBinaryString(byte[] ba)
        {
            if (ba == null) return "";
            return hexStringToBinaryString(byteArrayToHexString(ba));
        }

        public static byte[] binaryStringToByteArray(String binary)
        {
            if (binary == null) return null;
            if (binary.Equals("")) return null;

            return hexStringToByteArray(binaryStringToHexString(binary));
        }

        public static Byte[] a2eANDe2a(Byte[] input, Boolean a2e)
        {
            int[] ascii2ebcdic = new int[256]{
                0, 1, 2, 3, 55, 45, 46, 47, 22, 5, 37, 11, 12, 13, 14, 15,
                16, 17, 18, 19, 60, 61, 50, 38, 24, 25, 63, 39, 28, 29, 30, 31,
                64, 79,127,123, 91,108, 80,125, 77, 93, 92, 78,107, 96, 75, 97,
                240,241,242,243,244,245,246,247,248,249,122, 94, 76,126,110,111,
                124,193,194,195,196,197,198,199,200,201,209,210,211,212,213,214,
                215,216,217,226,227,228,229,230,231,232,233, 74,224, 90, 95,109,
                121,129,130,131,132,133,134,135,136,137,145,146,147,148,149,150,
                151,152,153,162,163,164,165,166,167,168,169,192,106,208,161, 7,
                32, 33, 34, 35, 36, 21, 6, 23, 40, 41, 42, 43, 44, 9, 10, 27,
                48, 49, 26, 51, 52, 53, 54, 8, 56, 57, 58, 59, 4, 20, 62,225,
                65, 66, 67, 68, 69, 70, 71, 72, 73, 81, 82, 83, 84, 85, 86, 87,
                88, 89, 98, 99,100,101,102,103,104,105,112,113,114,115,116,117,
                118,119,120,128,138,139,140,141,142,143,144,154,155,156,157,158,
                159,160,170,171,172,173,174,175,176,177,178,179,180,181,182,183,
                184,185,186,187,188,189,190,191,202,203,204,205,206,207,218,219,
                220,221,222,223,234,235,236,237,238,239,250,251,252,253,254,255
                };

            Byte[] output = new Byte[input.Length];

            if (a2e)
            {
                for (Int32 i = 0; i < input.Length; i++)
                {
                    output[i] = (byte)ascii2ebcdic[Convert.ToInt32(input[i])];
                }
            }
            else
            {
                for (Int32 i = 0; i < input.Length; i++)
                {
                    for (Int32 j = 0; j < ascii2ebcdic.Length; j++)
                    {
                        if (ascii2ebcdic[j].Equals(Convert.ToInt32(input[i])))
                        {
                            output[i] = (byte)j;
                            break;
                        }
                    }
                }
            }

            return output;
        }

        public static void pack(ref byte[] message, byte[] data)
        {
            int position = message.Length;

            Array.Resize(ref message, position + data.Length);
            Array.Copy(data, 0, message, position, data.Length);
        }
    }

    public class Record
    {
        public string messageHeader = "";
        public string FunctionCode = "";
        public object[] data = new object[128];
        private string bitmap = "";

        public Boolean isFieldPresent(int field)
        {
            try
            {
                return bitmap.Substring(field - 1, 1).Equals("1");
            }
            catch (Exception xObj)
            {
                return false;
            }
        }

        private void constructBitmap()
        {
            bitmap = Converter.byteArrayToBinaryString((byte[])data[0]) + Converter.byteArrayToBinaryString((byte[])data[1]);
        }

        private void setFieldIndicator(int field)
        {
            setBitmap(field, '1');
        }

        private void removeFieldIndicator(int field)
        {
            setBitmap(field, '0');
        }

        private void setBitmap(int field, char value)
        {
            int position = (field < 65) ? 0 : 1;

            char[] bitmap = null;

            if (data[position] == null) data[position] = new byte[16];
            bitmap = Converter.byteArrayToBinaryString((byte[])data[position]).ToCharArray();

            
            if (field==72)
            {
                bitmap[7] = value;
            }
            else
                if (field == 95)
                {
                    bitmap[30] = value;
                }
                else
            {
                bitmap[field - 1] = value;
            }

            data[position] = Converter.binaryStringToByteArray(new String(bitmap));

            constructBitmap();
        }

        public void removeField(Int32 field)
        {
            data[field] = null;
            removeFieldIndicator(field);
        }

        public Boolean setField(Int32 field, Object value)
        {
            try
            {
                data[field] = value;

                if (field < 2) constructBitmap();

                if (!isFieldPresent(field)) setFieldIndicator(field);

                return true;
            }
            catch (Exception xObj)
            {
                return false;
            }
        }

        public object getField(Int32 field)
        {
            if (isFieldPresent(field))
                return data[field];
            else
                return null;
        }
    }

    public class IPMRecord
    {
        public List<Record> records;
        public Boolean success = false;
        public Boolean incoming = false;

        //private Record record;
        //private String[] recordFormat;
        private Int32 lastSearchedIndex = -1;        

        public IPMRecord()
        {
            //recordFormat = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\ipmrecord.cfg");
        }

        public IPMRecord(Byte[] message)
        {
            //recordFormat = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\ipmrecord.cfg");

            success = parseMessage(message);
        }

        private byte[] strip124(byte[] data)
        {
            int len = 0;
            byte[] clearData = new byte[data.Length];
            for (int i =0; i < data.Length; i++)
            {
                if (data[i] != 124)
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

        public Boolean parseMessage(Byte[] data)
        {
            Int32 position = 0, length = 0, length2 = 0;
            Record record;
            Byte[] message;
            if (incoming)
            {
                message = strip124(data);
            }
            else
            {
               message = data;
            }
                
            try
            {
                do
                {
                    record = new Record();

                    if (!incoming)
                        record.messageHeader = ASCIIEncoding.ASCII.GetString(Converter.a2eANDe2a(message.Skip(position).Take(4).ToArray(), false));
                    else
                        record.messageHeader = ASCIIEncoding.ASCII.GetString(Converter.a2eANDe2a(message.Skip(position + 4).Take(4).ToArray(), false));

                    position += 4 + ((incoming) ? 4 : 0);

                    if (!incoming)
                        record.setField(0, message.Skip(position).Take(8).ToArray());
                    else
                        record.setField(0, Converter.a2eANDe2a(message.Skip(position).Take(8).ToArray(), false));
                    position += 8;

                    for (Int32 index = 1; index < record.data.Length; index++)
                    {
                        length2 = 0;

                        if (index == 1 || index == 9 || index == 10 || index == 41 || index == 71) length2 = 8;
                        if (index == 3 || index == 38 || index == 73 ) length2 = 6;
                        if ((index >= 4 && index <= 6) || index == 12 || index == 22 || index == 37) length2 = 12;
                        if (index == 14 || index == 25 || index == 26) length2 = 4;
                        if (index == 23 || index == 24 || index == 40 || (index >= 49 && index <= 51)) length2 = 3;
                        if (index == 30) length2 = 24;
                        if (index == 42) length2 = 15;

                      //  if (index == 37) length2 = 7;

                        if (length2 > 0)
                        {
                            if (record.isFieldPresent(index))
                            {
                               

                                if (index.Equals(1) && incoming)
                                    record.setField(index, Converter.a2eANDe2a(message.Skip(position).Take(length2).ToArray(), false));
                                else
                                    record.setField(index, message.Skip(position).Take(length2).ToArray());

                                if (index.Equals(24))
                                {
                                    record.FunctionCode = ASCIIEncoding.ASCII.GetString(Converter.a2eANDe2a(message.Skip(position).Take(length2).ToArray(), false));
                                }
                                position += length2;
                            }
                        }
                        else
                        {
                            if (index == 2 || (index >= 31 && index <= 33) || index == 43 || index == 54 || index == 55 || index == 62 || index == 63 || index == 72 || (index >= 93 && index <= 95) || index == 100 || index == 111 || (index >= 123 && index <= 125) || index == 127)
                            {
                                length2 = 2;
                                if (index == 54 || index == 55 || index == 62 || index == 63 || index == 72 || index == 111 || (index >= 123 && index <= 125) || index == 127) length2 = 3;

                                if (record.isFieldPresent(index))
                                {
                                    try
                                    {
                                        length = Convert.ToInt32(ASCIIEncoding.ASCII.GetString(Converter.a2eANDe2a(message.Skip(position).Take(length2).ToArray(), false)));
                                    }
                                    catch (Exception a)
                                    { 
                                    
                                    }
                                    record.setField(index, message.Skip(position + length2).Take(length).ToArray());
                                    position += length + length2;
                                }
                            }
                            else if (index == 48)
                            {
                                if (record.isFieldPresent(48))
                                {
                                    try
                                    {
                                        length = Convert.ToInt32(ASCIIEncoding.ASCII.GetString(Converter.a2eANDe2a(message.Skip(position).Take(3).ToArray(), false)));
                                    }
                                    catch (Exception b)
                                    { 
                                    
                                    }
                                    List<Record> pdsRecords = null;
                                    Byte[] field48 = message.Skip(position + 3).Take(length).ToArray();
                                    for (int index2 = 0; index2 < length; )
                                    {
                                        Record pdsRecord = new Record();
                                        pdsRecord.messageHeader = ASCIIEncoding.ASCII.GetString(Converter.a2eANDe2a(field48.Skip(index2).Take(4).ToArray(), false));
                                        index2 += 4;
                                        length2 = Convert.ToInt32(ASCIIEncoding.ASCII.GetString(Converter.a2eANDe2a(field48.Skip(index2).Take(3).ToArray(), false)));
                                        index2 += 3;
                                        pdsRecord.setField(0, field48.Skip(index2).Take(length2).ToArray());
                                        index2 += length2;

                                        if (pdsRecords == null) pdsRecords = new List<Record>();
                                        pdsRecords.Add(pdsRecord);
                                    }

                                    if (pdsRecords != null) record.setField(48, pdsRecords);
                                    position += length + 3;
                                }
                            }
                        }
                    }

                    //if (records == null) records = new List<Record>();
                    //records.Add(record);
                    appendRecord(record);
                    if (record.messageHeader == "1644" && record.FunctionCode == "695")
                    {
                        break;
                    }
                } while (position < message.Length);

                return true;
            }
            catch (Exception xObj)
            {
                return false;
            }
        }

        private void pack(ref byte[] message, byte[] data)
        {
            int position = message.Length;

            Array.Resize(ref message, position + data.Length);
            Array.Copy(data, 0, message, position, data.Length);
        }

        public Byte[] buildMessage()
        {
            Byte[] message = new Byte[0];

            success = false;

            try
            {
                foreach (Record record in records.ToArray())
                {
                    pack(ref message, Converter.a2eANDe2a(ASCIIEncoding.ASCII.GetBytes(record.messageHeader), true));

                    pack(ref message, (byte[])record.data[0]);
                    
                    if (record.isFieldPresent(1)) pack(ref message, (byte[])record.data[1]);
                    
                    for (int index = 2; index < record.data.Length; index++)
                    {
                        if ((index >= 3 && index <= 30) || (index >= 34 && index <= 42) || (index >= 49 && index <= 51) || index == 71 || index == 73) if (record.isFieldPresent(index)) pack(ref message, (byte[])record.data[index]);

                        if (index == 2 || (index >= 31 && index <= 33) || index == 43 || (index >= 93 && index <= 95) || index == 100)
                        {
                            if (record.isFieldPresent(index))
                            {
                                pack(ref message, Converter.a2eANDe2a(ASCIIEncoding.ASCII.GetBytes(Convert.ToString(((byte[])record.data[index]).Length).PadLeft(2, '0')), true));
                                pack(ref message, (byte[])record.data[index]);
                            }
                        }

                        if (index == 48)
                        {
                            if (record.isFieldPresent(index))
                            {
                                Byte[] field48 = new Byte[0];
                                foreach (Record pdsRecord in (List<Record>)record.data[index])
                                {
                                    pack(ref field48, Converter.a2eANDe2a(ASCIIEncoding.ASCII.GetBytes(pdsRecord.messageHeader), true));
                                    pack(ref field48, Converter.a2eANDe2a(ASCIIEncoding.ASCII.GetBytes(Convert.ToString(((byte[])pdsRecord.data[0]).Length).PadLeft(3, '0')), true));
                                    pack(ref field48, (byte[])pdsRecord.data[0]);
                                }

                                if (field48.Length != 0)
                                {
                                    pack(ref message, Converter.a2eANDe2a(ASCIIEncoding.ASCII.GetBytes(Convert.ToString(field48.Length).PadLeft(3, '0')), true));
                                    pack(ref message, field48);
                                }
                            }
                        }

                        if (index == 54 || index == 55 || index == 62 || index == 63 || index == 72 || index == 73 || index == 111 || (index >= 123 && index <= 127))
                        {
                            if (record.isFieldPresent(index))
                            {
                                pack(ref message, Converter.a2eANDe2a(ASCIIEncoding.ASCII.GetBytes(Convert.ToString(((byte[])record.data[index]).Length).PadLeft(3, '0')), true));
                                pack(ref message, (byte[])record.data[index]);
                            }
                        }
                    }
                }

                success = true;
                return message;
            }
            catch (Exception xObj)
            {
                return null;
            }
        }

        public Int32 searchByField(Int32 field, string value)
        {
            Int32 index = 0;

            try
            {
                foreach (Record record in records)
                {
                    if (lastSearchedIndex < index)
                    {
                        if (record.isFieldPresent(field))
                        {
                            if (ASCIIEncoding.ASCII.GetString(Converter.a2eANDe2a((byte[])record.data[field], false)).Equals(value))
                            {
                                lastSearchedIndex = index;
                                return index;
                            }
                        }
                    }

                    index++;
                }
            }
            catch (Exception xObj)
            {

            }

            resetSearch();
            return -1;
        }

        public Record getRecordByIndex(Int32 index)
        {
            Int32 currentIndex = 0;

            try
            {
                foreach (Record record in records)
                {
                    if (currentIndex.Equals(index))
                        return record;
                    else
                        currentIndex++;
                }
            }
            catch (Exception xObj)
            {
            }

            return null;
        }

        public void resetSearch()
        {
            lastSearchedIndex = -1;
        }

        public void appendRecord()
        {
            appendRecord(new Record());
        }

        public void appendRecord(Record record)
        {
            if (records == null) records = new List<Record>();
            records.Add(record);
        }

        public void appendRecord(Record record, Int32 index)
        {
            if (records == null) records = new List<Record>();
            if (records.Count == 0)
                appendRecord(record);
            else
                if (index > 0) records.Insert(index, record);
        }

        public void removeRecord(Int32 index)
        {
            if (records != null)
            {
                if (index.Equals(-1))
                {
                    records.RemoveRange(0, records.Count);
                }
                else
                {
                    if (records.Count > index)
                    {
                        records.RemoveAt(index);
                    }
                }
            }
        }
    }
}
