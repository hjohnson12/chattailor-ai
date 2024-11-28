using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Assistants.OpenAI;
using ChatTailorAI.Shared.Services.Assistants.OpenAI;

namespace ChatTailorAI.Services.Assistants.OpenAI
{
    public class OpenAIAssistantManagerService : IOpenAIAssistantManagerService
    {
        private readonly IOpenAIAssistantService _openAIAssistantService;
        private readonly IOpenAIThreadService _openAIThreadService;
        private readonly IOpenAIMessageService _openAIMessageService;
        private readonly IOpenAIRunService _openAIRunService;

        public OpenAIAssistantManagerService(
            IOpenAIAssistantService openAIAssistantService,
            IOpenAIThreadService openAIThreadService,
            IOpenAIMessageService openAIMessageService,
            IOpenAIRunService openAIRunService)
        {
            _openAIAssistantService = openAIAssistantService;
            _openAIThreadService = openAIThreadService;
            _openAIMessageService = openAIMessageService;
            _openAIRunService = openAIRunService;
        }

        public async Task<string> CreateAssistant(AssistantDto assistant)
        {
            var newAssistant = await _openAIAssistantService
                .CreateAssistantAsync(assistant);

            return newAssistant.Id;
        }

        public async Task DeleteAssistant(string id)
        {
            await _openAIAssistantService.DeleteAssistantAsync(id);
        }

        public async Task<AssistantDto> GetAssistant(string id)
        {
            return await _openAIAssistantService.RetrieveAssistantAsync(id);
        }

        public async Task<List<AssistantDto>> GetAssistants()
        {
            return await _openAIAssistantService.ListAssistantsAsync();
        }

        public async Task<List<OpenAIThreadMessage>> SendMessage(string assistantId, string threadId, string message)
        {
            var openAIMessage = await _openAIMessageService.CreateMessageAsync(threadId, message);
            var openAIRun = await _openAIRunService.CreateRunAsync(assistantId, threadId);

            var currentRun = await _openAIRunService.RetrieveRunAsync(openAIRun.Id, threadId);
            while (currentRun.Status != "completed")
            {
                Thread.Sleep(1000);
                currentRun = await _openAIRunService.RetrieveRunAsync(openAIRun.Id, threadId);
            }

            if (currentRun.Status == "completed")
            {
                // Get messages from OpenAI thread
                var messages = await _openAIMessageService.ListMessagesAsync(threadId);
                var userMessageIndex = messages.FindLastIndex(m => m.Id == openAIMessage.Id);

                // TODO: When assistant is OpenAI and uses tools, multiple messages can be returned
                var assistantMessages  = messages.Take(userMessageIndex).ToList();
                return assistantMessages;
            } 
            else if (currentRun.Status == "failed")
            {
                throw new Exception("Run failed");
            } 
            else
            {
                throw new Exception("Run status unknown");
            }
        }

        public async Task UpdateAssistant(AssistantDto assistant)
        {
            await _openAIAssistantService.ModifyAssistantAsync(assistant.ExternalAssistantId, assistant.Model, assistant.Name, assistant.Description, assistant.Instructions);
        }

        public async Task<OpenAIThread> CreateThreadAsync()
        {
            // TODO: Allow messages to be prefilled in the thread

            var threadId = await _openAIThreadService.CreateThreadAsync();
            return threadId;
        }
    }
}