using System;
using System.Windows.Input;
using ChatTailorAI.Shared.Base;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Models.Prompts;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Events;

namespace ChatTailorAI.Shared.ViewModels.Dialogs
{
    public class NewPromptDialogViewModel : Observable
    {
        private readonly IAppNotificationService _appNotificationService;
        private readonly IEventAggregator _eventAggregator;
        private string _title;
        private string _content;

        public NewPromptDialogViewModel(
            IAppNotificationService appNotificationService,
            IEventAggregator eventAggregator)
        {
            _appNotificationService = appNotificationService;
            _eventAggregator = eventAggregator;

            CreatePromptCommand = new RelayCommand(CreatePrompt);
        }

        public ICommand CreatePromptCommand { get; private set; }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public void CreatePrompt()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Content))
                {
                    _appNotificationService.Display("Title and Content are required.");
                    return;
                }

                var prompt = new Prompt
                {
                    Title = Title,
                    Content = Content,
                    IsActive = false,
                    PromptType = PromptType.Standard
                };

                _eventAggregator.PublishNewPromptCreated(new PromptCreatedEvent
                {
                    Prompt = prompt
                });
            }
            catch (Exception ex)
            {
                _appNotificationService.Display(ex.Message);
            }
        }
    }
}