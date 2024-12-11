
namespace ControleEnderecos.Models.Repository
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> 
        where TEntity : class
    {
        private ControleEnderecoContext _context;

        public RepositoryBase(ControleEnderecoContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(TEntity entity)
        {
            try
            {   

                using (var dbTransaction = _context.Database.BeginTransaction())
                {
                    var dbSet = _context.Set<TEntity>();
                    
                    //A função FromResult transforma operação não Async para
                    //Async.
                    await Task.FromResult(_context.Remove(entity));

                    await _context.SaveChangesAsync();

                    await dbTransaction.CommitAsync();
                }

                return true;

            }catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception(ex.InnerException.Message);
                else
                    throw new Exception(ex.Message);
            }
        }

        public async Task<List<TEntity>> GetAll() 
        {
            return await Task.FromResult(_context.Set<TEntity>().ToList());
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<bool> Insert(TEntity entity)
        {
            try{
                    
                using(var dbTransaction = _context.Database.BeginTransaction())
                {
                    //Retornando conjunto referente a entidade
                    var dbSet = _context.Set<TEntity>();

                    //Adiciona o objeto no consulta
                    await dbSet.AddAsync(entity);

                    //Envia os dados para base de dados
                    await _context.SaveChangesAsync();

                    //Efetiva a transação na base de dados.
                    dbTransaction.Commit(); 
                }

                return true;

            }catch(Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception(ex.InnerException.Message);                
                else
                    throw new Exception(ex.Message);
            }

        }

        public async Task<bool> Update(TEntity entity)
        {
            try
            {

                using (var dbTransaction = _context.Database.BeginTransaction())
                {
                    var dbSet = _context.Set<TEntity>();

                    //A função FromResult transforma operação não Async para
                    //Async.
                    await Task.FromResult(_context.Update(entity));

                    await _context.SaveChangesAsync();

                    await dbTransaction.CommitAsync();
                }

                return true;

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception(ex.InnerException.Message);
                else
                    throw new Exception(ex.Message);
            }

        }
    }
}
