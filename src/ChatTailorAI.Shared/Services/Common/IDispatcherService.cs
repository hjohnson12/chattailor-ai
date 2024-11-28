using System;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Common
{
    public interface IDispatcherService
    {
        Task RunOnUIThreadAsync(Action action);
    }
}
