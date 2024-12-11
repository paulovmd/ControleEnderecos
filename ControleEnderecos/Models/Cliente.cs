using Org.BouncyCastle.Pkcs;

namespace ControleEnderecos.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Telefone {  get; set; } = string.Empty;

        public string Rua { get; set; } = string.Empty;
        
        public string Bairro {  get; set; } = string.Empty;

        public string Numero {  get; set; } = string.Empty; 

        public string Cep { get; set; } = string.Empty;

        public int IdCidade { get; set; }

        public string NomeCidade { get; set; } = string.Empty;  

        public int IdEstado { get; set; }

        public string NomeEstado {  get; set; } = string.Empty;

        public string Uf { get; set; } = string.Empty;  
        
        
    }
}
