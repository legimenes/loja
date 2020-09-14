using Loja.Core.Domain.Objects;

namespace Loja.Cadastros.Domain.Localidades.Entities
{
    public class Endereco : Entity
    {
        public Endereco()
        {
        }

        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string CEP { get; set; }
        public long IdBairro { get; set; }
        public Bairro Bairro { get; set; }
        public long IdCidade { get; set; }
        public Cidade Cidade { get; set; }
        public long IdUF { get; set; }
        public UF UF { get; set; }
    }
}