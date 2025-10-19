using AutoMapper;
using SnippetNet.Application.DTOs.Snippets;
using SnippetNet.Application.ViewModels;
using SnippetNet.Domain.Entities;

namespace SnippetNet.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ViewModel -> DTO
        CreateMap<SnippetListVm, SnippetDto>().ReverseMap();
        CreateMap<SnippetDetailsVm, SnippetDto>().ReverseMap();

        // DTO -> Entity
        CreateMap<CreateSnippetDto, Snippet>()
            .ForMember(d => d.SnippetTags, opt => opt.Ignore());

        CreateMap<UpdateSnippetDto, Snippet>()
            .ForMember(d => d.SnippetTags, opt => opt.Ignore());

        // Entity -> DTO/VM
        CreateMap<Snippet, SnippetDto>()
            .ForMember(d => d.Tags, opt =>
                opt.MapFrom(s => s.SnippetTags.Select(st => st.Tag.Name).ToList()));

        CreateMap<Snippet, SnippetListVm>();
        CreateMap<Snippet, SnippetDetailsVm>()
            .ForMember(d => d.Tags, opt =>
                opt.MapFrom(s => s.SnippetTags.Select(st => st.Tag.Name).ToList()));
    }
}
