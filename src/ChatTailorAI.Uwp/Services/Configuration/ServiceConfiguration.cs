using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ChatTailorAI.DataAccess.Database;
using ChatTailorAI.DataAccess.Repositories;
using ChatTailorAI.Shared.Repositories;
using ChatTailorAI.Shared.Services.Chat;
using ChatTailorAI.Shared.Services.Image;
using ChatTailorAI.Shared.Services.Speech;
using ChatTailorAI.Shared.Services.Assistants.OpenAI;
using ChatTailorAI.Shared.Services.Database.Repositories;
using ChatTailorAI.Shared.Services.DataServices;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Services.DataService;
using ChatTailorAI.Services.Chat;
using ChatTailorAI.Services.Image;
using ChatTailorAI.Services.Speech;
using ChatTailorAI.Services.Assistants.OpenAI;
using ChatTailorAI.Services.Events;
using ChatTailorAI.Shared.Services.Events;
using ChatTailorAI.Services.Uwp.Settings;
using ChatTailorAI.Services.Uwp.Dispatching;
using ChatTailorAI.Services.Uwp.FileManagement;
using ChatTailorAI.Services.Uwp.Audio;
using ChatTailorAI.Services.Uwp.System;
using ChatTailorAI.Uwp.Factories;
using ChatTailorAI.Shared.Factories;
using ChatTailorAI.Services.Uwp.Notification;
using ChatTailorAI.Services.Tools;
using ChatTailorAI.Shared.Mappers;
using ChatTailorAI.Services.DataServices;
using ChatTailorAI.Shared.Services.Chat.OpenAI;
using ChatTailorAI.Shared.Services.Tools;
using ChatTailorAI.Shared.Services.Authentication;
using ChatTailorAI.Shared.Services.Files;
using ChatTailorAI.Shared.Factories.ViewModels;
using ChatTailorAI.Shared.Builders;
using ChatTailorAI.Shared.Transformers.OpenAI;
using ChatTailorAI.Shared.Factories.Interfaces;
using ChatTailorAI.Shared.Mappers.Interfaces;
using ChatTailorAI.Services.Chat.OpenAI;
using ChatTailorAI.Shared.ViewModels.Dialogs;
using ChatTailorAI.Shared.ViewModels.Pages;
using ChatTailorAI.DataAccess.Database.Providers.SQLite;
using ChatTailorAI.Shared.Transformers.Anthropic;
using ChatTailorAI.Shared.Services.Chat.Anthropic;
using ChatTailorAI.Services.Chat.Anthropic;
using ChatTailorAI.Shared.Mappers.ViewModels;
using ChatTailorAI.Shared.Transformers.Google;
using ChatTailorAI.Shared.Services.Chat.Google;
using ChatTailorAI.Services.Chat.Google;
using ChatTailorAI.Services.Uwp;
using ChatTailorAI.Shared.Services.Chat.LMStudio;
using ChatTailorAI.Services.Chat.LMStudio;
using ChatTailorAI.Shared.Transformers.LMStudio;
using ChatTailorAI.Services.Common;
using ChatTailorAI.Services.Tools.Executors;
using ChatTailorAI.Services.Authentication;
using ChatTailorAI.Uwp.Services.UI.Navigation;
using ChatTailorAI.Services.Uwp.UI.Navigation;
using ChatTailorAI.Uwp.Services.UI.Dialogs;
using ChatTailorAI.Services.Uwp.UI.Views;
using ChatTailorAI.Services.Image.OpenAI;
using ChatTailorAI.Shared.Services.Audio;
using ChatTailorAI.Shared.Services.Common.Navigation;

