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
        //static string original = "here s some data to encrypt";
        //static string filename = "C:/Users/ilja/Documents/Visual Studio 2015/Projects/Enigma/ToEncrypt.txt";
        //static string encryptedFile = "C:/Users/ilja/Documents/Visual Studio 2015/Projects/Enigma/encrypted";
        //private static string outfile = "C:/Users/ilja/Documents/Visual Studio 2015/Projects/Enigma/out.txt";
        static SymmetricAlgorithm _symmetricAlgorithm;
        public static void Encrypt(string[] args) 
        {
            try
            {

                switch (args[3])
                {
                    case "rc2":
                        _symmetricAlgorithm = RC2.Create();
                        break;
                    case "aes":
                        _symmetricAlgorithm = Aes.Create();
                        break;
                    case "des":
                        _symmetricAlgorithm = DES.Create();
                        break;
                    case "rij":
                        _symmetricAlgorithm = Rijndael.Create();
                        break;
                    default:
                        _symmetricAlgorithm = Aes.Create();
                        break;
                }
                if (args[0] == "encrypt")
                {
                    string filename = args[1];
                    string encryptedFile = args[3];
                    EncryptStringToBytes(filename, encryptedFile, _symmetricAlgorithm.Key, _symmetricAlgorithm.IV,
                        _symmetricAlgorithm);
                    string key = Convert.ToBase64String(_symmetricAlgorithm.Key);
                    string IV = Convert.ToBase64String(_symmetricAlgorithm.IV);
                    using (FileStream keyStream = File.OpenWrite("key.txt"))
                    {
                        using (StreamWriter writer = new StreamWriter(keyStream))
                        {
                            writer.WriteLine(key);
                            writer.WriteLine(IV);
                        }
                    }

                }
                if (args[0] == "decrypt")
                {
                    string encryptedFile = args[1];
                    string outfile = args[4];
                    using (FileStream keyStream = File.OpenRead(args[3]))
                    {
                        using (StreamReader keyReader = new StreamReader(keyStream))
                        {
                            _symmetricAlgorithm.Key = Convert.FromBase64String(keyReader.ReadLine());
                            _symmetricAlgorithm.IV = Convert.FromBase64String(keyReader.ReadLine());
                        }
                    }

                    DecryptStringFromBytes(encryptedFile, outfile, _symmetricAlgorithm.Key, _symmetricAlgorithm.IV,
                        _symmetricAlgorithm);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            finally
            {
                if ( _symmetricAlgorithm != null )
                _symmetricAlgorithm.Dispose();
            }
        }
        static void EncryptStringToBytes(string filename, string encryptedFile, byte[] Key,byte[] IV, SymmetricAlgorithm symmetricAlgorithm ) 
        {
            // Check arguments.
            if (filename == null || filename.Length <= 0)
                throw new ArgumentNullException("filename");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;

            using ( ICryptoTransform encryptor = symmetricAlgorithm.CreateEncryptor(symmetricAlgorithm.Key, symmetricAlgorithm.IV))
            {
                using (FileStream msEncrypt = File.Open(encryptedFile, FileMode.OpenOrCreate))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (FileStream fs = File.OpenRead(filename))
                        {
                            fs.CopyTo(csEncrypt);

                        }

                    }
                }
            }

            return ;

        }

        static void DecryptStringFromBytes(string encryptedFile, string outfile, byte[] Key, byte[] IV, SymmetricAlgorithm symmetricAlgorithm)
        {
            // Check arguments.
            if (encryptedFile == null || encryptedFile.Length <= 0)
                throw new ArgumentNullException("encryptedFile");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            symmetricAlgorithm.Key = Key;
            symmetricAlgorithm.IV = IV;
            File.Delete(outfile);


            using ( ICryptoTransform decryptor = symmetricAlgorithm.CreateDecryptor(symmetricAlgorithm.Key, symmetricAlgorithm.IV))
            {

                using (FileStream inputStream = File.Open(encryptedFile, FileMode.OpenOrCreate))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (FileStream outFileStream = File.Open(outfile, FileMode.OpenOrCreate))
                        {
                            csDecrypt.CopyTo(outFileStream);
                        }
                    }
                }
            }
            return ;    
        }
    }
}