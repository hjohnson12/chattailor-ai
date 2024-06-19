using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Services.Store.Engagement;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ChatTailorAI.DataAccess.Database;
using ChatTailorAI.Uwp.Services.Configuration;
using ChatTailorAI.Uwp.Helpers;
using ChatTailorAI.Shared.Services.Common;
using ChatTailorAI.Shared.Services.Prompts;
using ChatTailorAI.Uwp.Views;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ChatTailorAI.Uwp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public IHost ServiceHost { get; set; }
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            var configuration = LoadConfiguration();
            var appCenterKey = configuration["AppCenter:Key"];

            // AppCenter is set to retire March 31, 2025
            // https://learn.microsoft.com/en-us/appcenter/retirement
            if (!string.IsNullOrEmpty(appCenterKey))
            {
                AppCenter.Start(appCenterKey, typeof(Analytics), typeof(Crashes));
            }

            ServiceHost = ServiceConfiguration.BuildServiceHost();
        }

        private IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsecrets.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(ShellPage), e.Arguments);
                }

                // Ensure the current window is active
                Window.Current.Activate();

                await InitializeApplicationAsync();
            }
        }

        /// <summary>
        /// Initialize parts of the application on startup
        /// </summary>
        /// <returns></returns>
        async Task InitializeApplicationAsync()
        {
            TitleBarHelper.ExpandViewIntoTitleBar();
            TitleBarHelper.SetupTitleBar();

            ConfigureLogger();
            await InitializeDatabaseAsync();
            await MigratePromptsFileToDatabase();
            await InitializeStoreEngagement();
            await CheckForUpdateOrFirstRun();
        }

        private void ConfigureLogger()
        {
            var logger = ServiceHost.Services.GetService<ILoggerService>();
            logger.Info("Starting ChatTailorAI.Uwp...");
        }

        private async Task InitializeDatabaseAsync()
        {
            var dbInitializer = ServiceHost.Services.GetService<DbInitializer>();
            await dbInitializer.InitializeAsync();
        }

        private async Task MigratePromptsFileToDatabase()
        {
            // Remove this in the future after all users have migrated off of the file
            // This will be when the app has been updated a few times and no users
            // are left on v1.3.1 or lower
            IPromptService promptService = ServiceHost.Services.GetService<IPromptService>();
            await promptService.MigratePromptsAsync();
        }

        private async Task InitializeStoreEngagement()
        {
            StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
            await engagementManager.RegisterNotificationChannelAsync();
        }

        /// <summary>
        /// Checks if the app is the first time ran or updated,
        /// displays a dialog accordingly
        /// </summary>
        /// <returns></returns>
        public async Task CheckForUpdateOrFirstRun()
        {
            IDialogService dialogService = ServiceHost.Services.GetService<IDialogService>();

            if (SystemInformation.Instance.IsAppUpdated)
            {
                await dialogService.ShowAppUpdatedDialogAsync();
            }
            else if (SystemInformation.Instance.IsFirstRun)
            {
                await dialogService.ShowFirstRunDialogAsync();
            }
        }


        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}