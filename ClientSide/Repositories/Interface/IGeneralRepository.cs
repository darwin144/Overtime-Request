using ClientSide.ViewModel.Response;
using System.Security.Claims;

namespace ClientSide.Repositories.Interface
{
    public interface IGeneralRepository<T, X>
        where T : class
    {
        Task<ResponseListVM<T>> Get();
        Task<ResponseViewModel<T>> Get(Guid guid);
        Task<ResponseMessageVM> Post(T entity);
        Task<ResponseMessageVM> Put(T entity);
        Task<ResponseMessageVM> Deletes(Guid guid);
        IEnumerable<Claim> ExtractClaims(string jwtToken);
    }
}
