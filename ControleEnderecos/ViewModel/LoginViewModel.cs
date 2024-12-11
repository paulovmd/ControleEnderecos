using System.ComponentModel.DataAnnotations;

namespace ControleEnderecos.ViewModel
{
    /// <summary>
    /// A classe LoginViewModel é utilizada pelas
    /// classes controller para receber parâmetro
    /// de dados enviados pelo usuário.
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
