using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ChatTailorAI.Shared.Base;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Dto.Conversations;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.DataServices;

namespace ChatTailorAI.Shared.ViewModels.Dialogs
{
    public class EditChatDialogViewModel : Observable
    {
        private string _assistantId;
        private string _assistantName;
        private ConversationDto _currentChat;

        private readonly IAssistantDataService _assistantDataService;
        private readonly IModelManagerService _modelManagerService;

        public EditChatDialogViewModel(
            IAssistantDataService assistantDataService,
            IModelManagerService modelManagerService)
        {
            _assistantDataService = assistantDataService;
            _modelManagerService = modelManagerService;

            Models = new ObservableCollection<string>(_modelManagerService.GetAllModels());
        }

        public async Task RefreshModelsAsync()
        {
            // TODO: Fix this method, it's doing multiple hits on _currentChat
            // Also temp fix for CurrentChat.Model being null when Models are refreshed

            var currentChatModel = CurrentChat?.Model;

            await _modelManagerService.RefreshDynamicModelsAsync();

            var newModels = new ObservableCollection<string>(_modelManagerService.GetAllModels());

            // Swap the contents of the Models collection with newModels
            Models.Clear();
            foreach (var model in newModels)
            {
                Models.Add(model);
            }

            // Restore the selected model if it still exists
            if (Models.Contains(currentChatModel))
            {
                CurrentChat.Model = currentChatModel;
            }
            else
            {
                CurrentChat.Model = null;
            }

            OnPropertyChanged(nameof(CurrentChat));
        }

        public ConversationDto CurrentChat
        {
            get => _currentChat;
            set
            {
                SetProperty(ref _currentChat, value);

                // TODO: Find better way to get assistant name than this
                LoadAssistantNameAsync();
            }
        }
        public ObservableCollection<string> Models { get; set; }

        public string AssistantId
        {
            get => _assistantId;
            set => SetProperty(ref _assistantId, value);
        }

        public string AssistantName
        {
            get => _assistantName;
            set => SetProperty(ref _assistantName, value);
        }

        public bool IsAssistantConversation
        {
            get => CurrentChat.ConversationType != "Standard" && CurrentChat.ConversationType != "OpenAI";
        }

        private async void LoadAssistantNameAsync()
        {
            if (CurrentChat is AssistantConversationDto assistantConversation)
            {
                // TODO: Handle better if assistant has been deleted from db since convo was created
                // Try to keep reference to name after deletion somehow? Maybe just store name in convo?
                // or utilize IsDeleted field and keep deleted fields in db till archived is deleted? 
                AssistantId = assistantConversation.AssistantId;
                try
                {
                    var assistant = await ResolveAssistantDetailsAsync(assistantConversation.AssistantId);
                    if (assistant != null)
                    {
                        AssistantName = assistant.Name;
                    }
                    else
                    {
                        AssistantName = "N/A";
                    }
                }
                catch (Exception ex)
                {
                    // Possible assistant was already deleted from db
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private async Task<AssistantDto> ResolveAssistantDetailsAsync(string assistantId)
        {
            var assistant = await _assistantDataService.GetAssistantByIdAsync(assistantId);
            return assistant;
        }
    }
}