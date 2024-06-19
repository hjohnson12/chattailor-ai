using ChatTailorAI.Shared.Base;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Enums;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Models.Assistants;
using ChatTailorAI.Shared.Models.Chat.OpenAI;
using ChatTailorAI.Shared.Services.Assistants.OpenAI;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.DataServices;
using ChatTailorAI.Shared.Services.Events;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatTailorAI.Shared.ViewModels.Pages
{
    public class AssistantsPageViewModel : Observable, IDisposable
    {
        private readonly IAssistantDataService _assistantDataService;
        private readonly IAppNotificationService _appNotificationService;
        private readonly IOpenAIAssistantManagerService _openAIAssistantManagerService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILoggerService _loggerService;

        private ObservableCollection<AssistantViewModel> _assistants;

        private AssistantViewModel _selectedAssistant;
        private ObservableCollection<AssistantViewModel> _selectedAssistants;
        public event EventHandler<AssistantCreatedEvent> AssistantCreated;

        public AssistantsPageViewModel(
            IAssistantDataService assistantDataService,
            IAppNotificationService appNotificationService,
            IOpenAIAssistantManagerService openAIAssistantManagerService,
            IDialogService dialogService,
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            ILoggerService loggerService
            )
        {
            _assistantDataService = assistantDataService;
            _appNotificationService = appNotificationService;
            _openAIAssistantManagerService = openAIAssistantManagerService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;

            Assistants = new ObservableCollection<AssistantViewModel>();

            _eventAggregator.AssistantCreated += OnAssistantCreated;
            //GenericEventAggregator.Instance.Subscribe<AssistantCreatedEvent>(OnAssistantCreated);

            CreateAssistantCommand = new AsyncRelayCommand<AssistantDto>(CreateAssistant);
            LoadedCommand = new AsyncRelayCommand(LoadAssistants);
            ShowCreateAssistantDialogCommand = new AsyncRelayCommand(ShowCreateAssistantDialog);
            DeleteAssistantsCommand = new AsyncRelayCommand(DeleteAssistants);
            SelectAssistantCommand = new AsyncRelayCommand<AssistantViewModel>(SelectAssistant);
            DeleteAssistantCommand = new AsyncRelayCommand<AssistantViewModel>(DeleteAssistant);
            StartChatCommand = new AsyncRelayCommand<AssistantViewModel>(StartChat);
            EditAssistantCommand = new AsyncRelayCommand(UpdateAssistant);
            ShowAssistantHelpCommand = new AsyncRelayCommand(ShowAssistantHelp);
        }

        ~AssistantsPageViewModel()
        {
            _eventAggregator.AssistantCreated -= OnAssistantCreated;
        }

        public ICommand CreateAssistantCommand { get; private set; }
        public ICommand LoadedCommand { get; private set; }
        public ICommand ShowCreateAssistantDialogCommand { get; private set; }
        public ICommand DeleteAssistantsCommand { get; private set; }
        public ICommand SelectAssistantCommand { get; private set; }
        public ICommand DeleteAssistantCommand { get; private set; }
        public ICommand StartChatCommand { get; private set; }
        public ICommand EditAssistantCommand { get; private set; }
        public ICommand ShowAssistantHelpCommand { get; private set; }

        public bool IsAssistantsEmpty => Assistants.Count == 0;

        public ObservableCollection<AssistantViewModel> Assistants
        {
            get => _assistants;
            set
            {
                SetProperty(ref _assistants, value);
                OnPropertyChanged(nameof(IsAssistantsEmpty));
            }
        }

        public AssistantViewModel SelectedAssistant
        {
            get => _selectedAssistant;
            set => SetProperty(ref _selectedAssistant, value);
        }

        public ObservableCollection<AssistantViewModel> SelectedAssistants
        {
            get => _selectedAssistants;
            set => SetProperty(ref _selectedAssistants, value);
        }

        public void Dispose()
        {
            _eventAggregator.AssistantCreated -= OnAssistantCreated;
        }

        private async void OnAssistantCreated(object sender, AssistantCreatedEvent e)
        {
            await CreateAssistant(e.Assistant);
        }

        private async void OnAssistantCreated(AssistantCreatedEvent assistantCreatedEvent)
        {
            await CreateAssistant(assistantCreatedEvent.Assistant);
        }

        public async Task LoadAssistants()
        {
            try
            {
                var assistantDtos = await _assistantDataService.GetAssistantsAsync();
                var assistantViewModels = assistantDtos.Select(dto => new AssistantViewModel(dto)).ToList();
                Assistants = new ObservableCollection<AssistantViewModel>(assistantViewModels);
            }
            catch (Exception ex)
            {
                _appNotificationService.Display($"Failed to load assistants: {ex.Message}");
            }
        }

        public async Task CreateAssistant(AssistantDto assistantDto)
        {
            if (!IsValid(assistantDto))
            {
                _appNotificationService.Display("Please insert valid assistant information");
                return;
            }

            try
            {
                assistantDto.Id = Guid.NewGuid().ToString();
                if (assistantDto.AssistantType == AssistantType.OpenAI)
                {
                    var externalAssistantId = await _openAIAssistantManagerService.CreateAssistant(assistantDto);
                    assistantDto.ExternalAssistantId = externalAssistantId;
                }

                await _assistantDataService.SaveAssistantAsync(assistantDto);

                // Handle rollback if errors below ? 
                var assistantViewModel = new AssistantViewModel(assistantDto);
                Assistants.Add(assistantViewModel);
                OnPropertyChanged(nameof(IsAssistantsEmpty));
                _appNotificationService.Display("Assistant saved successfully");
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(assistantDto.ExternalAssistantId))
                {
                    await _openAIAssistantManagerService.DeleteAssistant(assistantDto.ExternalAssistantId);
                }

                _appNotificationService.Display($"Failed to save assistant: {ex.Message}");
            }
        }

        public async Task ShowCreateAssistantDialog()
        {
            await _dialogService.ShowCreateAssistantDialogAsync();
        }

        public async Task DeleteAssistants()
        {
            try
            {
                await _assistantDataService.DeleteAssistantsAsync();
                Assistants = new ObservableCollection<AssistantViewModel>();
                SelectedAssistant = null; // Need? May set automatically with ObservableCollection change on GridView
            }
            catch (Exception ex)
            {
                _loggerService.Error($"Failed to delete assistants: {ex.Message}");
                _appNotificationService.Display($"Failed to delete assistants: {ex.Message}");
            }
        }

        public async Task UpdateAssistant()
        {
            if (SelectedAssistant == null)
            {
                _appNotificationService.Display("Please select an assistant to edit");
                return;
            }

            var backupCopy = SelectedAssistant.ToDto();
            var assistantDto = SelectedAssistant.ToDto();

            bool result = await _dialogService.ShowEditAssistantDialogAsync(assistantDto);
            if (result != true)
            {
                // User cancelled, revert changes to backup
                SelectedAssistant.UpdateFromDto(backupCopy);
                return;
            }

            try
            {
                await _assistantDataService.UpdateAssistantAsync(assistantDto);
                if (SelectedAssistant.AssistantType == AssistantType.OpenAI)
                {
                    await _openAIAssistantManagerService.UpdateAssistant(assistantDto);
                }

                SelectedAssistant.UpdateFromDto(assistantDto);
                _appNotificationService.Display("Assistant updated successfully");
            }
            catch (Exception ex)
            {
                _loggerService.Error($"Failed to update assistant: {ex.Message}");

                var assistantSaved = await _assistantDataService.GetAssistantAsync(assistantDto);
                if (assistantSaved.Name != backupCopy.Name)
                {
                    await _assistantDataService.UpdateAssistantAsync(backupCopy);
                }

                SelectedAssistant.UpdateFromDto(backupCopy);
                _appNotificationService.Display($"Failed to update assistant: {ex.Message}");
            }
        }

        private bool IsValid(AssistantDto assistant)
        {
            if (assistant == null)
            {
                return false;
            }

            return true;
        }

        public async Task SelectAssistant(AssistantViewModel assistant)
        {
            //_appNotificationService.Display($"Selected assistant: {assistant.Name}");
        }

        public async Task DeleteAssistant(AssistantViewModel assistant)
        {
            if (SelectedAssistant == null)
            {
                _appNotificationService.Display("Please select an assistant to delete");
                return;
            }

            var result = await _dialogService.ShowDeleteDialogAsync();
            if (result == false)
            {
                return;
            }

            try
            {
                var assistantToDelete = assistant ?? SelectedAssistant;
                await _assistantDataService.DeleteAssistantAsync(assistantToDelete.ToDto());

                if (assistantToDelete.AssistantType == AssistantType.OpenAI)
                {
                    var assistantExists = await _openAIAssistantManagerService.GetAssistant(assistantToDelete.ExternalAssistantId);
                    if (assistantExists != null)
                    {
                        await _openAIAssistantManagerService.DeleteAssistant(assistantToDelete.ExternalAssistantId);
                    }
                }

                Assistants.Remove(assistantToDelete);
                OnPropertyChanged(nameof(IsAssistantsEmpty));
                _appNotificationService.Display("Assistant deleted successfully");
            }
            catch (Exception ex)
            {
                _loggerService.Error($"Failed to delete assistant: {ex.Message}");
                _appNotificationService.Display($"Failed to delete assistant: {ex.Message}");
            }
        }

        public async Task StartChat(AssistantViewModel assistant)
        {
            try
            {
                var assistantDto = assistant.ToDto();

                var assistsant = await _openAIAssistantManagerService.GetAssistant(assistantDto.ExternalAssistantId);
                if (assistsant == null)
                {
                    _appNotificationService.Display("Assistant not found within OpenAI Assistants, please retry or delete the assistant if it no longer exists in OpenAI");
                    return;
                }

                // A new chat is created for the specified assistant
                _navigationService.NavigateTo(NavigationPageKeys.ChatPage, assistantDto);
            }
            catch (Exception ex)
            {
                _loggerService.Error($"Failed to start new chat, {ex.Message}");
                _appNotificationService.Display($"Failed to start new chat, {ex.Message}");
            }
        }

        private void SortAssistants(object parameter)
        {
            bool ascending = (bool)parameter;
            var sorted = ascending
                ? Assistants.OrderBy(a => a.Name)
                : Assistants.OrderByDescending(a => a.Name);

            Assistants = new ObservableCollection<AssistantViewModel>(sorted);
        }

        public async Task ShowAssistantHelp()
        {
            await _dialogService.ShowAssistantHelpDialogAsync();
        }
    }
}
