using AutoMapper;
using ControleEnderecos.Application;
using ControleEnderecos.Models;
using ControleEnderecos.Models.Repository;
using ControleEnderecos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControleEnderecos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CidadeController : ControllerBase
    {
        //Objeto que representa a conexão com o banco de dados.
        private ControleEnderecoContext _context;
        //Objeto repositório para manipular os dados do banco.
        private RepositoryBase<Cidade> _repository;
        //Objeto responsável por buscar e realizar o mapeamento
        //das classes.
        private IMapper _mapper;

        public CidadeController(ControleEnderecoContext context, IMapper mapper)
        {
            _context = context;
            _repository = new RepositoryBase<Cidade>(_context);
            _mapper = mapper;
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] CidadeViewModel cidadeViewModel)
        {
            ResultApplication<Cidade> resultApplication = new ResultApplication<Cidade>();

            //Valida o estado do ViewModel
            if (!ModelState.IsValid)
            {
                return DadosInvalidos(resultApplication);
            }

            Cidade cidade = _mapper.Map<CidadeViewModel, Cidade>(cidadeViewModel);

            try { 
            
                await _repository.Insert(cidade);
                resultApplication.Message = "Operação realizada com sucesso!";
                return Ok(resultApplication);

            }           
            catch (Exception ex)
            {
                return InternalServerError(resultApplication, ex);
            }

        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CidadeViewModel cidadeViewModel)
        {
            ResultApplication<Cidade> resultApplication = new ResultApplication<Cidade>();

            if (!ModelState.IsValid)
            {
                return DadosInvalidos(resultApplication);
            }

            Cidade cidade = _mapper.Map<CidadeViewModel, Cidade>(cidadeViewModel);

            try
            {
                await _repository.Update(cidade);
                resultApplication.Message = "Dados atualizados com sucesso!";
                return Ok(resultApplication);
            }catch (Exception ex) { 
                return InternalServerError(resultApplication, ex);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            ResultApplication<Cidade> resultApplication = new ResultApplication<Cidade>();

            try
            {
                resultApplication.Success = true;
                resultApplication.Data = await _repository.GetAll();
                return Ok(resultApplication);
            }catch(Exception ex)
            {
                return InternalServerError(resultApplication, ex);
            }            
        }

        private IActionResult InternalServerError(ResultApplication<Cidade> resultApplication, Exception ex)
        {
            resultApplication.Message = ex.Message;
            return StatusCode(StatusCodes.Status500InternalServerError,
                resultApplication);
        }

        private IActionResult DadosInvalidos(ResultApplication<Cidade> resultApplication)
        {
            resultApplication.Message = "Há dados não informados!";
            resultApplication.Error = ModelState.ToString();
            return BadRequest(resultApplication);
        }


    }
}
