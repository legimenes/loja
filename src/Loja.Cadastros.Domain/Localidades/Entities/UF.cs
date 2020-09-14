using Loja.Core.Domain.Objects;

namespace Loja.Cadastros.Domain.Localidades.Entities
{
    public class UF : Entity
    {
        public UF()
        {
        }

        public string Sigla { get; set; }
        public string Nome { get; set; }
        public string CodigoIBGE { get; set; }
    }
}