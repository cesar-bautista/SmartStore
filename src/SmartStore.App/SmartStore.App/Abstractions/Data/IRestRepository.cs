using System.Threading.Tasks;

namespace SmartStore.App.Abstractions.Data
{
    public interface IRestRepository
    {
        IRestRepository SetBaseAddress(string baseAddress);
        IRestRepository SetTimeout(int timeOut);
        IRestRepository SetHeader(string name, string value);

        Task<TResult> GetAsync<TResult>(string path, object content = null);
        Task<string> GetAsync(string path, string content = null);

        Task<TResult> PostAsync<TResult>(string path, object content);
        Task<TResult> PostAsync<TResult>(string path, string content = null);
        Task<string> PostAsync(string path, string content = null);
        
        Task<TResult> PutAsync<TResult>(string path, object content);
        Task<TResult> PutAsync<TResult>(string path, string content = null);
        Task<string> PutAsync(string path, string content = null);
        
        Task<TResult> DeleteAsync<TResult>(string path);
        Task<string> DeleteAsync(string path);
    }
}