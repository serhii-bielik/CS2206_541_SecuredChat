using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SecuredChat
{
    class RSACrypter
    {
        string publicKey;
        string thirdPartyPublicKey;
        string privateKey;
        RSACryptoServiceProvider cryptoProvider;

        public string PublicKey
        {
            get
            {
                return publicKey;
            }
        }

        public string ThirdPartyPublicKey
        {
            get
            {
                return thirdPartyPublicKey;
            }

            set
            {
                thirdPartyPublicKey = value;
            }
        }

        public RSACrypter()
        {
            cryptoProvider = new RSACryptoServiceProvider(2048);
            publicKey = cryptoProvider.ToXmlString(false);
            privateKey = cryptoProvider.ToXmlString(true);
        }

        public byte[] encryptMessage(string message)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            rsa.FromXmlString(ThirdPartyPublicKey);
            byte[] encripted_mes = rsa.Encrypt(Encoding.Unicode.GetBytes(message), false);

            return encripted_mes;
        }

        public string decryptMessage(byte[] message)
        {
            try
            {
                //byte[] encripted_mes = Encoding.Unicode.GetBytes(message);
                RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider();
                rsa2.FromXmlString(privateKey);
                byte[] decripted_mes = rsa2.Decrypt(message, false);
                return Encoding.Unicode.GetString(decripted_mes);
            } catch (Exception ex)
            {                
                return "Exception while decrypting " + ex.Message;
            }
        }
    }
}
