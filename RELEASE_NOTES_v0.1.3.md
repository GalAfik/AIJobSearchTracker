# Release Notes - Version 0.1.3

**Release Date:** January 2026  
**Status:** ✅ Ready for Deployment

---

## 🎯 Overview

Version 0.1.3 introduces important data protection features to prevent accidental data loss and streamline workflow efficiency. This release focuses on user experience improvements for project management and file operations.

---

## ✨ New Features

### 1. **Unsaved Changes Warning Dialog**

The application now tracks unsaved changes and warns you before closing.

**How it works:**
- When you try to close the application with unsaved changes, a warning dialog appears
- You can choose to:
  - **Yes**: Save changes and close
  - **No**: Discard changes and close
  - **Cancel**: Stay in the application

**What triggers unsaved changes:**
- Adding a new job (manually or from URL)
- Editing an existing job
- Deleting a job
- Importing jobs from CSV

**What clears unsaved changes:**
- Saving the project (Save or Save As)
- Loading a different project
- Creating a new project

### 2. **Auto-Save After Import Preference**

New optional feature to automatically save your project after importing CSV files.

**How to enable:**
1. Open **File → Preferences**
2. Go to **Appearance** tab
3. Check **"Automatically save project after importing files"**
4. Click **Save**

**Behavior:**
- Only works for projects that have already been saved (not new unsaved projects)
- After importing a CSV file, the project automatically saves
- Shows confirmation message after auto-save
- If auto-save fails, shows error and prompts you to save manually

---

## ⚡ Improvements

### User Experience
- **Better Data Protection**: Prevents accidental loss of work when closing the application
- **Streamlined Workflow**: Auto-save option eliminates manual save step after imports
- **Clear Feedback**: Visual and textual indicators when project has unsaved changes

### Change Tracking
- Tracks changes across all job operations (add, edit, delete, import)
- Automatically resets when saving, loading, or creating projects
- Intelligent handling of save vs. save-as scenarios

---

## 🔧 Technical Changes

### Models
- **`UserPreferences.cs`**: Added `AutoSaveAfterImport` property (default: `false`)

### ViewModels
- **`MainViewModel.cs`**:
  - Added `HasUnsavedChanges` property with change notification
  - Updated all job operation methods to set `HasUnsavedChanges = true`
  - Updated save methods to reset `HasUnsavedChanges = false`
  - Changed `ImportCsv()` to `async` to support auto-save logic
  - Added `CheckUnsavedChanges()` method for window closing confirmation
  - Implemented auto-save logic in `ImportCsv()` when preference is enabled

### Views
- **`MainWindow.xaml.cs`**:
  - Added `Closing` event handler
  - Calls `CheckUnsavedChanges()` before window closes
  - Cancels close operation if user chooses to cancel

- **`PreferencesDialog.xaml`**:
  - Added "File Operations" section in Appearance tab
  - Added auto-save checkbox with description

- **`PreferencesDialog.xaml.cs`**:
  - Load and save `AutoSaveAfterImport` preference

- **`ChangelogWindow.xaml`**:
  - Added v0.1.3 release notes
  - Moved "Latest" badge to v0.1.3

- **`MainWindow.xaml`**:
  - Updated footer version from v0.1.2 to v0.1.3

---

## 🧪 Testing Checklist

### Unsaved Changes Warning

- [ ] Add a job and try to close → Should show warning
- [ ] Edit a job and try to close → Should show warning
- [ ] Delete a job and try to close → Should show warning
- [ ] Import CSV and try to close → Should show warning
- [ ] Click "Yes" in warning → Should save and close
- [ ] Click "No" in warning → Should close without saving
- [ ] Click "Cancel" in warning → Should stay in application
- [ ] Save project and try to close → Should NOT show warning
- [ ] Load project without changes and try to close → Should NOT show warning
- [ ] Create new project and try to close → Should NOT show warning

### Auto-Save After Import

- [ ] Enable auto-save preference
- [ ] Open an existing saved project
- [ ] Import a CSV file → Should auto-save and show confirmation
- [ ] Verify jobs are saved correctly
- [ ] Disable auto-save preference
- [ ] Import another CSV → Should NOT auto-save
- [ ] Create new unsaved project
- [ ] Import CSV → Should NOT auto-save (project not saved yet)
- [ ] Test with invalid file path → Should show error and not crash

### Edge Cases

- [ ] Multiple edits without saving → Should accumulate changes
- [ ] Save As with unsaved changes → Should reset unsaved flag
- [ ] Import into new project (not saved) → Should mark as unsaved
- [ ] Import into existing project → Should mark as unsaved (if auto-save disabled)
- [ ] Close during save operation → Should handle gracefully

---

## 📋 Known Limitations

1. **Auto-save only works for already-saved projects**: If you import into a new project that hasn't been saved yet, auto-save won't trigger. This is intentional to prevent unwanted saves.

2. **SaveProjectAs cancellation**: If you click "Yes" to save before closing but then cancel the SaveProjectAs dialog, the close operation is cancelled. This is correct behavior but may seem unexpected.

---

## 🚀 Deployment Steps

1. **Build the application:**
   ```powershell
   dotnet build -c Release
   ```

2. **Publish for distribution:**
   ```powershell
   dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
   ```

3. **Copy distribution files:**
   - Copy from: `bin\Release\net10.0-windows\win-x64\publish\`
   - Include: All `.exe`, `.dll`, `.json`, and `.pdb` files
   - Add documentation: `README.md`, `LICENSE`, `GETTING_STARTED.txt`, `AI_FEATURE_GUIDE.md`
   - Add samples: `Samples/` folder

4. **Create release package:**
   ```powershell
   Compress-Archive -Path bin\Release\net10.0-windows\win-x64\publish\* -DestinationPath JobSearchTracker-v0.1.3-Windows-x64.zip
   ```

5. **Upload to GitHub:**
   - Create new release: `v0.1.3`
   - Tag: `v0.1.3`
   - Title: "Version 0.1.3 - Unsaved Changes Protection & Auto-Save"
   - Upload ZIP file
   - Paste release notes

---

## 📝 Upgrade Notes

### For Users

If you're upgrading from v0.1.2:
- Your existing preferences will be preserved
- The new auto-save feature is **disabled by default**
- You can enable it in Preferences if desired
- Unsaved changes warning is **always enabled** (cannot be disabled)

### For Developers

New properties and methods:
- `UserPreferences.AutoSaveAfterImport` (bool)
- `MainViewModel.HasUnsavedChanges` (bool)
- `MainViewModel.CheckUnsavedChanges()` (returns bool)
- `MainWindow.Closing` event handler

---

## 🎉 What's Next?

Future improvements being considered:
- Visual indicator in title bar when project has unsaved changes (e.g., asterisk)
- Auto-save on timer (optional)
- Undo/redo functionality
- Project backup/restore

---

## 📞 Support

- **GitHub Issues**: https://github.com/GalAfik/AIJobSearchTracker/issues
- **Email**: [Your support email]
- **Documentation**: See `GETTING_STARTED.txt` and `README.md`

---

**Happy Job Hunting! 🎯**

*Remember: The right opportunity is out there. Keep applying, keep improving, and don't give up!*
