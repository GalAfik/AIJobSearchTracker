# Job Search Tracker

A comprehensive WPF desktop application for tracking job applications, interviews, and networking contacts during your job search.

## Features

### Project Management
- **Create Multiple Projects**: Organize different job searches (e.g., "2026 Software Engineer Search")
- **Save/Load Projects**: All data stored locally in JSON format
- **Project Metadata**: Track project creation date, description, and last modified date

### Job Tracking
- **Comprehensive Job Details**:
  - Company Name
  - Job Title
  - Job URL
  - Location
  - Salary Range
  - Date Posted
  - Date Applied
  - Application Platform (LinkedIn EasyApply, Company Website, Indeed, etc.)
  - Job Description
  - Notes

- **Job Status Management**:
  - Not Applied
  - Applied
  - Interviewed
  - Offered
  - Accepted
  - Rejected (automatically grayed out but still editable)
  - Withdrawn

### Interview Management
- **Unlimited Interviews per Job**
- **Interview Details**:
  - Round Number
  - Interview Type (Phone, Video, In-Person, Technical)
  - Date and Time
  - Interviewer Name and Email
  - Outcome
  - Notes

### Contact Management
- **Track Important Contacts** at each company
- **Contact Information**:
  - Name
  - Email
  - Position/Title
  - Notes

### Email Integration
- **Direct Email Buttons** for each contact
- **Multiple Email Options**:
  - Default email client (mailto:)
  - Gmail (web)
  - Outlook Web

### Filtering and Search
- **Text Search**: Search across company names, job titles, and locations
- **Status Filter**: Filter by application status
- **Real-time Filtering**: Updates as you type
- **Visual Indicators**: Rejected jobs are grayed out with reduced opacity

### Data Export
- **Excel Export**: Export all data to a well-formatted Excel file
- **Multiple Worksheets**:
  - Jobs: All job details
  - Interviews: All interview records
  - Contacts: All contact information
  - Summary: Project statistics and overview

## Architecture

### MVVM Pattern
The application follows the Model-View-ViewModel (MVVM) architectural pattern for clean separation of concerns:

- **Models**: Pure data classes (Job, Interview, Contact, JobSearchProject)
- **ViewModels**: Business logic and data presentation (MainViewModel, JobViewModel, etc.)
- **Views**: XAML-based UI (MainWindow, JobDialog, etc.)

### Project Structure
```
JobSearchTracker/
├── Models/
│   ├── ApplicationPlatform.cs
│   ├── JobStatus.cs
│   ├── Job.cs
│   ├── Interview.cs
│   ├── Contact.cs
│   └── JobSearchProject.cs
├── ViewModels/
│   ├── ViewModelBase.cs
│   ├── RelayCommand.cs
│   ├── MainViewModel.cs
│   ├── JobViewModel.cs
│   ├── InterviewViewModel.cs
│   └── ContactViewModel.cs
├── Views/
│   ├── NewProjectDialog.xaml/.cs
│   ├── JobDialog.xaml/.cs
│   ├── InterviewDialog.xaml/.cs
│   └── ContactDialog.xaml/.cs
├── Services/
│   ├── ProjectService.cs
│   ├── ExportService.cs
│   └── EmailService.cs
├── Converters/
│   ├── JobStatusToBackgroundConverter.cs
│   └── JobStatusToOpacityConverter.cs
├── MainWindow.xaml/.cs
└── App.xaml/.cs
```

## Technical Details

- **Framework**: .NET 10.0
- **UI Framework**: WPF (Windows Presentation Foundation)
- **Language**: C# 14.0
- **Data Storage**: JSON files stored in `Documents/JobSearchTracker`
- **Dependencies**: ClosedXML for Excel export

## Usage

### Getting Started
1. Launch the application
2. Click "New Project" to create your first job search project
3. Enter a project name (e.g., "2026 Software Engineer Search")
4. Start adding jobs!

### Adding a Job
1. Click "Add Job" button or use File > Job > Add Job
2. Fill in job details in the dialog
3. Add interviews and contacts as needed
4. Click "Save"

### Managing Interviews
1. Open a job in the job dialog
2. Navigate to the "Interviews" tab
3. Click "Add Interview" to record a new interview
4. Fill in interview details including date, time, interviewer, and notes

### Managing Contacts
1. Open a job in the job dialog
2. Navigate to the "Contacts" tab
3. Click "Add Contact" to add a new contact
4. Use the Email/Gmail/Outlook buttons to compose emails directly

### Filtering Jobs
- Use the search box to find jobs by company, title, or location
- Use the status dropdown to filter by application status
- Click "Clear Filter" to reset filters

### Exporting Data
1. Click "Export to Excel" button
2. Choose a location and filename
3. Open the generated Excel file with all your data organized into worksheets

## Data Storage

All projects are saved as JSON files in:
```
C:\Users\[YourUsername]\Documents\JobSearchTracker\
```

Files are named after your project name (e.g., `2026 Software Engineer Search.json`)

## Code Quality

This application is built with high coding standards:
- ✅ Comprehensive XML documentation
- ✅ MVVM architectural pattern
- ✅ Separation of concerns
- ✅ Proper encapsulation
- ✅ Type safety
- ✅ Error handling
- ✅ Consistent naming conventions
- ✅ Clean, readable code

## Future Enhancements

Planned features for future versions:
- Auto-fill job details from URL
- Statistics dashboard
- Reminder notifications for follow-ups
- Import from LinkedIn/Indeed
- Cloud sync capabilities
- Mobile companion app

## License

This project is provided as-is for personal use.
