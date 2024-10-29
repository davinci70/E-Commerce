using e_commerce.Models.Auth;

namespace e_commerce.Services.IService
{
    public interface ILoginService
    {
        Task<AuthModel> LoginAsync(TokenRequestModel model);
    }
}
