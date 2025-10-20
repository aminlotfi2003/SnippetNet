using SnippetNet.Application.Identity.Dtos;
using SnippetNet.Domain.Identity;

namespace SnippetNet.Application.Common.Services.Identity;

public interface ITokenService
{
    TokenPair GenerateTokenPair(ApplicationUser user);
    string ComputeHash(string value);
}
