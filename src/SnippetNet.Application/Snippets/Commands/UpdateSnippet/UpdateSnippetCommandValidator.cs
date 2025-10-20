using FluentValidation;

namespace SnippetNet.Application.Snippets.Commands.UpdateSnippet;

public class UpdateSnippetCommandValidator : AbstractValidator<UpdateSnippetCommand>
{
    public UpdateSnippetCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Language).IsInEnum();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.TagName).NotEmpty().MaximumLength(50);
    }
}
