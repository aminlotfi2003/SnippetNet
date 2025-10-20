using FluentValidation;

namespace SnippetNet.Application.Snippets.Commands.CreateSnippet;

public class CreateSnippetCommandValidator : AbstractValidator<CreateSnippetCommand>
{
    public CreateSnippetCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Language).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.TagName).NotEmpty().MaximumLength(50);
    }
}
