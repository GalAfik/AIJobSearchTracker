# Version 0.1.5 Release Summary

## Release Information
- **Version**: 0.1.5
- **Release Date**: March 30, 2026
- **Previous Version**: 0.1.4
- **Build Status**: ✅ Successful

## New Features Implemented

### 1. Software Update Checker
**Status**: ✅ Complete

Automatically checks for new versions on startup by scraping www.galafik.com.

**Implementation:**
- `Services/UpdateService.cs` - New service for update checking
- Website scraping with regex pattern matching
- Semantic version comparison using System.Version
- 10-second timeout with silent failure
- Non-intrusive footer notification (bottom-left)
- Clickable notification opens download page

**User Control:**
- Preference toggle: "Check for software updates on startup"
- Location: Preferences → Appearance
- Default: Enabled (checks for updates)
- Can be disabled without affecting other functionality

**Files Modified:**
- Services/UpdateService.cs (NEW)
- Models/UserPreferences.cs (added CheckForUpdates property)
- Views/PreferencesDialog.xaml (added checkbox)
- Views/PreferencesDialog.xaml.cs (load/save logic)
- MainWindow.xaml (footer notification UI)
- MainWindow.xaml.cs (update check methods)

**Documentation:**
- SOFTWARE_UPDATE_CHECK_FEATURE.md

### 2. Suppress Warning Messages
**Status**: ✅ Complete

Allows users to suppress informational success messages for a cleaner workflow.

**Implementation:**
- `SuppressWarnings` property in UserPreferences
- Conditional checks before showing success messages
- Preserves critical errors and confirmations
- Default: Disabled (warnings shown for safety)

**Affected Messages:**
1. Job save success ("Job changes saved successfully!")
2. Job reload ("Job data reloaded.")
3. Project save ("Project saved successfully!")
4. Project save-as ("Project saved successfully! Saved to: [path]")
5. CSV import success ("Successfully imported N jobs...")
6. Auto-save success ("Project auto-saved successfully!")
7. Excel export success ("Data exported successfully!")
8. CSV export success ("Data exported to CSV successfully!")

**Messages NOT Suppressed:**
- All error messages (failures, exceptions)
- Delete confirmations (data loss prevention)
- Validation errors (required fields, invalid input)
- Unsaved changes warnings

**User Control:**
- Preference toggle: "Suppress warning messages"
- Location: Preferences → Appearance
- Default: Disabled (warnings shown)
- Clear description of what is/isn't suppressed

**Files Modified:**
- Models/UserPreferences.cs (added SuppressWarnings property)
- Views/PreferencesDialog.xaml (added checkbox)
- Views/PreferencesDialog.xaml.cs (load/save logic)
- MainWindow.xaml.cs (2 messages)
- ViewModels/MainViewModel.cs (6 messages)

**Documentation:**
- SUPPRESS_WARNINGS_FEATURE.md

## Version Number Updates

Updated version references throughout the application:

1. **MainWindow.xaml** - Footer copyright line (v0.1.5)
2. **MainWindow.xaml.cs** - About dialog (Version 0.1.5)
3. **Services/UpdateService.cs** - CurrentVersion constant ("0.1.5")
4. **Views/ChangelogWindow.xaml** - Added v0.1.5 entry, moved "Latest" badge

## Changelog Updates

Added comprehensive v0.1.5 entry to ChangelogWindow.xaml:
- New Features section (Update Checker, Suppress Warnings)
- Improvements section (4 items)
- Technical Changes section (4 items)
- Release date: March 30, 2026
- "Latest" badge moved from 0.1.4 to 0.1.5

## Build Verification

✅ **All Builds Successful**

Build history:
1. Initial build after UserPreferences updates - SUCCESS
2. Build after UI implementation - SUCCESS
3. Build after message suppression logic - SUCCESS
4. Final build after version updates - SUCCESS

No compilation errors or warnings.

## Testing Checklist

### Update Checker Testing
- [ ] Update check runs on startup (when enabled)
- [ ] Update notification appears when newer version available
- [ ] Clicking notification opens correct website
- [ ] Update check can be disabled in preferences
- [ ] Network failures don't cause errors or delays

### Suppress Warnings Testing
- [ ] Warnings shown by default (preference disabled)
- [ ] All 8 success messages suppressible when enabled
- [ ] Error messages still show when suppressed
- [ ] Delete confirmations still show when suppressed
- [ ] Validation errors still show when suppressed
- [ ] Preference persists across restarts

### Version Display Testing
- [ ] Footer shows v0.1.5
- [ ] About dialog shows Version 0.1.5
- [ ] Changelog shows 0.1.5 as Latest
- [ ] Changelog entry complete and accurate

## User Benefits

### For All Users
- Stay informed about new versions and features
- Non-intrusive notification system
- Control over application behavior

### For Power Users
- Cleaner workflow without confirmation dialogs
- Faster batch operations (export, import, save)
- Professional application feel

### For New Users
- Guided experience with default settings (warnings on, updates on)
- Safety-first approach (critical messages always show)
- Clear preference descriptions

## Technical Highlights

### Update Service
- HTTP client with timeout
- Regex pattern flexibility (handles naming variations)
- Semantic version parsing with System.Version
- Fire-and-forget async pattern
- Silent failure philosophy

### Suppress Warnings
- Zero performance impact (dialog not created when suppressed)
- Consistent implementation pattern
- Conservative approach (when in doubt, don't suppress)
- Centralized preference flag

### Code Quality
- Comprehensive XML documentation
- Follows existing patterns and conventions
- No breaking changes to existing functionality
- Clean separation of concerns

## Files Summary

### New Files
- Services/UpdateService.cs (106 lines)
- SOFTWARE_UPDATE_CHECK_FEATURE.md (312 lines)
- SUPPRESS_WARNINGS_FEATURE.md (353 lines)

### Modified Files
- Models/UserPreferences.cs (2 new properties)
- Views/PreferencesDialog.xaml (2 new checkboxes)
- Views/PreferencesDialog.xaml.cs (load/save for 2 preferences)
- MainWindow.xaml (footer notification UI, version update)
- MainWindow.xaml.cs (update check methods, 2 message suppressions, version update)
- ViewModels/MainViewModel.cs (6 message suppressions)
- Views/ChangelogWindow.xaml (new v0.1.5 entry)
- Services/UpdateService.cs (version constant update)

### Documentation Files
- SOFTWARE_UPDATE_CHECK_FEATURE.md (complete feature documentation)
- SUPPRESS_WARNINGS_FEATURE.md (complete feature documentation)

## Next Steps

1. **User Testing**: Test both features in real-world scenarios
2. **Website Update**: Publish v0.1.5 to www.galafik.com
3. **Release Notes**: Create user-facing release notes
4. **Distribution**: Package and distribute new version
5. **Monitor**: Watch for user feedback and issues

## Version History Context

- **v0.1.4** (March 29, 2026): Auto-open last file, color-coded status badges, persistent filters
- **v0.1.5** (March 30, 2026): Update checker, suppress warnings
- **Next**: Consider additional user customization features

## Success Metrics

✅ All planned features implemented
✅ All builds successful
✅ Comprehensive documentation created
✅ Version numbers updated consistently
✅ Changelog entries complete
✅ No breaking changes
✅ Follows existing patterns and conventions

## Notes

- Both features respect user preferences and provide control
- Default settings prioritize safety and user information
- Silent failure approach prevents disruption
- Non-modal notifications preferred over modal dialogs
- Conservative suppression (errors and confirmations always show)

---

**Implementation Complete**: March 30, 2026
**Ready for Testing**: Yes
**Ready for Release**: Pending testing
