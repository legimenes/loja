using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Loja.Core.Identity.Stores;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace Loja.Core.Identity.Managers
{
    public class RsaManager : IRsaManager
    {
        private readonly IRsaStore _rsaStore;

        public RsaManager(IRsaStore rsaStore)
        {
            _rsaStore = rsaStore;
        }

        public async Task<RSAParameters> GetRsaPrivateKeyParameters()
        {
            string rsaPrivateKey = await GetRsaPrivateKey();
            RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParameters = LoadRsaPrivateCrtKeyParameters(rsaPrivateKey);

            RSAParameters rsaParameters = new RSAParameters
            {
                Modulus = rsaPrivateCrtKeyParameters.Modulus.ToByteArrayUnsigned(),
                Exponent = rsaPrivateCrtKeyParameters.PublicExponent.ToByteArrayUnsigned(),
                D = rsaPrivateCrtKeyParameters.Exponent.ToByteArrayUnsigned(),
                P = rsaPrivateCrtKeyParameters.P.ToByteArrayUnsigned(),
                Q = rsaPrivateCrtKeyParameters.Q.ToByteArrayUnsigned(),
                DP = rsaPrivateCrtKeyParameters.DP.ToByteArrayUnsigned(),
                DQ = rsaPrivateCrtKeyParameters.DQ.ToByteArrayUnsigned(),
                InverseQ = rsaPrivateCrtKeyParameters.QInv.ToByteArrayUnsigned(),
            };

            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(2048);
            rsaProvider.ImportParameters(rsaParameters);
            RSAParameters rsaPrivateKeyParameters = rsaProvider.ExportParameters(true);

            return rsaPrivateKeyParameters;
        }
        public async Task<RSAParameters> GetRsaPublicKeyParameters()
        {
            string rsaPublicKey = await GetRsaPublicKey();
            RsaKeyParameters rsaKeyParameters = LoadRsaKeyParameters(rsaPublicKey);

            RSAParameters rsaParameters = new RSAParameters
            {
                Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned(),
                Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned()
            };

            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(2048);
            rsaProvider.ImportParameters(rsaParameters);
            RSAParameters rsaPrivateKeyParameters = rsaProvider.ExportParameters(false);

            return rsaPrivateKeyParameters;
        }

        private async Task<string> GetRsaPrivateKey()
        {
            string rsaPrivateKey = await _rsaStore.GetPrivateKey();
            rsaPrivateKey = rsaPrivateKey.Replace("\\n", Environment.NewLine);

            if (string.IsNullOrWhiteSpace(rsaPrivateKey))
                throw new InvalidOperationException("Private key not found");
            
            return rsaPrivateKey;
        }
        private async Task<string> GetRsaPublicKey()
        {
            string rsaPublicKey = await _rsaStore.GetPublicKey();
            rsaPublicKey = rsaPublicKey.Replace("\\n", Environment.NewLine);

            if (string.IsNullOrWhiteSpace(rsaPublicKey))
                throw new InvalidOperationException("Public key not found");
            
            return rsaPublicKey;
        }
        private RsaPrivateCrtKeyParameters LoadRsaPrivateCrtKeyParameters(string rsaPrivateKey)
        {
            RsaPrivateCrtKeyParameters rsaPrivateKeyParams;
            using (StringReader reader = new StringReader(rsaPrivateKey))
            {
                PemReader pemReader = new PemReader(reader);
                rsaPrivateKeyParams = (RsaPrivateCrtKeyParameters)pemReader.ReadObject();
            }

            if (rsaPrivateKeyParams == null)
                throw new InvalidOperationException("Invalid RSA private key parameters");

            return rsaPrivateKeyParams;
        }
        private RsaKeyParameters LoadRsaKeyParameters(string rsaPublicKey)
        {
            RsaKeyParameters rsaKeyParams;
            using (StringReader reader = new StringReader(rsaPublicKey))
            {
                PemReader pemReader = new PemReader(reader);
                rsaKeyParams = (RsaKeyParameters)pemReader.ReadObject();
            }

            if (rsaKeyParams == null)
                throw new InvalidOperationException("Invalid RSA public key parameters");

            return rsaKeyParams;
        }
    }
}