# Persistent Filter and Sort Settings

## Overview
Enhanced the application to remember and automatically restore the last used "Sort By" and "Status Filter" settings when the program starts. This provides a seamless user experience by maintaining the user's preferred view state across sessions.

## Implementation Details

### 1. UserPreferences Enhancement
Added new property to `Models/UserPreferences.cs`:

```csharp
/// <summary>
/// Gets or sets the last used status filter.
/// </summary>
public string LastStatusFilter { get; set; } = "All";
```

This complements the existing `DefaultSortBy` property, which was already present but is now being actively saved.

### 2. MainViewModel Changes

#### Added PreferencesService
- Added `PreferencesService` field to enable preference saving from the ViewModel
- Instantiated in constructor: `_preferencesService = new PreferencesService();`

#### Enhanced Properties
Updated `StatusFilterText` and `CurrentSortOption` properties to auto-save preferences when changed:

**StatusFilterText Property:**
```csharp
public string StatusFilterText
{
    get => _statusFilterText;
    set
    {
        if (SetProperty(ref _statusFilterText, value))
        {
            ApplyFilter();
            
            // Save the preference
            UserPreferences.LastStatusFilter = value;
            _ = SavePreferencesAsync();
        }
    }
}
```

**CurrentSortOption Property:**
```csharp
public string CurrentSortOption
{
    get => _currentSortOption;
    set
    {
        if (SetProperty(ref _currentSortOption, value))
        {
            ApplyFilter();
            
            // Save the preference
            UserPreferences.DefaultSortBy = value;
            _ = SavePreferencesAsync();
        }
    }
}
```

#### New Helper Method
Added `SavePreferencesAsync()` method:
```csharp
/// <summary>
/// Saves user preferences asynchronously. Silently fails if save fails.
/// </summary>
private async Task SavePreferencesAsync()
{
    try
    {
        await _preferencesService.SavePreferencesAsync(UserPreferences);
    }
    catch
    {
        // Silently fail - don't interrupt user experience for preference save failures
    }
}
```

### 3. MainWindow Startup Restoration
Updated `LoadPreferencesAsync()` in `MainWindow.xaml.cs` to restore both settings:

```csharp
private async Task LoadPreferencesAsync()
{
    try
    {
        _viewModel.UserPreferences = await _preferencesService.LoadPreferencesAsync();
        ApplyTheme(_viewModel.UserPreferences.Theme);
        _viewModel.CurrentSortOption = _viewModel.UserPreferences.DefaultSortBy;
        _viewModel.StatusFilterText = _viewModel.UserPreferences.LastStatusFilter;  // NEW
    }
    catch
    {
        // Use defaults if preferences can't be loaded
    }
}
```

## User Experience Flow

### First Launch
1. Application starts with default settings:
   - Status Filter: "All"
   - Sort By: "Date Added (Newest)"
2. User can change these settings via the Filter & Search panel

### Subsequent Launches
1. Application loads and applies previously saved settings
2. User sees jobs filtered and sorted exactly as they left them
3. Any changes to filter/sort are immediately persisted

### Example Scenarios

**Scenario 1: Focusing on Active Applications**
1. User sets Status Filter to "Applied"
2. Sets Sort By to "Date Applied (Newest)"
3. Works with these settings
4. Closes application
5. Next time: Application opens with "Applied" filter and "Date Applied (Newest)" sort already applied

**Scenario 2: Reviewing Interviews**
1. User sets Status Filter to "Interviewed"
2. Sets Sort By to "Company Name (A-Z)"
3. Reviews upcoming interviews
4. Closes application
5. Next time: Application remembers these settings

## Benefits

✅ **Seamless Experience** - Users don't need to reconfigure their view every time they start the application
✅ **Increased Productivity** - No time wasted resetting filters and sort options
✅ **Context Preservation** - Users can pick up exactly where they left off
✅ **Reduced Friction** - The application adapts to user preferences automatically
✅ **Silent Persistence** - Settings save automatically without user intervention

## Technical Features

### Auto-Save Mechanism
- Settings are saved immediately when changed
- Uses fire-and-forget async pattern (`_ = SavePreferencesAsync()`)
- Doesn't block UI or interrupt user workflow
- Silent failure handling - won't crash if save fails

### Initialization Synchronization
- Preferences are loaded before auto-loading last project
- Ensures consistent state on startup
- Works with the existing race-condition-free preference loading system

### Backward Compatibility
- Default values ensure old preference files work
- No migration needed for existing users
- Gracefully handles missing properties in JSON

## Testing Checklist

To verify this feature works correctly:

1. **Initial State Test**
   - [ ] Start application fresh
   - [ ] Verify Status Filter shows "All"
   - [ ] Verify Sort By shows "Date Added (Newest)"

2. **Save Test**
   - [ ] Change Status Filter to "Applied"
   - [ ] Change Sort By to "Company Name (A-Z)"
   - [ ] Close application
   - [ ] Reopen application
   - [ ] Verify both settings were restored

3. **Multiple Changes Test**
   - [ ] Change Status Filter multiple times
   - [ ] Verify each change persists across restarts
   - [ ] Change Sort By multiple times
   - [ ] Verify each change persists across restarts

4. **Project Loading Test**
   - [ ] Set specific filter and sort options
   - [ ] Close and reopen application (auto-loads last project)
   - [ ] Verify filter and sort are restored correctly
   - [ ] Verify filtered jobs are displayed correctly

5. **Preferences Dialog Test**
   - [ ] Open Preferences → General
   - [ ] Change Default Sort By
   - [ ] Save preferences
   - [ ] Verify sort option is applied immediately
   - [ ] Restart application to verify persistence

## Files Modified

- `Models/UserPreferences.cs` - Added `LastStatusFilter` property
- `ViewModels/MainViewModel.cs` - Added auto-save to properties and `SavePreferencesAsync()` method
- `MainWindow.xaml.cs` - Enhanced `LoadPreferencesAsync()` to restore status filter

## Future Enhancements

Potential improvements for future versions:
- Remember search text (FilterText) across sessions
- Save column widths and window size/position
- Remember expanded/collapsed state of detail sections
- Profile-based filter/sort presets
- Quick-switch between saved view configurations

---

**Version**: 2.1
**Date**: 2024
**Feature Type**: Quality of Life Enhancement
**Impact**: User Experience Improvement
