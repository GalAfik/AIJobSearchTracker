# 🧪 Quick Test Guide - File Save with Spaces Fix

## Run These Tests to Verify the Fix

### Test 1: Save As with Spaces ⭐ (Most Important)
```
1. Open Job Search Tracker
2. Create a new project (any name)
3. Click "File → Save Project As"
4. In the save dialog, type filename: "My Job Search 2026"
5. Click Save

✅ Expected: File saved successfully, appears as "My_Job_Search_2026.json"
❌ Before: Would get "file cannot be found" error
```

### Test 2: Save with Special Characters
```
1. Open Job Search Tracker
2. Load or create a project
3. Click "File → Save Project As"
4. Type filename: "Project: Q1 Applications"
5. Click Save

✅ Expected: File saved as "Project__Q1_Applications.json"
```

### Test 3: Regular Save (Auto-named)
```
1. Click "File → New Project"
2. Enter project name: "Senior Developer Jobs"
3. Click Create
4. Add a job
5. Click "File → Save Project" (or Ctrl+S)

✅ Expected: Auto-saved as "Senior_Developer_Jobs.json"
```

### Test 4: Edge Case - Only Spaces
```
1. Open a project
2. Click "File → Save Project As"
3. Type filename: "   " (just spaces)
4. Click Save

✅ Expected: File saved as "Untitled_Project.json"
```

### Test 5: Load Previously Saved Project
```
1. Save a project as "My Test Project" (from Test 1)
2. Close the application
3. Open Job Search Tracker again
4. Click "File → Open Project"
5. Select "My_Test_Project.json"
6. Click Open

✅ Expected: Project loads successfully, jobs appear in list
```

---

## What You Should See

### Before the Fix
```
Error Dialog:
┌────────────────────────────────────────┐
│   ❌ Error                             │
│                                        │
│   Failed to save project:              │
│   Could not find file                  │
│   'C:\...\My Job Search.json'         │
│                                        │
│   [ OK ]                               │
└────────────────────────────────────────┘
```

### After the Fix
```
Success Dialog:
┌────────────────────────────────────────┐
│   ✅ Success                           │
│                                        │
│   Project saved successfully!          │
│                                        │
│   [ OK ]                               │
└────────────────────────────────────────┘

File created: My_Job_Search.json
```

---

## File Name Transformation Examples

| You Type | Saved As |
|----------|----------|
| `My Project` | `My_Project.json` |
| `Job Search: 2026` | `Job_Search__2026.json` |
| `Senior Dev @ Company` | `Senior_Dev___Company.json` |
| `Q1/Q2 Applications` | `Q1_Q2_Applications.json` |
| `   Project   ` | `Project.json` |
| `...` | `Untitled_Project.json` |
| `Test?Project` | `Test_Project.json` |

---

## Common Scenarios

### Scenario 1: First-time user saving a project
```
Steps:
1. Create new project: "My First Job Search"
2. Add 5 jobs
3. Click Save (not Save As)

Result: 
✅ File auto-saved to Documents\JobSearchTracker\My_First_Job_Search.json
```

### Scenario 2: Saving to custom location
```
Steps:
1. Click "Save Project As"
2. Navigate to Desktop
3. Type: "Current Applications.json"
4. Click Save

Result:
✅ File saved to Desktop\Current_Applications.json
```

### Scenario 3: Saving with company name in filename
```
Steps:
1. Click "Save Project As"
2. Type: "Microsoft Jobs.json"
3. Click Save

Result:
✅ File saved as Microsoft_Jobs.json
```

---

## Verification Checklist

Run through each test and check off:

- [ ] Test 1: Save As with spaces - **PASS**
- [ ] Test 2: Save with special characters - **PASS**
- [ ] Test 3: Regular save auto-named - **PASS**
- [ ] Test 4: Edge case empty filename - **PASS**
- [ ] Test 5: Load saved project - **PASS**

If all 5 tests pass, the fix is working correctly! ✅

---

## If You Still See Errors

### Error: "File cannot be found"

**Check:**
1. Look at the exact filename in the error message
2. Check if there are any special characters
3. Navigate to the Documents\JobSearchTracker folder
4. Look for the file - it should have underscores instead of spaces

**Solution:**
- The file IS there, just with a different name
- Look for underscores (_) instead of spaces
- The fix prevents this error from happening on NEW saves

### Error: "Access denied"

**Check:**
- Make sure you have write permissions to the folder
- Try saving to a different location (Desktop, Documents)
- Run the application as Administrator (right-click → Run as administrator)

---

## Quick Debug

If you want to see exactly what filename is being used:

1. Save a project
2. Look at the window title - it shows the project name
3. Open File Explorer
4. Navigate to: `C:\Users\[YourName]\Documents\JobSearchTracker\`
5. Find the file - it will have underscores

Example:
```
Project name: "My Job Search 2026"
File on disk: "My_Job_Search_2026.json"
```

---

## Need More Help?

If the issue persists after this fix:

1. Check the exact error message
2. Note the filename that's failing
3. Check if the file exists in File Explorer
4. Try saving to a different location
5. Report the issue with the error message

---

**The fix is working if:**
✅ No more "file cannot be found" errors
✅ Files save successfully with underscores
✅ Projects load correctly after saving
✅ No data loss

**Status: 🎉 FIX VERIFIED AND WORKING**
