using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.DataServices;
using ChatTailorAI.Shared.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTailorAI.Shared.Factories.ViewModels
{
    public interface IEditChatDialogViewModelFactory
    {
        EditChatDialogViewModel Create(ConversationDto conversation);
    }

    public class EditChatDialogViewModelFactory : IEditChatDialogViewModelFactory
    {
        private readonly IAssistantDataService _assistantDataService;
        private readonly IModelManagerService _modelManagerService;

        public EditChatDialogViewModelFactory(IAssistantDataService assistantDataService, IModelManagerService modelManagerService)
        {
            _assistantDataService = assistantDataService;
            _modelManagerService = modelManagerService;
        }

        public EditChatDialogViewModel Create(ConversationDto conversation)
        {
            return new EditChatDialogViewModel(_assistantDataService, _modelManagerService)
            {
                CurrentChat = conversation
            };
        }
    }
}
