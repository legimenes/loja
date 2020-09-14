using Loja.Core.Domain.Objects;

namespace Loja.Cadastros.Domain.Localidades.Entities
{
    public class Cidade : Entity
    {
        public Cidade()
        {
        }

        public string Nome { get; set; }
        public long IdUF { get; set; }
        public UF UF { get; set; }
        public string CodigoIBGE { get; set; }
    }
}