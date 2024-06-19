using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatTailorAI.Shared.Services.Tools
{
    public interface IToolExecutorService
    {
        /// <summary>
        /// Executes a tool with the given name and arguments.
        /// </summary>
        /// <param name="toolName">The name of the tool to execute.</param>
        /// <param name="toolArguments">The arguments to pass to the tool.</param>
        /// <returns>A task that represents the asynchronous operation and resolves with the result of the tool execution.</returns>
        Task<string> Execute(string toolName, Dictionary<string, string> toolArguments);
    }
}
