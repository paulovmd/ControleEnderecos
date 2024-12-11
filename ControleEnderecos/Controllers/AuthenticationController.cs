using ControleEnderecos.Application;
using ControleEnderecos.Models;
using ControleEnderecos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;



namespace ControleEnderecos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        //Representa a nossa conexão com o banco de dados
        private ControleEnderecoContext _context;

        public AuthenticationController(ControleEnderecoContext context) { 
            _context = context;
        }

        [HttpPost("Logar")]        
        public async Task<IActionResult> Logar([FromBody]LoginViewModel loginViewModel)
        {
            ResultApplication<Usuario> resultApplication = new ResultApplication<Usuario>();

            /*
             * Verificando se o usuário existe no sistema
             */
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(
                entidade => entidade.Login == loginViewModel.Login 
                         ||  entidade.Email == loginViewModel.Login 
                );

            if (usuario == null)
            {
                resultApplication.Message = "Usuário ou senha Inválidos.";
                return Unauthorized(resultApplication);
            }

            //Converte o salt armazenado na tabela do usuário para byte[]
            byte[] salt = Convert.FromBase64String(usuario.Salt);

            //Pega a senha que o usuário digitou durante o login e aplica o hash
            var password = Helper.GenerateSHA256Hash(loginViewModel.Password);

            //Pega a senha a digitada que foi o hash e salt para retorna o valor orignal
            //da senha criptografada
            var passHash = Helper.GetPasswordHash(password, salt);

            if ( (usuario == null) || (passHash != usuario.Password) )
            {
                resultApplication.Message = "Usuário ou senha Inválidos.";
                return Unauthorized(resultApplication);
            }
            
            resultApplication.Success = true;
            resultApplication.Message = "Usuário logado sucesso!";
            return Ok(resultApplication);
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar([FromBody]RegistrarViewModel registrarViewModel)
        {
            ResultApplication<Usuario> resultApplication = new ResultApplication<Usuario>();

            if (!ModelState.IsValid)
            {
                resultApplication.Message = "Há campos a serem preenchidos";
                return BadRequest(resultApplication);
            }

            //Trocar para o automapper
            Usuario usuario = new Usuario();            
            usuario.Nome = registrarViewModel.Nome;
            usuario.Login = registrarViewModel.Login;   
            usuario.Telefone = registrarViewModel.Telefone;
            usuario.Email = registrarViewModel.Email;
            usuario.Status = "A";

            ///Passos para criptografar a senha do usuário
            byte[] salt = Helper.GenerateSaltToBytes();

            usuario.Salt = Convert.ToBase64String(salt);

            var passNew = Helper.GenerateSHA256Hash(registrarViewModel.Senha);

            var passHash = Helper.GetPasswordHash(passNew, salt);            

            usuario.Password = passHash;

            try
            {
                //Utilizando controle de transação
                using (var dbTransaction = _context.Database.BeginTransaction())
                {
                    await _context.Usuarios.AddAsync(usuario);

                    //Salva as alterações no context sem efetivar os dados no banco
                    await _context.SaveChangesAsync();

                    //Confirma a transação da operação e efetiva de fato
                    //os dados na base de dados
                    await dbTransaction.CommitAsync();

                }

            } catch (Exception ex) {
                resultApplication.Message = "Ocorreu um erro interno!";
                resultApplication.Error = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError
                        , resultApplication);
            }
            
            resultApplication.Success = true;
            resultApplication.Message = "Usuário registrado com sucesso!";
            return Ok(resultApplication);

        }

    }
}
