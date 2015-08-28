using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApplication2
{
    class Encrypter
    {
        public static void Decode(byte[] src, byte[] key)
        {
            Encode(src, key);
        }
        public static void Encode(byte[] src, byte[] key)
        {
            for (int i = 0; i < src.Length; i++)
            {
                src[i] = (byte)(src[i] ^ key[i % key.Length]);
            }
        }
    }
    class Program
    {
        private static void EncryptFile(string target_name, string dest_name)
        {
            DecryptFile(dest_name, target_name);
        }
        private static void DecryptFile(string target_name, string dest_name)
        {
            Console.WriteLine(String.Format("Xor File:{0}", target_name));
            FileStream fs = new FileStream(target_name, FileMode.Open);
            long size = fs.Length;
            byte[] src = new byte[size];
            fs.Read(src, 0, src.Length);
            fs.Close();
            byte[] dst = decrypt(src);
            FileStream ds = new FileStream(dest_name, FileMode.Create);
            ds.Write(dst,0,dst.Length);
            ds.Close();

        }
        private static byte[] decrypt(byte[] src)
        {
            //init xor key
            int[] numArray = new int[] { -1387743643, 0x132f087a, -380916995, 0x4271fbc6,
                0x481e49b7, 0x839d7f1, -893766998, -1242421477,
                -1230126924, 0x255d7a9e, 0x64659018 };//for net.kairosoft.android.airplane_ja
            byte[] key = new byte[numArray.Length * 4];
            for (int i = 0; i < key.Length; i++)
            {
                key[i] = (byte)((numArray[i / 4] >> ((i % 4) * 8)) & 0xff);
            }
            Encrypter.Decode(src, key);
            return src;
        }
        static void Main(string[] args)
        {
            string base_name = System.AppDomain.CurrentDomain.BaseDirectory;
            String[] files = Directory.GetFiles(String.Format("{0}\\data", base_name));
            foreach (String file in files)
            {
                string ext_name = Path.GetExtension(file);
                if (ext_name == ".dat")
                {
                    string target_name = String.Format("{0}\\data\\{1}", base_name,Path.GetFileName(file));
                    string dest_name = String.Format("{0}\\data\\{1}", base_name, Path.GetFileName(file));
                    DecryptFile(target_name, dest_name);
                }
            }

        }
    }
}
