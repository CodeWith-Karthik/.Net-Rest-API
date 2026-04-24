using Microsoft.AspNetCore.Identity;
using Velora.Application.InputModel;

namespace Velora.Application.Services.Interface
{
    public interface IAuthService
    {
        Task<IEnumerable<IdentityError>> WorkspaceRegister(WorkspaceRegister workspaceRegister);

        Task<IEnumerable<IdentityError>> Register(Register register);

        Task<object> Login(Login login);

        Task<bool> IsUserExists(Guid id);
    }
}
