using AutoMapper;
using ControleEnderecos.Models;
using ControleEnderecos.ViewModel;
namespace ControleEnderecos.Application
{
    /// <summary>
    /// Configura os mapeamentos para as classes do AutoMapper.
    /// </summary>
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile() { 
            //Criando o mapeamento entre as classes
            CreateMap<Usuario, RegistrarViewModel>();
            CreateMap<RegistrarViewModel, Usuario>();

            CreateMap<Cidade, CidadeViewModel>();
            CreateMap<CidadeViewModel, Cidade>();


        }
    }
}
