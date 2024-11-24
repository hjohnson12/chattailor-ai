using ChatTailorAI.Shared.Base;
using ChatTailorAI.Shared.Dto;
using ChatTailorAI.Shared.Enums;
using ChatTailorAI.Shared.Events;
using ChatTailorAI.Shared.Models.Prompts;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.DataServices;
using ChatTailorAI.Shared.Services.Events;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatTailorAI.Shared.ViewModels.Pages
{
    public class PromptsPageViewModel : Observable
    {
        private readonly IAppNotificationService _appNotificationService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPromptDataService _promptDataService;
        private readonly ILoggerService _loggerService;

        private ObservableCollection<PromptDto> _chatPrompts;
        private PromptDto _selectedPrompt;

        public PromptsPageViewModel(
            IAppNotificationService appNotificationService,
            IDialogService dialogService,
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            IPromptDataService promptDataService,
            ILoggerService loggerService)
        {
            _appNotificationService = appNotificationService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _promptDataService = promptDataService;
            _loggerService = loggerService;

            _eventAggregator.PromptCreated += OnPromptCreated;

            Prompts = new ObservableCollection<PromptDto>();

            EditPromptCommand = new AsyncRelayCommand(EditPrompt);
            SelectPromptCommand = new AsyncRelayCommand<PromptDto>(SelectPrompt);
            ShowCreatePromptDialogCommand = new AsyncRelayCommand(ShowCreatePromptDialog);
            DeleteCommand = new AsyncRelayCommand(DeletePrompt);
            LoadedCommand = new AsyncRelayCommand(LoadPrompts);
            StartChatCommand = new AsyncRelayCommand<PromptDto>(StartChat);
        }

        public ICommand EditPromptCommand { get; private set; }
        public ICommand SelectPromptCommand { get; private set; }
        public ICommand ShowCreatePromptDialogCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand LoadedCommand { get; private set; }
        public ICommand StartChatCommand { get; private set; }

        public bool IsPromptsEmpty => Prompts.Count == 0;

        public ObservableCollection<PromptDto> Prompts
        {
            get => _chatPrompts;
            set
            {
                SetProperty(ref _chatPrompts, value);
                OnPropertyChanged(nameof(IsPromptsEmpty));
            }
        }

        public PromptDto SelectedPrompt
        {
            get => _selectedPrompt;
            set => SetProperty(ref _selectedPrompt, value);
        }

        private async Task LoadPrompts()
        {
            try
            {
                var prompts = await _promptDataService
                    .GetAllAsync();

                // For now only show standard prompts
                // Eventually add a filter to show all prompts or a designated area
                // for prompts tied to an image generation
                var chatPrompts = prompts
                    .Where(p => p.PromptType == PromptType.Standard)
                    .OrderByDescending(p => p.CreatedAt);

                Prompts = new ObservableCollection<PromptDto>(chatPrompts);
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Failed to load prompts");
                _appNotificationService.Display(ex.Message);
            }
        }

        private async Task EditPrompt()
        {
            try
            {
                if (SelectedPrompt == null)
                {
                    _appNotificationService.Display("Please select a prompt to edit");
                    return;
                }

                var backupCopy = SelectedPrompt;

                bool result = await _dialogService.ShowEditPromptDialogAsync(SelectedPrompt);
                if (result != true)
                {
                    // User cancelled, revert changes
                    SelectedPrompt = backupCopy;
                    return;
                }

                await _promptDataService.UpdateAsync(SelectedPrompt);
                var index = Prompts.IndexOf(backupCopy);
                Prompts[index] = SelectedPrompt;

                _appNotificationService.Display("Prompt updated successfully");
               
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Failed to edit prompt");
                _appNotificationService.Display($"Unable to edit prompt: {ex.Message}");
            }
        }

        public async Task ShowCreatePromptDialog()
        {
            await _dialogService.ShowCreatePromptDialogAsync();
        }

        private async Task SelectPrompt(PromptDto prompt)
        {
            //_appNotificationService.Display("Selected prompt: " + prompt.Title);
            SelectedPrompt = prompt;
        }

        private async void OnPromptCreated(object sender, PromptCreatedEvent e)
        {
            await CreatePrompt(e.Prompt);
        }

        private async Task CreatePrompt(Prompt prompt)
        {
            try
            {
                var promptDto = new PromptDto
                {
                    Title = prompt.Title,
                    Content = prompt.Content,
                    IsActive = prompt.IsActive,
                    PromptType = prompt.PromptType
                };

                await _promptDataService.AddPromptAsync(promptDto);

                Prompts.Add(promptDto);
                OnPropertyChanged(nameof(IsPromptsEmpty));
                _appNotificationService.Display("Prompt created successfully");
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Failed to create prompt");

                // TODO: Handle rollback
                _appNotificationService.Display(ex.Message);
            }
        }

        private async Task DeletePrompt()
        {
            // Delete multiple selected at one point, currently
            // just delete the selected one if any
            try
            {
                if (SelectedPrompt == null)
                {
                    _appNotificationService.Display("Please select a prompt to delete");
                    return;
                }

                var result = await _dialogService.ShowDeleteDialogAsync();
                if (result == false)
                {
                    return;
                }

                await _promptDataService.DeleteAsync(SelectedPrompt);

                Prompts.Remove(SelectedPrompt);
                OnPropertyChanged(nameof(IsPromptsEmpty));
                _appNotificationService.Display("Prompt deleted successfully");
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Failed to delete prompt");
                _appNotificationService.Display(ex.Message);
            }
        }

        public async Task StartChat(PromptDto promptDto)
        {
            try
            {
                // A new chat is started with the selected prompt
                _navigationService.NavigateTo(NavigationPageKeys.ChatPage, promptDto);
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Failed to start a new chat");
                _appNotificationService.Display($"Failed to start a new chat: {ex.Message}");
            }
        }
    }
}
