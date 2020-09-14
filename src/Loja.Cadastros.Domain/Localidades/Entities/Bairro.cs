using Loja.Core.Domain.Objects;

namespace Loja.Cadastros.Domain.Localidades.Entities
{
    public class Bairro : Entity
    {
        public Bairro()
        {
        }

        public string Nome { get; set; }
        public long IdCidade { get; set; }
        public Cidade Cidade { get; set; }
    }
}