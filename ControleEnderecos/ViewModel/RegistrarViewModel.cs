using System.ComponentModel.DataAnnotations;

namespace ControleEnderecos.ViewModel
{
    /// <summary>
    /// ViewModel que representa as informações
    /// para registrar um novo usuário.
    /// </summary>
    public class RegistrarViewModel
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Login { get; set; }    

        [Required]
        public string Telefone { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]        
        public string ConfirmarSenha { get; set; }

    }
}
