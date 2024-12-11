namespace ControleEnderecos.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public string Salt { get; set; } = string.Empty;   

        public string Password { get; set;} = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime Data { get; set; } = DateTime.Now;


    }
}
