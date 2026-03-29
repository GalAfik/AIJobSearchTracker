# 🔍 Diagnostic Guide - "Could Not Find File" Error

## Purpose

This enhanced error handling will help us identify the **exact** cause of your "could not find file" error when saving projects.

---

## What Changed

### 1. Enhanced Error Messages

**Before:**
```
Failed to save project: could not find file
```

**Now:**
```
Failed to save project: [detailed message]

Path: C:\Users\...\My_Project.json
Attempted path: C:\Users\...\My Project.json
Exception: DirectoryNotFoundException
Stack trace: [full trace]
```

### 2. Better Path Validation

The save process now:
- ✅ Validates the filename isn't empty
- ✅ Defaults to `.json` extension if missing
- ✅ Uses default directory if no directory specified
- ✅ Validates the final path before saving
- ✅ Provides specific error messages for each failure type

### 3. Specific Error Types

The error message will now tell you **exactly** what went wrong:

| Error Type | Meaning | Solution |
|------------|---------|----------|
| `DirectoryNotFoundException` | The folder doesn't exist | Check the folder path |
| `UnauthorizedAccessException` | No permission to write | Run as administrator or choose different folder |
| `PathTooLongException` | Path exceeds Windows limit | Use shorter filename or save to root folder |
| `IOException` | File is locked or disk full | Close other programs, check disk space |
| `ArgumentException` | Invalid path or filename | Check for invalid characters |

---

## How to Use This Diagnostic

### Step 1: Try to Save

1. Open Job Search Tracker
2. Create or load a project
3. Try to save (either Save or Save As)
4. **If error occurs**, READ THE ENTIRE ERROR MESSAGE

### Step 2: Identify the Error Type

Look for these specific phrases in the error message:

#### Error Type 1: "Directory not found"
```
Failed to save project: Directory not found: C:\Users\...\SomeFolder

Exception: DirectoryNotFoundException
```

**Cause:** The folder you're trying to save to doesn't exist  
**Solution:** 
- Use "Save As" and navigate to an existing folder
- OR save to Documents (default location)

#### Error Type 2: "Access denied"
```
Failed to save project: Access denied to path: C:\...

Exception: UnauthorizedAccessException
```

**Cause:** You don't have permission to write to that folder  
**Solution:**
- Run Visual Studio or the app as Administrator
- OR choose a different save location (Documents, Desktop, etc.)
- OR check folder permissions

#### Error Type 3: "Path too long"
```
Failed to save project: Path too long: C:\Very\Long\Path\...

Exception: PathTooLongException
```

**Cause:** Windows has a 260-character path limit  
**Solution:**
- Use a shorter project name
- Save to a folder closer to C:\ (like C:\Projects\)
- Enable long path support in Windows

#### Error Type 4: "Invalid file path"
```
Failed to save project: Invalid file path: '' - filename is empty

Exception: ArgumentException
```

**Cause:** The filename is empty or contains only invalid characters  
**Solution:**
- Type a valid filename in the Save As dialog
- Use only letters, numbers, spaces, hyphens, underscores

#### Error Type 5: "IO error"
```
Failed to save project: IO error saving to: C:\...

Exception: IOException
```

**Cause:** File is locked, disk is full, or drive is disconnected  
**Solution:**
- Close other programs that might be using the file
- Check disk space (need at least a few MB free)
- Make sure external drives are connected

---

## Quick Tests

Run these tests and note the **exact error message** for each:

### Test 1: Save to Default Location
```
1. Create new project: "Test"
2. Add a job
3. Click "File → Save Project" (not Save As)
4. Note result:
   □ Success - shows path where saved
   □ Error - copy the ENTIRE error message
```

### Test 2: Save As to Documents
```
1. Load a project
2. Click "File → Save As"
3. Navigate to Documents folder
4. Type filename: "TestProject"
5. Click Save
6. Note result:
   □ Success - shows saved path
   □ Error - copy the ENTIRE error message
```

### Test 3: Save As to Desktop
```
1. Load a project
2. Click "File → Save As"
3. Navigate to Desktop
4. Type filename: "TestDesktop"
5. Click Save
6. Note result:
   □ Success
   □ Error - copy message
```

### Test 4: Save with Spaces
```
1. Load a project
2. Click "File → Save As"
3. Type filename: "My Test Project"
4. Click Save
5. Note result:
   □ Success - filename should be "My_Test_Project.json"
   □ Error - copy message
```

---

## Common Scenarios & Solutions

### Scenario 1: First-time Save After Creating Project
```
Steps:
1. Click "New Project"
2. Name: "My Job Search"
3. Create
4. Add a job
5. Click "Save" (not Save As)

Expected: Auto-saves to Documents\JobSearchTracker\My_Job_Search.json
If Error: Check error type above
```

### Scenario 2: Saving Existing Project
```
Steps:
1. Load existing project
2. Make changes
3. Click "Save"

Expected: Overwrites existing file
If Error: File might be open elsewhere or locked
```

### Scenario 3: Save As to New Location
```
Steps:
1. Load project
2. Click "Save As"
3. Navigate to new folder
4. Type new name
5. Save

Expected: Creates new file in new location
If Error: Check folder exists and you have permissions
```

---

## What to Report

When you get the error, please provide:

1. **Exact error message** (copy the entire thing)
2. **Which test scenario** were you trying?
3. **Project name** you're trying to save
4. **Where you're trying to save** (Documents? Desktop? Network drive?)
5. **Full path** shown in the error (if visible)

Example report:
```
Error: "Failed to save project: Directory not found: E:\Projects\Jobs"
Scenario: Test 2 - Save As to Documents
Project name: "My Applications"
Attempted path: E:\Projects\Jobs\My_Applications.json
Note: The E:\Projects folder exists, but E:\Projects\Jobs doesn't
```

---

## Immediate Workaround

If you're getting errors repeatedly, try this guaranteed method:

### Guaranteed Save Method
```
1. Open Windows Explorer
2. Navigate to: C:\Users\[YourName]\Documents\
3. Create a folder: "JobSearchTracker" (if it doesn't exist)
4. In the app, click "File → Save As"
5. Navigate to C:\Users\[YourName]\Documents\JobSearchTracker\
6. Type filename: "test" (just 4 letters, no spaces)
7. Click Save

This should ALWAYS work. If this fails, there's a deeper Windows permissions issue.
```

---

## Next Steps

After you see the new error message:

1. **Copy the exact error message**
2. **Identify the error type** from the table above
3. **Try the suggested solution**
4. **If still failing**, report back with:
   - Full error message
   - What you were trying to do
   - Where you were trying to save

The enhanced error messages will help us pinpoint exactly what's going wrong!

---

**Status:** Enhanced diagnostics deployed ✅  
**Build:** Successful  
**Ready:** To help identify the root cause of your save issue
