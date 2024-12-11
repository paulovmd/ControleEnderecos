using System.ComponentModel.DataAnnotations;

namespace ControleEnderecos.ViewModel
{
    public class CidadeViewModel
    {
        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public string Uf { get; set; } = string.Empty;
        public int Id { get; internal set; }
    }
}
