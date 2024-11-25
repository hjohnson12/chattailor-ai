using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChatTailorAI.Shared.Base;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Models.Tools;
using ChatTailorAI.Shared.Resources;

namespace ChatTailorAI.Shared.ViewModels.Dialogs
{
    public class EditAssistantDialogViewModel : Observable
    {
        public EditAssistantDialogViewModel()
        {
            Models = new ObservableCollection<string>(ModelConstants.AssistantChatModels);

            AssistantTools = new ObservableCollection<Tool>
            {
                new Tool { Type = "Code Interpreter" }
            };
        }

        public AssistantDto CurrentAssistant { get; set; }
        public ObservableCollection<string> Models { get; set; }

        private ObservableCollection<Tool> _assistantTools;
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

        public string AssistantId { get; set; }
        public string AssistantName { get; set; }
        public string AssistantDescription { get; set; }
        public string AssistantInstructions { get; set; }
        public string AssistantModel { get; set; }
    }
}