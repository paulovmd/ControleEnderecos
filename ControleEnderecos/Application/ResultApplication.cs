using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ControleEnderecos.Application
{
    /// <summary>
    /// Classe utilizada nas WEBAPI como padrão
    /// de retorno para as requisições
    /// </summary>
    public class ResultApplication<TEntity> where TEntity : class
    {
        /// <summary>
        /// Define se a requisição obteve sucesso
        /// True para sim, caso contrário false
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// Mensagem retornada pela requisição.
        /// </summary>
        public string Message { get; set; } = string.Empty;
        /// <summary>
        /// Propriedade utiliza somente para exibir mensagens de erro
        /// retornas pelo try..catch
        /// </summary>
        public string Error { get; set; } = string.Empty;


        public List<TEntity> Data { get; set; } = [];

    }
}
