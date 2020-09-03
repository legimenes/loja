namespace Loja.Cadastros.Domain.Localidades.Entities
{
    public class Bairro
    {
        public Bairro()
        {
        }

        public string Nome { get; set; }
        public long IdCidade { get; set; }
        public Cidade Cidade { get; set; }
    }
}