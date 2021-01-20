using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Loja.Core.Identity.Managers
{
    public interface IRsaManager
    {
        Task<RSAParameters> GetRsaPrivateKeyParameters();
        Task<RSAParameters> GetRsaPublicKeyParameters();
    }
}