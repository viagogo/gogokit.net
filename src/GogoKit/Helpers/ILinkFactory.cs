using System.Threading.Tasks;
using GogoKit.Models;
using HalKit.Models;

namespace GogoKit.Helpers
{
    public interface ILinkFactory
    {
        Task<Link> CreateLinkAsync(string relativeUriFormat, params object[] args);
    }
}