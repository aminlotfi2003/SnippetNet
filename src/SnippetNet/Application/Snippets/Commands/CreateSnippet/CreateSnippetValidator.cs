using FluentValidation;
using SnippetNet.Application.DTOs.Snippets;

namespace SnippetNet.Application.Snippets.Commands.CreateSnippet;

public class CreateSnippetValidator : AbstractValidator<CreateSnippetDto>
{
    public CreateSnippetValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Language).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Code).NotEmpty();
        RuleForEach(x => x.Tags).NotEmpty().MaximumLength(50);
    }
}
