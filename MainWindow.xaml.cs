using JobSearchTracker.Models;
using JobSearchTracker.Services;
using JobSearchTracker.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JobSearchTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        private readonly PreferencesService _preferencesService;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            _preferencesService = new PreferencesService();
            DataContext = _viewModel;

            // Set random motivational title message
            Title = $"Job Search Tracker - {GetRandomMotivationalMessage()}";

            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;

            // Start loading preferences and store the task so we can await it later
            _loadPreferencesTask = LoadPreferencesAsync();

            // Enable drag-and-drop for JSON files
            AllowDrop = true;
            DragEnter += MainWindow_DragEnter;
            Drop += MainWindow_Drop;
        }

        private string GetRandomMotivationalMessage()
        {
            var messages = new[]
            {
                "You've got this!",
                "Your dream job is out there!",
                "Keep pushing forward!",
                "Job searching sucks, but you don't!",
                "Today could be the day!",
                "One step closer to your goal!",
                "You're doing amazing!",
                "Believe in yourself!",
                "Your next opportunity awaits!",
                "Stay positive, stay persistent!",
                "You're stronger than you think!",
                "Great things take time!",
                "You belong at the table!",
                "Your skills are valuable!",
                "Don't give up now!",
                "Success is just around the corner!",
                "You're more qualified than you realize!",
                "Keep showing up!",
                "Your persistence will pay off!",
                "You're on the right path!",
                "Every application is progress!",
                "You're building something great!",
                "Trust the process!",
                "Your effort matters!",
                "Small steps lead to big results!",
                "You're not alone in this journey!",
                "Your breakthrough is coming!",
                "Stay focused on your goals!",
                "You've overcome challenges before!",
                "This is just a chapter, not the whole story!",
                "Your best days are ahead!",
                "Keep refining your craft!",
                "You bring unique value!",
                "Rejection redirects you to something better!",
                "Your determination is inspiring!",
                "One day at a time!",
                "You're making progress, even when it's hard to see!",
                "The right fit is worth the wait!",
                "Your resilience is your superpower!",
                "Keep learning, keep growing!",
                "You deserve success!",
                "Don't let setbacks define you!",
                "Your story isn't over yet!",
                "Consistency beats perfection!",
                "You're investing in your future!",
                "Stay hungry, stay humble!",
                "Your next chapter starts now!",
                "Embrace the journey!",
                "You're capable of amazing things!",
                "Keep your head up, champion!"
            };

            var random = new Random();
            return messages[random.Next(messages.Length)];
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // IMPORTANT: Wait for preferences to load before doing anything else
            // This ensures LastOpenedFilePath is available
            await EnsurePreferencesLoadedAsync();

            // Show intro dialog if preferences indicate it should be shown
            if (_viewModel.UserPreferences.ShowIntroOnStartup)
            {
                var welcomeDialog = new Views.WelcomeDialog();
                welcomeDialog.ShowDialog();

                if (welcomeDialog.DontShowAgain)
                {
                    _viewModel.UserPreferences.ShowIntroOnStartup = false;
                    await _preferencesService.SavePreferencesAsync(_viewModel.UserPreferences);
                }
            }

            // Auto-load last opened file if it exists
            await AutoLoadLastProjectAsync();

            // Check for software updates if enabled
            await CheckForUpdatesAsync();
        }

        private bool _preferencesLoaded = false;
        private Task? _loadPreferencesTask = null;

        /// <summary>
        /// Ensures preferences are loaded before proceeding.
        /// </summary>
        private async Task EnsurePreferencesLoadedAsync()
        {
            if (_preferencesLoaded)
                return;

            // Wait for the existing load task to complete
            if (_loadPreferencesTask != null)
            {
                await _loadPreferencesTask;
            }

            _preferencesLoaded = true;
        }

        /// <summary>
        /// Attempts to automatically load the last opened project file.
        /// </summary>
        private async Task AutoLoadLastProjectAsync()
        {
            var lastFilePath = _viewModel.UserPreferences.LastOpenedFilePath;

            // Only attempt auto-load if there's a path and no project currently loaded
            if (string.IsNullOrWhiteSpace(lastFilePath))
                return;

            try
            {
                // Try to load the last file (without showing success/error messages)
                bool loaded = await _viewModel.LoadProjectFromFileAsync(lastFilePath, showSuccessMessage: false);

                if (loaded)
                {
                    // Show subtle notification in status bar or title
                    Title += $" - {System.IO.Path.GetFileNameWithoutExtension(lastFilePath)} (auto-loaded)";
                }
                else
                {
                    // File couldn't be loaded (deleted, moved, etc.)
                    // Clear the last opened path so we don't try again
                    _viewModel.UserPreferences.LastOpenedFilePath = string.Empty;
                    await _preferencesService.SavePreferencesAsync(_viewModel.UserPreferences);
                }
            }
            catch
            {
                // Silently fail - don't interrupt user startup experience
                _viewModel.UserPreferences.LastOpenedFilePath = string.Empty;
                await _preferencesService.SavePreferencesAsync(_viewModel.UserPreferences);
            }
        }

        /// <summary>
        /// Checks for software updates if the preference is enabled.
        /// </summary>
        private async Task CheckForUpdatesAsync()
        {
            // Only check if user has enabled update checking
            if (!_viewModel.UserPreferences.CheckForUpdates)
                return;

            try
            {
                var updateService = new UpdateService();
                var latestVersion = await updateService.CheckForUpdatesAsync();

                if (latestVersion != null)
                {
                    // Show update notification in bottom left
                    UpdateVersionRun.Text = latestVersion;
                    UpdateNotificationTextBlock.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                // Silently fail - don't interrupt user experience
            }
        }

        /// <summary>
        /// Opens the download website when the update notification is clicked.
        /// </summary>
        private void UpdateNotification_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://www.galafik.com/job-search-tracker/",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open website: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the dragged data contains files
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Only allow single JSON files
                if (files != null && files.Length == 1 && files[0].EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }

            e.Handled = true;
        }

        private async void MainWindow_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files != null && files.Length == 1 && files[0].EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    var filePath = files[0];

                    // Check if we need confirmation (project already loaded)
                    bool hasProject = _viewModel.Jobs.Count > 0;

                    if (hasProject)
                    {
                        var result = MessageBox.Show(
                            "You have a project currently open. Do you want to close it and open the dropped file?",
                            "Open Dropped File?",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question
                        );

                        if (result != MessageBoxResult.Yes)
                            return;
                    }

                    // Load the dropped file
                    await _viewModel.LoadProjectFromFileAsync(filePath, showSuccessMessage: true);

                    // Save preferences to update last opened file
                    await _preferencesService.SavePreferencesAsync(_viewModel.UserPreferences);
                }
            }

            e.Handled = true;
        }

        private async void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Check for unsaved changes
            if (!_viewModel.CheckUnsavedChanges())
            {
                e.Cancel = true;
                return;
            }

            // Save preferences (including last opened file path)
            try
            {
                await _preferencesService.SavePreferencesAsync(_viewModel.UserPreferences);
            }
            catch
            {
                // Don't block closing if preferences can't be saved
            }
        }

        private async Task LoadPreferencesAsync()
        {
            try
            {
                _viewModel.UserPreferences = await _preferencesService.LoadPreferencesAsync();
                ApplyTheme(_viewModel.UserPreferences.Theme);
                _viewModel.CurrentSortOption = _viewModel.UserPreferences.DefaultSortBy;
                _viewModel.StatusFilterText = _viewModel.UserPreferences.LastStatusFilter;
            }
            catch
            {
                // Use defaults if preferences can't be loaded
            }
        }

        private void ApplyTheme(AppTheme theme)
        {
            var themeUri = theme == AppTheme.Dark
                ? new Uri("Themes/DarkTheme.xaml", UriKind.Relative)
                : new Uri("Themes/LightTheme.xaml", UriKind.Relative);

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(
                new ResourceDictionary { Source = themeUri }
            );
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void PreferencesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var preferencesDialog = new Views.PreferencesDialog(_viewModel.UserPreferences);

            // Handle live theme preview
            preferencesDialog.ThemeChanged += (s, theme) => ApplyTheme(theme);

            if (preferencesDialog.ShowDialog() == true)
            {
                _viewModel.UserPreferences = preferencesDialog.Preferences;
                ApplyTheme(_viewModel.UserPreferences.Theme);
                _viewModel.CurrentSortOption = _viewModel.UserPreferences.DefaultSortBy;

                try
                {
                    await _preferencesService.SavePreferencesAsync(_viewModel.UserPreferences);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save preferences: {ex.Message}", "Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Revert theme if canceled
                ApplyTheme(_viewModel.UserPreferences.Theme);
            }
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Job Search Tracker\n\n" +
                "A comprehensive application for tracking job applications, interviews, and contacts.\n\n" +
                "Features:\n" +
                "- Track multiple job search projects\n" +
                "- Manage job applications with detailed information\n" +
                "- Record interviews and contacts\n" +
                "- Filter, search, and sort jobs\n" +
                "- Get directions to job locations\n" +
                "- Export data to Excel\n" +
                "- Email integration\n" +
                "- Light and Dark themes\n\n" +
                "Version 0.1.5\n\n" +
                "© 2026 Gal Afik",
                "About Job Search Tracker",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        private void AnalyticsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Jobs == null || _viewModel.Jobs.Count == 0)
            {
                MessageBox.Show(
                    "No data available for analytics.\n\nPlease create a project and add some jobs first.",
                    "Analytics Unavailable",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                return;
            }

            // Convert JobViewModels to Jobs for analytics
            var jobs = _viewModel.Jobs.Select(jvm => jvm.Model).ToList();
            var analyticsWindow = new Views.AnalyticsWindow(jobs);
            analyticsWindow.ShowDialog();
        }

        private void ChangelogMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var changelogWindow = new Views.ChangelogWindow();
            changelogWindow.ShowDialog();
        }

        private void ReportBugMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/GalAfik/AIJobSearchTracker/issues",
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show(
                    "Could not open the bug reporting page.\n\nPlease visit:\nhttps://github.com/GalAfik/AIJobSearchTracker/issues",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }

        private void JobsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.SelectedJob != null)
            {
                _viewModel.EditJobCommand.Execute(null);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = e.Uri.AbsoluteUri,
                    UseShellExecute = true
                });
                e.Handled = true;
            }
            catch
            {
                MessageBox.Show("Failed to open URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Inline Editing Event Handlers

        private void SaveJobButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedJob == null)
                return;

            // The bindings are already updating the model, so just notify user
            MessageBox.Show("Job changes saved successfully!", "Saved", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ReloadJobButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedJob == null)
                return;

            var currentJob = _viewModel.SelectedJob;
            _viewModel.SelectedJob = null;
            _viewModel.SelectedJob = currentJob;

            if (!_viewModel.UserPreferences.SuppressWarnings)
            {
                MessageBox.Show("Job data reloaded.", "Reloaded", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OpenJobUrlButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedJob == null || string.IsNullOrWhiteSpace(_viewModel.SelectedJob.JobUrl))
                return;

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = _viewModel.SelectedJob.JobUrl,
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show("Failed to open URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Interview Management

        private void AddInterviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedJob == null)
                return;

            var newInterview = new Interview();
            var dialog = new Views.InterviewDialog(newInterview);

            if (dialog.ShowDialog() == true)
            {
                _viewModel.SelectedJob.Model.Interviews.Add(newInterview);
                _viewModel.SelectedJob.Interviews.Add(new InterviewViewModel(newInterview));
            }
        }

        private void InterviewsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (InterviewsListView.SelectedItem is InterviewViewModel interviewVM)
            {
                var dialog = new Views.InterviewDialog(interviewVM.Model);
                if (dialog.ShowDialog() == true)
                {
                    // Refresh the view
                    var index = _viewModel.SelectedJob.Interviews.IndexOf(interviewVM);
                    _viewModel.SelectedJob.Interviews[index] = new InterviewViewModel(interviewVM.Model);
                }
            }
        }

        private void EditInterviewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (InterviewsListView.SelectedItem is InterviewViewModel interviewVM)
            {
                var dialog = new Views.InterviewDialog(interviewVM.Model);
                if (dialog.ShowDialog() == true)
                {
                    var index = _viewModel.SelectedJob.Interviews.IndexOf(interviewVM);
                    _viewModel.SelectedJob.Interviews[index] = new InterviewViewModel(interviewVM.Model);
                }
            }
        }

        private void DeleteInterviewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (InterviewsListView.SelectedItem is InterviewViewModel interviewVM)
            {
                var result = MessageBox.Show(
                    "Are you sure you want to delete this interview?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.SelectedJob.Model.Interviews.Remove(interviewVM.Model);
                    _viewModel.SelectedJob.Interviews.Remove(interviewVM);
                }
            }
        }

        // Contact Management

        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedJob == null)
                return;

            var newContact = new Contact();
            var dialog = new Views.ContactDialog(newContact);

            if (dialog.ShowDialog() == true)
            {
                _viewModel.SelectedJob.Model.Contacts.Add(newContact);
                _viewModel.SelectedJob.Contacts.Add(new ContactViewModel(newContact));
            }
        }

        private void ContactsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ContactsListView.SelectedItem is ContactViewModel contactVM)
            {
                var dialog = new Views.ContactDialog(contactVM.Model);
                if (dialog.ShowDialog() == true)
                {
                    var index = _viewModel.SelectedJob.Contacts.IndexOf(contactVM);
                    _viewModel.SelectedJob.Contacts[index] = new ContactViewModel(contactVM.Model);
                }
            }
        }

        private void EditContactMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (ContactsListView.SelectedItem is ContactViewModel contactVM)
            {
                var dialog = new Views.ContactDialog(contactVM.Model);
                if (dialog.ShowDialog() == true)
                {
                    var index = _viewModel.SelectedJob.Contacts.IndexOf(contactVM);
                    _viewModel.SelectedJob.Contacts[index] = new ContactViewModel(contactVM.Model);
                }
            }
        }

        private void DeleteContactMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (ContactsListView.SelectedItem is ContactViewModel contactVM)
            {
                var result = MessageBox.Show(
                    "Are you sure you want to delete this contact?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.SelectedJob.Model.Contacts.Remove(contactVM.Model);
                    _viewModel.SelectedJob.Contacts.Remove(contactVM);
                }
            }
        }

        private void SponsorButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/sponsors/GalAfik",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Unable to open sponsor link: {ex.Message}\n\n" +
                    "Please visit: https://github.com/sponsors/GalAfik",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
    }
}