using JobSearchTracker.Models;
using JobSearchTracker.Services;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace JobSearchTracker.ViewModels
{
    /// <summary>
    /// Main ViewModel for the application.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly ProjectService _projectService;
        private readonly ExportService _exportService;
        private readonly CsvService _csvService;
        private readonly EmailService _emailService;
        private readonly DirectionsService _directionsService;

        private JobSearchProject? _currentProject;
        private string? _currentFilePath;
        private JobViewModel? _selectedJob;
        private string _filterText = string.Empty;
        private string _statusFilterText = "All";
        private string _currentSortOption = "Date Added (Newest)";
        private UserPreferences _userPreferences = new UserPreferences();
        private bool _hasUnsavedChanges = false;

        /// <summary>
        /// Gets or sets the user preferences.
        /// </summary>
        public UserPreferences UserPreferences
        {
            get => _userPreferences;
            set
            {
                if (_userPreferences != null)
                {
                    _userPreferences.PropertyChanged -= UserPreferences_PropertyChanged;
                }

                if (SetProperty(ref _userPreferences, value))
                {
                    if (_userPreferences != null)
                    {
                        _userPreferences.PropertyChanged += UserPreferences_PropertyChanged;
                    }
                }
            }
        }

        /// <summary>
        /// Handles property changes on UserPreferences to bubble up notifications.
        /// </summary>
        private void UserPreferences_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Bubble up the property change notification for nested property bindings
            OnPropertyChanged(nameof(UserPreferences));

            // Also raise notification for direct properties that wrap UserPreferences properties
            if (e.PropertyName == nameof(UserPreferences.UseCompactView))
            {
                OnPropertyChanged(nameof(UseCompactView));
            }
        }

        /// <summary>
        /// Gets or sets whether the current project has unsaved changes.
        /// </summary>
        public bool HasUnsavedChanges
        {
            get => _hasUnsavedChanges;
            set => SetProperty(ref _hasUnsavedChanges, value);
        }

        public MainViewModel()
        {
            _projectService = new ProjectService();
            _exportService = new ExportService();
            _csvService = new CsvService();
            _emailService = new EmailService();
            _directionsService = new DirectionsService();

            Jobs = new ObservableCollection<JobViewModel>();
            FilteredJobs = new ObservableCollection<JobViewModel>();

            // Subscribe to UserPreferences PropertyChanged to enable nested property bindings
            _userPreferences.PropertyChanged += UserPreferences_PropertyChanged;

            // Initialize commands
            NewProjectCommand = new RelayCommand(_ => NewProject());
            LoadProjectCommand = new RelayCommand(_ => LoadProject());
            ImportCsvCommand = new RelayCommand(_ => ImportCsv());
            SaveProjectCommand = new RelayCommand(_ => SaveProject(), _ => _currentProject != null);
            SaveProjectAsCommand = new RelayCommand(_ => SaveProjectAs(), _ => _currentProject != null);
            ExportToExcelCommand = new RelayCommand(_ => ExportToExcel(), _ => _currentProject != null);
            ExportToCsvCommand = new RelayCommand(_ => ExportToCsv(), _ => _currentProject != null);
            AddJobCommand = new RelayCommand(_ => AddJob(), _ => _currentProject != null);
            AddJobFromUrlCommand = new RelayCommand(_ => AddJobFromUrl(), _ => _currentProject != null);
            EditJobCommand = new RelayCommand(_ => EditJob(), _ => SelectedJob != null);
            DeleteJobCommand = new RelayCommand(_ => DeleteJob(), _ => SelectedJob != null);
            ApplyFilterCommand = new RelayCommand(_ => ApplyFilter());
            ClearFilterCommand = new RelayCommand(_ => ClearFilter());
            GetDirectionsCommand = new RelayCommand(_ => GetDirections(), _ => SelectedJob != null && !string.IsNullOrWhiteSpace(SelectedJob.Location));
        }

        #region Properties

        public ObservableCollection<JobViewModel> Jobs { get; }
        public ObservableCollection<JobViewModel> FilteredJobs { get; }

        public JobViewModel? SelectedJob
        {
            get => _selectedJob;
            set => SetProperty(ref _selectedJob, value);
        }

        public string ProjectName => _currentProject?.Name ?? "No Project Loaded";

        /// <summary>
        /// Gets or sets whether to use compact view for the job list.
        /// This is a direct property to avoid nested binding issues with DataTriggers.
        /// </summary>
        public bool UseCompactView
        {
            get => UserPreferences.UseCompactView;
            set
            {
                if (UserPreferences.UseCompactView != value)
                {
                    UserPreferences.UseCompactView = value;
                    OnPropertyChanged(nameof(UseCompactView));
                }
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                if (SetProperty(ref _filterText, value))
                {
                    ApplyFilter();
                }
            }
        }

        public string StatusFilterText
        {
            get => _statusFilterText;
            set
            {
                if (SetProperty(ref _statusFilterText, value))
                {
                    ApplyFilter();
                }
            }
        }

        public string CurrentSortOption
        {
            get => _currentSortOption;
            set
            {
                if (SetProperty(ref _currentSortOption, value))
                {
                    ApplyFilter();
                }
            }
        }

        #endregion

        #region Commands

        public RelayCommand NewProjectCommand { get; }
        public RelayCommand LoadProjectCommand { get; }
        public RelayCommand ImportCsvCommand { get; }
        public RelayCommand SaveProjectCommand { get; }
        public RelayCommand SaveProjectAsCommand { get; }
        public RelayCommand ExportToExcelCommand { get; }
        public RelayCommand ExportToCsvCommand { get; }
        public RelayCommand AddJobCommand { get; }
        public RelayCommand AddJobFromUrlCommand { get; }
        public RelayCommand EditJobCommand { get; }
        public RelayCommand DeleteJobCommand { get; }
        public RelayCommand ApplyFilterCommand { get; }
        public RelayCommand ClearFilterCommand { get; }
        public RelayCommand GetDirectionsCommand { get; }

        #endregion

        #region Methods

        private void NewProject()
        {
            var dialog = new Views.NewProjectDialog();
            if (dialog.ShowDialog() == true)
            {
                _currentProject = _projectService.CreateNewProject(dialog.ProjectName, dialog.ProjectDescription);
                _currentFilePath = null;

                Jobs.Clear();
                FilteredJobs.Clear();
                HasUnsavedChanges = false;
                OnPropertyChanged(nameof(ProjectName));

                MessageBox.Show($"Project '{_currentProject.Name}' created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void LoadProject()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                InitialDirectory = _projectService.DefaultDirectory
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _currentProject = await _projectService.LoadProjectAsync(dialog.FileName);
                    _currentFilePath = dialog.FileName;

                    LoadProjectIntoViewModel(_currentProject);

                    HasUnsavedChanges = false;
                    MessageBox.Show($"Project '{_currentProject.Name}' loaded successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadProjectIntoViewModel(JobSearchProject project)
        {
            _currentProject = project;

            // Clear everything first to prevent mixing old and new jobs
            FilteredJobs.Clear();
            Jobs.Clear();
            SelectedJob = null;

            // Reset filters BEFORE adding new jobs to prevent filter from running on old data
            _filterText = string.Empty;
            _statusFilterText = "All";
            OnPropertyChanged(nameof(FilterText));
            OnPropertyChanged(nameof(StatusFilterText));

            // Now add the new jobs
            foreach (var job in project.Jobs)
            {
                Jobs.Add(new JobViewModel(job));
            }

            // Apply filter to populate FilteredJobs with new jobs
            ApplyFilter();

            OnPropertyChanged(nameof(ProjectName));
        }

        private async void SaveProject()
        {
            if (_currentProject == null)
                return;

            try
            {
                SyncJobsToProject();
                _currentFilePath = await _projectService.SaveProjectAsync(_currentProject, _currentFilePath);
                HasUnsavedChanges = false;
                MessageBox.Show("Project saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to save project: {ex.Message}\n\nPath: {_currentFilePath ?? "auto-generated"}\n\nDetails: {ex.GetType().Name}";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SaveProjectAs()
        {
            if (_currentProject == null)
                return;

            // Sanitize the project name for use as a filename
            var sanitizedName = string.Join("_", _currentProject.Name.Split(Path.GetInvalidFileNameChars()));
            sanitizedName = sanitizedName.Trim(' ', '.');
            if (string.IsNullOrWhiteSpace(sanitizedName))
            {
                sanitizedName = "Untitled_Project";
            }

            var dialog = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                InitialDirectory = _projectService.DefaultDirectory,
                FileName = sanitizedName + ".json"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    SyncJobsToProject();
                    var selectedPath = dialog.FileName;
                    _currentFilePath = await _projectService.SaveProjectAsync(_currentProject, selectedPath);
                    HasUnsavedChanges = false;
                    MessageBox.Show($"Project saved successfully!\n\nSaved to: {_currentFilePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    var errorMessage = $"Failed to save project: {ex.Message}\n\nAttempted path: {dialog.FileName}\n\nException: {ex.GetType().Name}\n\nStack trace: {ex.StackTrace}";
                    MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportToExcel()
        {
            if (_currentProject == null)
                return;

            var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = _currentProject.Name + "_Export.xlsx"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    SyncJobsToProject();
                    _exportService.ExportToExcel(_currentProject, dialog.FileName);
                    MessageBox.Show("Data exported successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to export data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportToCsv()
        {
            if (_currentProject == null)
                return;

            var dialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = _currentProject.Name + "_Export.csv"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    SyncJobsToProject();
                    _csvService.ExportToCsv(_currentProject, dialog.FileName);
                    MessageBox.Show("Data exported to CSV successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to export data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void ImportCsv()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var projectName = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName);
                    var importedProject = _csvService.ImportFromCsv(dialog.FileName, projectName, "Imported from CSV");

                    if (_currentProject == null)
                    {
                        // No project loaded - create a new project with imported jobs
                        LoadProjectIntoViewModel(importedProject);
                        _currentFilePath = null;
                        HasUnsavedChanges = true;
                        OnPropertyChanged(nameof(ProjectName));
                        MessageBox.Show($"Successfully imported {importedProject.Jobs.Count} jobs from CSV and created a new project!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // Add imported jobs to existing project
                        int importedCount = 0;
                        foreach (var job in importedProject.Jobs)
                        {
                            _currentProject.Jobs.Add(job);
                            Jobs.Add(new JobViewModel(job));
                            importedCount++;
                        }

                        HasUnsavedChanges = true;
                        ApplyFilter();
                        MessageBox.Show($"Successfully imported {importedCount} jobs from CSV into the current project!\n\nTotal jobs in project: {_currentProject.Jobs.Count}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Auto-save if preference is enabled
                        if (UserPreferences.AutoSaveAfterImport && _currentFilePath != null)
                        {
                            try
                            {
                                SyncJobsToProject();
                                await _projectService.SaveProjectAsync(_currentProject, _currentFilePath);
                                HasUnsavedChanges = false;
                                MessageBox.Show("Project auto-saved successfully!", "Auto-Save", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception saveEx)
                            {
                                MessageBox.Show($"Auto-save failed: {saveEx.Message}\n\nPlease save manually.", "Auto-Save Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to import CSV: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddJob()
        {
            if (_currentProject == null)
                return;

            var newJob = new Job();
            var dialog = new Views.JobDialog(newJob);

            if (dialog.ShowDialog() == true)
            {
                _currentProject.Jobs.Add(newJob);
                var jobViewModel = new JobViewModel(newJob);
                Jobs.Add(jobViewModel);
                HasUnsavedChanges = true;
                ApplyFilter();
            }
        }

        private void AddJobFromUrl()
        {
            if (_currentProject == null)
                return;

            var dialog = new Views.AddJobFromUrlDialog(UserPreferences);

            if (dialog.ShowDialog() == true && dialog.ScrapedJob != null)
            {
                _currentProject.Jobs.Add(dialog.ScrapedJob);
                var jobViewModel = new JobViewModel(dialog.ScrapedJob);
                Jobs.Add(jobViewModel);
                HasUnsavedChanges = true;
                ApplyFilter();

                // Optionally, allow the user to edit the scraped job
                var result = MessageBox.Show(
                    "Would you like to review and edit the job details?",
                    "Edit Job",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    SelectedJob = jobViewModel;
                    EditJob();
                }
            }
        }

        private void EditJob()
        {
            if (SelectedJob == null)
                return;

            var dialog = new Views.JobDialog(SelectedJob.Model);
            if (dialog.ShowDialog() == true)
            {
                SelectedJob.RefreshAllProperties();
                HasUnsavedChanges = true;
                ApplyFilter();
            }
        }

        private void DeleteJob()
        {
            if (SelectedJob == null || _currentProject == null)
                return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete the job '{SelectedJob.CompanyName} - {SelectedJob.JobTitle}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                _currentProject.Jobs.Remove(SelectedJob.Model);
                Jobs.Remove(SelectedJob);
                FilteredJobs.Remove(SelectedJob);
                HasUnsavedChanges = true;
                SelectedJob = null;
            }
        }

        private void ApplyFilter()
        {
            FilteredJobs.Clear();

            var filtered = Jobs.AsEnumerable();

            // Apply status filter
            if (!string.IsNullOrEmpty(StatusFilterText) && StatusFilterText != "All")
            {
                filtered = filtered.Where(j => j.Status.ToString() == StatusFilterText.Replace(" ", ""));
            }

            // Apply text filter
            if (!string.IsNullOrWhiteSpace(FilterText))
            {
                var searchText = FilterText.ToLower();
                filtered = filtered.Where(j =>
                    j.CompanyName.ToLower().Contains(searchText) ||
                    j.JobTitle.ToLower().Contains(searchText) ||
                    j.Location.ToLower().Contains(searchText)
                );
            }

            // Apply sorting
            filtered = ApplySorting(filtered);

            foreach (var job in filtered)
            {
                FilteredJobs.Add(job);
            }
        }

        private IEnumerable<JobViewModel> ApplySorting(IEnumerable<JobViewModel> jobs)
        {
            return CurrentSortOption switch
            {
                "Date Added (Newest)" => jobs.OrderByDescending(j => j.DateAdded),
                "Date Added (Oldest)" => jobs.OrderBy(j => j.DateAdded),
                "Company Name (A-Z)" => jobs.OrderBy(j => j.CompanyName),
                "Company Name (Z-A)" => jobs.OrderByDescending(j => j.CompanyName),
                "Date Applied (Newest)" => jobs.OrderByDescending(j => j.DateApplied ?? DateTime.MinValue),
                "Date Applied (Oldest)" => jobs.OrderBy(j => j.DateApplied ?? DateTime.MaxValue),
                "Status" => jobs.OrderBy(j => j.Status),
                _ => jobs.OrderByDescending(j => j.DateAdded)
            };
        }

        private void ClearFilter()
        {
            FilterText = string.Empty;
            StatusFilterText = "All";
            ApplyFilter();
        }

        private void GetDirections()
        {
            if (SelectedJob == null || string.IsNullOrWhiteSpace(SelectedJob.Location))
                return;

            try
            {
                _directionsService.GetDirections(UserPreferences.HomeAddress, SelectedJob.Location);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open directions: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SyncJobsToProject()
        {
            if (_currentProject == null)
                return;

            _currentProject.Jobs.Clear();
            foreach (var jobViewModel in Jobs)
            {
                _currentProject.Jobs.Add(jobViewModel.Model);
            }
        }

        public void ComposeEmail(string email, string subject = "")
        {
            try
            {
                _emailService.ComposeEmail(email, subject);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open email client: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ComposeGmail(string email, string subject = "")
        {
            try
            {
                _emailService.ComposeGmail(email, subject);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open Gmail: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ComposeOutlookWeb(string email, string subject = "")
        {
            try
            {
                _emailService.ComposeOutlookWeb(email, subject);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open Outlook Web: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Checks if there are unsaved changes and prompts the user to save.
        /// </summary>
        /// <returns>True if the operation should continue (no changes or user saved/discarded), false if user cancelled.</returns>
        public bool CheckUnsavedChanges()
        {
            if (!HasUnsavedChanges || _currentProject == null)
                return true;

            var result = MessageBox.Show(
                $"You have unsaved changes in '{_currentProject.Name}'.\\n\\nDo you want to save before closing?",
                "Unsaved Changes",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                // Try to save the project
                if (_currentFilePath != null)
                {
                    SaveProject();
                    return true;
                }
                else
                {
                    SaveProjectAs();
                    // If still has unsaved changes after SaveProjectAs, user cancelled the save dialog
                    return !HasUnsavedChanges;
                }
            }
            else if (result == MessageBoxResult.No)
            {
                // User wants to discard changes
                return true;
            }
            else
            {
                // User cancelled
                return false;
            }
        }

        #endregion
    }
}
