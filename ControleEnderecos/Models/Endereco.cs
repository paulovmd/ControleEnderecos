namespace ControleEnderecos.Models
{
    public class Endereco
    {
        public int Id { get; set; }

        public int IdCliente { get; set; } 

        public string TipoEndereco { get; set; } = string.Empty;

        public string Rua { get; set; } = string.Empty;

        public string Bairro { get; set; } = string.Empty;

        public string Cep { get; set; } = string.Empty;

        public int IdCidade { get; set; }

        public string NomeCidade { get; set; } = string.Empty;

        public int IdEstado { get; set; }

        public string NomeEstado { get; set; } = string.Empty;

        public string Uf { get; set; } = string.Empty;

    }
}
