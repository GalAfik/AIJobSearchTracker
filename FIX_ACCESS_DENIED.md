# 🔧 IMMEDIATE FIX - Access Denied Error

## Quick Solution

If you're getting "Access Denied" when saving to an **existing** file:

### ✅ Most Common Cause: File is Read-Only

**Quick Fix:**
1. Open File Explorer
2. Navigate to the file (usually in `Documents\JobSearchTracker\`)
3. **Right-click** the `.json` file
4. Select **Properties**
5. **Uncheck** the "Read-only" checkbox at the bottom
6. Click **OK**
7. **Try saving again**

---

### 🔍 Other Causes & Solutions

#### Cause 1: File is Open Elsewhere
**Symptoms:** Error says "file is locked by another program"

**Check if the file is open in:**
- ❌ Excel (right-click → Open With → Excel)
- ❌ Notepad or VS Code
- ❌ **Another instance of Job Search Tracker** ← Most common!
- ❌ Windows File Explorer preview pane

**Solution:**
1. Close **ALL** instances of Job Search Tracker
2. Close Excel, Notepad, etc.
3. Open Task Manager (Ctrl+Shift+Esc)
4. Look for "JobSearchTracker.exe" - End all instances
5. Restart the application
6. Try saving again

#### Cause 2: Antivirus Blocking
**Symptoms:** Intermittent errors, works sometimes but not others

**Solution:**
1. Temporarily disable antivirus
2. Try saving
3. If it works, add exception for `JobSearchTracker` folder
4. Re-enable antivirus

#### Cause 3: File Permissions
**Symptoms:** Always fails on the same file/folder

**Solution:**
1. Right-click the **folder** (not the file)
2. Properties → Security tab
3. Click **Edit**
4. Select your username
5. Check **Full Control**
6. Click **OK**

---

## 🧪 Immediate Test

**Try this RIGHT NOW:**

### Step 1: Check if File is Read-Only
```powershell
# Open PowerShell in the project directory
cd "E:\Code\JobSearchTracker"

# Check file attributes
Get-ChildItem -Path "C:\Users\$env:USERNAME\Documents\JobSearchTracker\*.json" | Select-Object Name, IsReadOnly, Attributes
```

If `IsReadOnly` is `True`, run:
```powershell
# Remove read-only attribute from ALL json files
Get-ChildItem -Path "C:\Users\$env:USERNAME\Documents\JobSearchTracker\*.json" | ForEach-Object { $_.IsReadOnly = $false }
```

### Step 2: Check if File is Locked
**Close everything** that might have the file open:
1. Close **ALL** Excel windows
2. Close **ALL** text editors (Notepad, VS Code, etc.)
3. Close **ALL** instances of Job Search Tracker
4. Wait 10 seconds
5. Open Job Search Tracker fresh
6. Try saving

### Step 3: Use Save As Workaround
If Save still fails:
1. Click **File → Save As**
2. Navigate to **Desktop**
3. Type a new name: `test_project`
4. Click **Save**

If this works, the issue is with the **original file location**, not the app.

---

## 🎯 What I Fixed in the Code

The application now:

1. ✅ **Checks if file is read-only** before saving
2. ✅ **Automatically removes read-only flag** if possible
3. ✅ **Checks if file is locked** by another program
4. ✅ **Shows specific error messages** for each scenario:
   - Read-only file with instructions
   - Locked file with likely programs
   - Permission issues with solutions

### New Error Messages You'll See

#### If File is Read-Only:
```
Access denied to file: C:\...\project.json

File exists: Yes
Read-only: True
Attributes: ReadOnly, Archive

Solutions:
1. Close any programs that have this file open
2. Check file properties and remove read-only attribute
3. Try running as Administrator
4. Use 'Save As' to save to a different location
```

#### If File is Locked:
```
File is locked by another program: C:\...\project.json

Possible causes:
- File is open in Excel, Notepad, or another editor
- Another instance of Job Search Tracker has it open
- Antivirus is scanning the file

Solution:
- Close all programs that might have this file open
- Wait a few seconds and try again
- Use 'Save As' to save to a different location
```

---

## 🔥 Emergency Workaround

If nothing works, use this **guaranteed method**:

### Method 1: Save to a New File
```
1. File → Save As
2. Change the filename slightly: "MyProject_v2.json"
3. Save
4. Delete the old file later
```

### Method 2: Save to a Different Location
```
1. File → Save As
2. Navigate to: Desktop
3. Save there
4. Later, move to Documents when file isn't locked
```

### Method 3: Restart Computer
```
Sometimes Windows locks files. A restart clears everything.
```

---

## 📊 Debugging Steps

Run these checks and report back:

### Check 1: File Attributes
```powershell
# In PowerShell
$file = "C:\Users\$env:USERNAME\Documents\JobSearchTracker\YOUR_FILE.json"
Get-Item $file | Select-Object FullName, IsReadOnly, Attributes, LastWriteTime, Length
```

Expected output:
```
FullName      : C:\Users\...\project.json
IsReadOnly    : False  ← Should be False!
Attributes    : Archive
LastWriteTime : 1/15/2026 10:30:00 AM
Length        : 5432
```

### Check 2: File Locks
```powershell
# Check if any process has the file open
# Install Process Explorer from Microsoft (optional)
# OR just close everything and try again
```

### Check 3: Permissions
```
1. Right-click the file
2. Properties → Security
3. Check if your username is listed
4. Check if you have "Full Control"
```

---

## 💡 Most Likely Scenario

Based on your error, **the most common cause is**:

🎯 **You have the file open in Excel or have opened it before**

When you double-click a `.json` file, Windows might open it in Excel (or default JSON viewer), which **locks** the file.

**Solution:**
1. Open Task Manager (Ctrl+Shift+Esc)
2. Look for **Excel** or **EXCEL.EXE**
3. **End Task** on all Excel instances
4. Try saving again

---

## 📝 Report Template

If still failing, provide this info:

```
1. Full error message: [paste entire error]

2. File path: [what file are you trying to save?]

3. File exists? [Yes/No]

4. Read-only? [Check properties]

5. What programs do you have open? 
   □ Excel
   □ Notepad
   □ VS Code
   □ Other text editor
   □ Multiple instances of Job Search Tracker

6. Can you save to Desktop instead? [Yes/No]

7. Are you running as Administrator? [Yes/No]
```

---

## ✅ Success Indicators

You'll know it's fixed when:
- ✅ Save completes without errors
- ✅ File shows updated LastWriteTime in File Explorer
- ✅ Opening the file shows your latest changes

---

**TL;DR:** 
1. **Close Excel** if you opened the JSON file there
2. **Remove read-only attribute** from the file
3. **Close all other instances** of Job Search Tracker
4. **Try Save As** to Desktop if Save still fails

**This should fix 90% of access denied errors!** 🎉
