using ChatTailorAI.Shared.Base;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Models.Tools;
using ChatTailorAI.Shared.Resources;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;

namespace ChatTailorAI.Shared.ViewModels.Dialogs
{
    public class AssistantDialogViewModel : Observable
    {
        private readonly IAppNotificationService _appNotificationService;
        private readonly IEventAggregator _eventAggregator;
        private string _id;
        private string _name;
        private string _model;
        private string _description;
        private string _createdAt;
        private string _instructions;
        private AssistantType _assistantType;
        private ObservableCollection<string> _models;
        private string _selectedModel;

        private ObservableCollection<Tool> _assistantTools;

        public AssistantDialogViewModel(
            IAppNotificationService appNotificationService,
            IEventAggregator eventAggregator)
        {
            _appNotificationService = appNotificationService;
            _eventAggregator = eventAggregator;

            Models = new ObservableCollection<string>(ModelConstants.AssistantChatModels);

            AssistantTools = new ObservableCollection<Tool>
            {
                new Tool { Type = "Code Interpreter" }
                // Uncomment when implemented
                // "Retrieval",
                // "Functions"
            };

            SelectedModel = Models[0];

            CreateAssistantCommand = new RelayCommand(CreateAssistant);
        }
        public ICommand CreateAssistantCommand { get; private set; }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string CreatedAt
        {
            get => _createdAt;
            set => SetProperty(ref _createdAt, value);
        }

        public string Instructions
        {
            get => _instructions;
            set => SetProperty(ref _instructions, value);
        }

        public AssistantType Type
        {
            get => _assistantType;
            set => SetProperty(ref _assistantType, value);
        }

        public ObservableCollection<string> Models
        {
            get => _models;
            set => SetProperty(ref _models, value);
        }

        public string SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        public ObservableCollection<Tool> AssistantTools
        {
            get => _assistantTools;
            set => SetProperty(ref _assistantTools, value);
        }

        private IList<Tool> _selectedTools;
        public IList<Tool> SelectedTools
        {
            get => _selectedTools;
            set => SetProperty(ref _selectedTools, value);
        }

        public void CreateAssistant()
        {
            try
            {
                ValidateInput();

                var tools = (SelectedTools != null && SelectedTools.Count > 0)
                    ? SelectedTools.ToList()
                    : null;

                var assistant = new AssistantDto
                {
                    Name = Name,
                    Model = SelectedModel,
                    Description = Description,
                    CreatedAt = DateTime.Now.ToString(),
                    Instructions = Instructions,
                    AssistantType = AssistantType.OpenAI,
                    Tools = tools
                };

                _eventAggregator.PublishNewAssistantCreated(new AssistantCreatedEvent { Assistant = assistant });
                //GenericEventAggregator.Instance.Publish(new AssistantCreatedEventArgs { Assistant = assistant });
            }
            catch (Exception ex)
            {
                _appNotificationService.Display($"Failed to create assistant: {ex.Message}");
            }
        }

        private void ValidateInput()
        {
            if (string.IsNullOrEmpty(Name))
                throw new ValidationException("Name is required");
            if (string.IsNullOrEmpty(Description))
                throw new ValidationException("Description is required");
            if (string.IsNullOrEmpty(Instructions))
                throw new ValidationException("Instructions are required");
            if (string.IsNullOrEmpty(SelectedModel))
                throw new ValidationException("Model is required");
        }
    }
}
