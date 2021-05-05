using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;
using NerdStore.Catalogo.Domain.ValueObjects;

namespace NerdStore.Catalogo.Application.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProdutoViewModel, Produto>()
               .ConstructUsing(p => new Produto(p.Nome, p.Descricao,
               p.Imagem, p.Valor, p.Ativo, p.QuantidadeEstoque, p.CategoriaId,
               new Dimensoes(p.Altura, p.Largura, p.Profundidade)));

            CreateMap<CategoriaViewModel, Categoria>()
                     .ConstructUsing(c => new Categoria(c.Nome, c.Codigo));
        }
    }
}