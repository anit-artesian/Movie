
using System.Threading.Tasks;

namespace MediaApplication.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
