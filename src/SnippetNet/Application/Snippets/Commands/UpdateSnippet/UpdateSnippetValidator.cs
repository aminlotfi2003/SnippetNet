using FluentValidation;
using SnippetNet.Application.DTOs.Snippets;

namespace SnippetNet.Application.Snippets.Commands.UpdateSnippet;

public class UpdateSnippetValidator : AbstractValidator<UpdateSnippetDto>
{
    public UpdateSnippetValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Language).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Code).NotEmpty();
        RuleForEach(x => x.Tags).NotEmpty().MaximumLength(50);
    }
}
