using JobSearchTracker.Models;
using JobSearchTracker.Services;
using JobSearchTracker.ViewModels;
using System;
using System.Diagnostics;
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

            LoadPreferencesAsync();
        }

        private async void LoadPreferencesAsync()
        {
            try
            {
                _viewModel.UserPreferences = await _preferencesService.LoadPreferencesAsync();
                ApplyTheme(_viewModel.UserPreferences.Theme);
                _viewModel.CurrentSortOption = _viewModel.UserPreferences.DefaultSortBy;
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
                "Version 2.0\n\n" +
                "© 2024 Gal Afik",
                "About Job Search Tracker",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
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
    }
}