namespace ChatTailorAI.Uwp.Services.Configuration
{
    public static class ServiceConfiguration
    {
        public static IHost BuildServiceHost()
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    ConfigureServices(services, hostContext.Configuration);
                })
                .UseConsoleLifetime();

            return builder.Build();
        }

        /// <summary>
        ///Configure services for dependency injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appsettings"></param>
        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ILoggerService, NLogService>();

            // Database Configuration
            services.AddDbContext<SQLiteDb>(optionsBuilder =>
            {
                DbContextConfiguration.ConfigureDbContextOptions(optionsBuilder, configuration);
            });
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
            services.AddSingleton<DbInitializer>();

            // Event Aggregators
            services.AddSingleton<IEventAggregator, EventAggregator>();
            services.AddSingleton<IGenericEventAggregator, GenericEventAggregator>();

            // Factories
            services.AddSingleton<IDialogFactory, DialogFactory>();
            services.AddSingleton<IPageTypeResolver, PageTypeResolver>();
            services.AddSingleton<IEditChatDialogViewModelFactory, EditChatDialogViewModelFactory>();
            services.AddSingleton<IChatMessageViewModelFactory, ChatMessageViewModelFactory>();
            services.AddSingleton<IChatSettingsFactory, ChatSettingsFactory>();

            // Mappers
            services.AddSingleton<IConversationMapper, ConversationMapper>();
            services.AddSingleton<IAssistantMapper, AssistantMapper>();
            services.AddSingleton<IMessageMapper, MessageMapper>();
            services.AddSingleton<IImageMapper, ImageMapper>();
            services.AddSingleton<IPromptMapper, PromptMapper>();
            services.AddSingleton<IChatMessageViewModelMapper, ChatMessageViewModelMapper>();

            // Data Services
            services.AddSingleton<IAssistantDataService, AssistantDataService>();
            services.AddSingleton<IConversationDataService, ConversationDataService>();
            services.AddSingleton<IMessageDataService, MessageDataService>();
            services.AddSingleton<IImageDataService, ImageDataService>();
            services.AddSingleton<IPromptDataService, PromptDataService>();

            // Repositories
            services.AddSingleton<IAssistantRepository, AssistantRepository>();
            services.AddSingleton<IConversationRepository, ConversationRepository>();
            services.AddSingleton<IMessageRepository, MessageRepository>();
            services.AddSingleton<IImageRepository, ImageRepository>();
            services.AddSingleton<IPromptRepository, PromptRepository>();

            // Services
            services.AddSingleton<IAppSettingsService, AppSettingsService>();
            services.AddSingleton<IUserSettingsService, UserSettingsService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IChatPageNavigationService, ChatPageNavigationService>();
            services.AddSingleton<IDispatcherService, DispatcherService>();
            services.AddSingleton<IFileDownloadService, FileDownloadService>();
            services.AddSingleton<IImageFileService, ImageFileService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IFolderService, FolderService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddHttpClient<IFileDownloadService, FileDownloadService>();
            services.AddSingleton<IChatFileService, ChatFileService>();
            services.AddHttpClient<IOpenAIChatService, OpenAIChatService>();
            services.AddHttpClient<IAnthropicChatService, AnthropicChatService>();
            services.AddHttpClient<IGoogleChatService, GoogleChatService>();
            services.AddHttpClient<ILMStudioChatService, LMStudioChatService>();
            services.AddHttpClient<IImageGenerationService, OpenAIDalleImageService>();
            services.AddSingleton<IImageService, ImageService>();
            services.AddSingleton<IAzureSpeechService, AzureSpeechService>();
            services.AddHttpClient<IOpenAISpeechService, OpenAISpeechService>();
            services.AddHttpClient<IElevenLabsSpeechService, ElevenLabsSpeechService>();
            services.AddHttpClient<IWhisperService, WhisperService>();
            services.AddSingleton<IWindowsClipboardService, WindowsClipboardService>();
            services.AddHttpClient<ISpotifyService, SpotifyService>();
            services.AddSingleton<IAudioRecorderService, AudioRecorderService>();
            services.AddSingleton<IAppNotificationService, AppNotificationService>();
            services.AddHttpClient<IOpenAIAssistantService, OpenAIAssistantService>();
            services.AddHttpClient<IOpenAIMessageService, OpenAIMessageService>();
            services.AddHttpClient<IOpenAIRunService, OpenAIRunService>();
            services.AddHttpClient<IOpenAIThreadService, OpenAIThreadService>();
            services.AddSingleton<IOpenAIAssistantManagerService, OpenAIAssistantManagerService>();
            services.AddSingleton<IAudioMediaPlayerService, AudioMediaPlayerService>();
            services.AddSingleton<IToolExecutorService, FunctionToolExecutorService>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<ISpeechService, SpeechService>();
            services.AddSingleton<IApplicationViewService, ApplicationViewService>();
            services.AddSingleton<IModelManagerService, ModelManagerService>();

            // Transformers
            services.AddSingleton<OpenAIChatMessageTransformer>();
            services.AddSingleton<AnthropicChatMessageTransformer>();
            services.AddSingleton<GoogleChatMessageTransformer>();
            services.AddSingleton<LMStudioChatMessageTransformer>();
            services.AddSingleton<IChatMessageTransformerFactory, ChatMessageTransformerFactory>();

            // Builders
            services.AddSingleton<OpenAIChatRequestBuilder>();
            services.AddSingleton<AnthropicChatRequestBuilder>();
            services.AddSingleton<GoogleChatRequestBuilder>();
            services.AddSingleton<LMStudioChatRequestBuilder>();
            services.AddSingleton<IChatRequestBuilderFactory, ChatRequestBuilderFactory>();

            // View Models
            services.AddTransient<ChatPageViewModel>();
            services.AddTransient<SettingsPageViewModel>();
            services.AddSingleton<ShellPageViewModel>();
            services.AddTransient<AssistantDialogViewModel>();
            services.AddTransient<AssistantsPageViewModel>();
            services.AddTransient<ChatsPageViewModel>();
            services.AddTransient<NewChatDialogViewModel>();
            services.AddTransient<PromptsPageViewModel>();
            services.AddTransient<ImagesPageViewModel>();
            services.AddTransient<NewImageDialogViewModel>();
            services.AddTransient<NewPromptDialogViewModel>();
        }
    }
}