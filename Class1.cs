using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace Enigma
{
    class Encryptor
    {

        public static void Encrypt() 
        {
            try
            {
                string original = "here s some data to encrypt";
                string filename = "C:/Users/ilja/Documents/Visual Studio 2015/Projects/Enigma/ToEncrypt.txt";
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
                using (SymmetricAlgorithm symmetricAlgorithm = Rijndael.Create())
                //using (SymmetricAlgorithm myAes = Aes.Create())
                //using (Aes myAes = Aes.Create())
                {

                    // Encrypt the string to an array of bytes.
                    byte[] encrypted = EncryptStringToBytes(filename, symmetricAlgorithm.Key, symmetricAlgorithm.IV, symmetricAlgorithm);

                    // Decrypt the bytes to a string.
                    string roundtrip = DecryptStringFromBytes(encrypted, symmetricAlgorithm.Key, symmetricAlgorithm.IV, symmetricAlgorithm);

                    //Display the original data and the decrypted data.
                    Console.WriteLine("Original:   {0}", original);
                    Console.WriteLine("Round Trip: {0}", roundtrip);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
        static byte[] EncryptStringToBytes(string plainText, byte[] Key,byte[] IV, SymmetricAlgorithm symmetricAlgorithm ) 
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            //using (Aes aesAlg = Aes.Create())

                symmetricAlgorithm.Key = Key;
                symmetricAlgorithm.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = symmetricAlgorithm.CreateEncryptor(symmetricAlgorithm.Key, symmetricAlgorithm.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            using (FileStream fs = File.OpenRead(plainText))
                            {
                                byte[] b = new byte[256];
                                UTF8Encoding temp = new UTF8Encoding(true);
                                int got;

                                while ((got = fs.Read(b, 0, b.Length)) > 0)
                                {
                                    swEncrypt.Write(temp.GetString(b, 0, got));
                                    //Console.WriteLine(temp.GetString(b, 0, got));

                                }
                            }
                            //swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV, SymmetricAlgorithm symmetricAlgorithm)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            //using (Aes aesAlg = Aes.Create())

                symmetricAlgorithm.Key = Key;
                symmetricAlgorithm.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = symmetricAlgorithm.CreateDecryptor(symmetricAlgorithm.Key, symmetricAlgorithm.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                                                        // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }



            return plaintext;

        }
    }
}