using System.Threading.Tasks;
using HalKit.Models.Response;

namespace GogoKit.Services
{
    public interface ILinkFactory
    {
        Task<Link> CreateLinkAsync(string relativeUriFormat, params object[] args);
    }
}