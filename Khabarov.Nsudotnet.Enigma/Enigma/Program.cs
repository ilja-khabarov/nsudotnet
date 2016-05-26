using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    class Program
    {
        static void Main(string[] args)
        {
            //string[] argv = {"sam", "encrypt", "ToEncrypt.txt", "aes", "encrypted"};
            //string[] argve = {"sam", "decrypt", "encrypted", "aes", "key.txt", "out.txt"};
            Encryptor.Encrypt(args);
            System.Console.Read();
        }
    }
}
