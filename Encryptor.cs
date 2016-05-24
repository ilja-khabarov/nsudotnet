using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace Enigma
{
    class Encryptor
    {
        static string original = "here s some data to encrypt";
        static string filename = "C:/Users/ilja/Documents/Visual Studio 2015/Projects/Enigma/ToEncrypt.txt";
        static string encryptedFile = "C:/Users/ilja/Documents/Visual Studio 2015/Projects/Enigma/encrypted";
        private static string outfile = "C:/Users/ilja/Documents/Visual Studio 2015/Projects/Enigma/out.txt";
        public static void Encrypt() 
        {
            try
            {

                //byte[] buf = new byte[128];
                /*
                FileStream inputStream = File.OpenRead(filename);
                while (inputStream.Read(buf, 0, 128) > 0)
                    Console.Write(buf.ToString());
                    */

                /*
            using (FileStream fs = File.OpenRead(filename))
            {
                byte[] b = new byte[256];
                UTF8Encoding temp = new UTF8Encoding(true);
                int got;

                while ( (got = fs.Read(b, 0, b.Length)) > 0)
                {

                    Console.WriteLine(temp.GetString(b,0,got));

                }
            }
            */

                // Create a new instance of the Aes
                // class.  This generates a new key and initialization 
                // vector (IV).
                using (SymmetricAlgorithm symmetricAlgorithm = DES.Create())
                //using (SymmetricAlgorithm myAes = Aes.Create())
                //using (Aes myAes = Aes.Create())
                {

                    // Encrypt the string to an array of bytes.
                    EncryptStringToBytes(filename, symmetricAlgorithm.Key, symmetricAlgorithm.IV, symmetricAlgorithm);

                    // Decrypt the bytes to a string.
                    DecryptStringFromBytes(encryptedFile, symmetricAlgorithm.Key, symmetricAlgorithm.IV, symmetricAlgorithm);

                    //Display the original data and the decrypted data.
                    Console.WriteLine("Original:   {0}", original);
                    //Console.WriteLine("Round Trip: {0}", roundtrip);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
        static void EncryptStringToBytes(string filjename, byte[] Key,byte[] IV, SymmetricAlgorithm symmetricAlgorithm ) 
        {
            // Check arguments.
            if (filename == null || filename.Length <= 0)
                throw new ArgumentNullException("filename");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            //using (Aes aesAlg = Aes.Create())

                //symmetricAlgorithm.Key = Key;
                //symmetricAlgorithm.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = symmetricAlgorithm.CreateEncryptor(symmetricAlgorithm.Key, symmetricAlgorithm.IV);

                // Create the streams used for encryption.
                //using (MemoryStream msEncrypt = new MemoryStream())
                using (FileStream msEncrypt = File.Open(encryptedFile, FileMode.OpenOrCreate))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            using (FileStream fs = File.OpenRead(filename))
                            {
                                fs.CopyTo(csEncrypt);
                            /*
                                byte[] b = new byte[256];
                                UTF8Encoding temp = new UTF8Encoding(true);
                                int got;

                                while ((got = fs.Read(b, 0, b.Length)) > 0)
                                {
                                    swEncrypt.Write(temp.GetString(b, 0, got));
                                    //Console.WriteLine(temp.GetString(b, 0, got));

                                }
                                */
                            }
                            //swEncrypt.Write(filename);
                        }
                        //encrypted = msEncrypt.ToArray();
                    /*
                        using (FileStream encryptedDataDFileStream = File.OpenWrite(encryptedFile))
                        {
                            encryptedDataDFileStream.Write(encrypted, 0, encrypted.Length);
                        }
                        */
                    }
                    msEncrypt.Close();
                }



            // Return the encrypted bytes from the memory stream.
            return ;

        }

        static void DecryptStringFromBytes(string filename, byte[] Key, byte[] IV, SymmetricAlgorithm symmetricAlgorithm)
        {
            // Check arguments.
            if (filename == null || filename.Length <= 0)
                throw new ArgumentNullException("filename");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");


            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;
            byte[] outBytes = null;
            int gotFromDecoder = 0;
            int offset = 0;

            // Create an Aes object
            // with the specified key and IV.
            //using (Aes aesAlg = Aes.Create())

            symmetricAlgorithm.Key = Key;
                symmetricAlgorithm.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = symmetricAlgorithm.CreateDecryptor(symmetricAlgorithm.Key, symmetricAlgorithm.IV);

            // Create the streams used for decryption.
            //using (MemoryStream msDecrypt = new MemoryStream(encryptedFile))
            File.Delete(outfile);

// there some black magic figures
            using (FileStream outFileStream = File.OpenWrite(outfile))
            {
                //using ( MemoryStream outFileStream = new MemoryStream())
                using (CryptoStream csDecrypt = new CryptoStream(outFileStream, decryptor, CryptoStreamMode.Read))
                {
                    
                    using (FileStream inputStream = File.OpenRead(encryptedFile))
                    {
                        //using (StreamWriter srDecrypt = new StreamWriter(csDecrypt))
                        {
                            inputStream.CopyTo(csDecrypt);
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            //plaintext = srDecrypt.ReadToEnd();

                        }
                        /*while ((gotFromDecoder = csDecrypt.Read(outBytes, 0, 128)) > 0)
                    {
                        outFileStream.Write(outBytes, offset, gotFromDecoder);
                        offset += gotFromDecoder;
                    }*/
                    }
                    Console.Write(outFileStream.ToString());
                }
                
            }


            return ;

        }
    }
}