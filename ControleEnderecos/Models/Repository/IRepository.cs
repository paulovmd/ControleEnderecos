namespace ControleEnderecos.Models.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Método responsável por inserir uma nova entidade
        /// na base de dados
        /// </summary>
        /// <param name="entity"> Instância da entidade a ser 
        /// inserida na base dados.</param>
        /// <returns></returns>
        public Task<bool> Insert(TEntity entity);

        public Task<bool> Update(TEntity entity);

        public Task<bool> Delete(TEntity entity);

        public Task<TEntity> GetById(int id);

        public Task<List<TEntity>> GetAll();
    }
}
