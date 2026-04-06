# Software Update Check Feature

## Overview
Added automatic software update checking functionality that notifies users when a new version is available on the official website.

## Implementation Details

### 1. Update Service (`Services/UpdateService.cs`)
- **Purpose**: Checks for updates by scraping the official website
- **URL**: https://www.galafik.com/job-search-tracker/
- **Method**: `CheckForUpdatesAsync()`
  - Fetches the website HTML
  - Uses regex pattern to find version numbers in download links: `Job[_\s-]?Search[_\s-]?Tracker[_\s-]?v?(\d+\.\d+\.\d+)`
  - Compares found versions with current version (0.1.4)
  - Returns latest version if newer, otherwise null
  - **Timeout**: 10 seconds
  - **Error Handling**: Silently fails on network errors

### 2. Version Comparison
- Uses `System.Version` class for proper semantic version comparison
- Correctly handles versions like: 0.1.4, 0.2.0, 1.0.0, etc.
- Compares major.minor.patch numbers

### 3. User Preference
- **Property**: `UserPreferences.CheckForUpdates`
- **Default**: `true` (enabled by default)
- **Location**: Preferences → Appearance tab → "Check for software updates on startup"
- **Behavior**: When disabled, no update check is performed

### 4. UI Components

#### Update Notification (MainWindow.xaml)
- **Location**: Bottom left of footer
- **Visibility**: Hidden by default, shown only when update available
- **Styling**:
  - Orange color (#FF8C00) for attention
  - Bold font weight
  - Clickable cursor (hand icon)
- **Content**: "🔔 Update available! Download v[X.X.X] at www.galafik.com"
- **Interaction**: Clicking opens the website in default browser

#### Preferences Toggle (PreferencesDialog.xaml)
- **Tab**: Appearance
- **Label**: "Check for software updates on startup"
- **Description**: "Automatically check for new versions when the application starts. A notification will appear if an update is available."

### 5. Startup Flow
1. Preferences load
2. Theme applied
3. Filter/sort settings restored
4. Last project auto-loaded (if available)
5. **Update check runs** (if enabled in preferences)
6. If update found, notification appears in footer

## User Experience

### When Update Available
1. User launches application
2. (Background) Update check runs silently
3. If newer version found, orange notification appears in bottom left
4. User can click notification to visit download page
5. User can ignore notification and continue working

### Disabling Updates
1. Go to File → Preferences (or Help → Preferences)
2. Navigate to Appearance tab
3. Uncheck "Check for software updates on startup"
4. Click Save
5. Future application launches will skip update check

## Technical Benefits
- **Non-blocking**: Update check runs asynchronously, doesn't delay startup
- **Fail-safe**: Network errors don't interrupt user experience
- **Privacy-focused**: No telemetry or user data sent
- **Configurable**: Users can opt-out via preferences
- **Minimal overhead**: 10-second timeout, single HTTP request

## Website Requirements
For this feature to work, your website (https://www.galafik.com/job-search-tracker/) should have download links in this format:
- `Job_Search_Tracker_v0.1.4.zip`
- `JobSearchTracker-v0.1.5.zip`
- `Job-Search-Tracker-0.2.0.exe`
- etc.

The regex pattern is flexible and will match variations in naming conventions as long as the version number (X.X.X format) is present after "Job Search Tracker".

## Future Enhancements (Optional)
- Add release notes display in update notification
- Implement in-app update download and installation
- Add update check to Help menu (manual check)
- Cache last check time to avoid excessive network requests
- Support different update channels (stable, beta)

## Testing
1. **Enabled State**:
   - Launch app with CheckForUpdates = true
   - Verify update check runs in background
   - Temporarily change UpdateService.CurrentVersion to lower version
   - Verify notification appears

2. **Disabled State**:
   - Uncheck preference toggle
   - Launch app
   - Verify no update check occurs
   - Verify no notification appears

3. **Network Failure**:
   - Disconnect internet
   - Launch app
   - Verify app starts normally without errors

4. **Click Behavior**:
   - Trigger update notification
   - Click on notification text
   - Verify browser opens to correct URL

## Files Modified
- `Services/UpdateService.cs` (NEW)
- `Models/UserPreferences.cs` (added CheckForUpdates property)
- `Views/PreferencesDialog.xaml` (added checkbox)
- `Views/PreferencesDialog.xaml.cs` (added checkbox handling)
- `MainWindow.xaml` (added update notification TextBlock)
- `MainWindow.xaml.cs` (added CheckForUpdatesAsync and click handler)
