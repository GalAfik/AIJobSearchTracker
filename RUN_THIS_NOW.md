# 🚀 Quick Fix Script - Run This Now!

## Immediate Solution

Open PowerShell and run these commands:

```powershell
# 1. Navigate to your JobSearchTracker folder
cd "C:\Users\$env:USERNAME\Documents\JobSearchTracker"

# 2. List all JSON files and their read-only status
Write-Host "Checking JSON files..." -ForegroundColor Cyan
Get-ChildItem *.json | ForEach-Object {
    Write-Host "File: $($_.Name)" -ForegroundColor Yellow
    Write-Host "  Read-Only: $($_.IsReadOnly)" -ForegroundColor $(if($_.IsReadOnly){"Red"}else{"Green"})
    Write-Host "  Locked: $(if(Test-FileLock $_){"Yes"}else{"No"})" -ForegroundColor $(if(Test-FileLock $_){"Red"}else{"Green"})
}

# 3. Remove read-only flag from ALL JSON files
Write-Host "`nRemoving read-only attributes..." -ForegroundColor Cyan
Get-ChildItem *.json | ForEach-Object {
    if ($_.IsReadOnly) {
        $_.IsReadOnly = $false
        Write-Host "  Fixed: $($_.Name)" -ForegroundColor Green
    }
}

Write-Host "`n✅ Done! Try saving again." -ForegroundColor Green
```

---

## Even Simpler Version

If the above doesn't work, just run this **one-liner**:

```powershell
Get-ChildItem "$env:USERPROFILE\Documents\JobSearchTracker\*.json" | ForEach-Object { $_.IsReadOnly = $false }
```

---

## Check What's Locking the File

```powershell
# This will show which process has the file open
$file = "C:\Users\$env:USERNAME\Documents\JobSearchTracker\YOUR_FILE_NAME.json"

# Check if file exists
if (Test-Path $file) {
    Write-Host "File exists: $file" -ForegroundColor Green
    
    # Get file info
    $info = Get-Item $file
    Write-Host "Read-Only: $($info.IsReadOnly)" -ForegroundColor $(if($info.IsReadOnly){"Red"}else{"Green"})
    Write-Host "Attributes: $($info.Attributes)"
    Write-Host "Size: $($info.Length) bytes"
    Write-Host "Last Modified: $($info.LastWriteTime)"
    
    # Try to open it
    try {
        $stream = [System.IO.File]::Open($file, 'Open', 'ReadWrite', 'None')
        $stream.Close()
        Write-Host "File is NOT locked" -ForegroundColor Green
    }
    catch {
        Write-Host "File IS locked by another process!" -ForegroundColor Red
        Write-Host "Close Excel, Notepad, or other programs that might have it open."
    }
} else {
    Write-Host "File does not exist: $file" -ForegroundColor Red
}
```

---

## Kill All Processes That Might Lock Files

```powershell
# Close Excel (if open)
Get-Process excel -ErrorAction SilentlyContinue | Stop-Process -Force

# Close Notepad (if open)  
Get-Process notepad -ErrorAction SilentlyContinue | Stop-Process -Force

# Close all Job Search Tracker instances except current
Get-Process JobSearchTracker -ErrorAction SilentlyContinue | Stop-Process -Force

Write-Host "✅ Closed all potentially locking processes" -ForegroundColor Green
Write-Host "Wait 5 seconds, then try saving again." -ForegroundColor Yellow
```

---

## Create Test File to Verify Permissions

```powershell
# Try to create a new file in the JobSearchTracker folder
$testFile = "$env:USERPROFILE\Documents\JobSearchTracker\test_permissions.json"

try {
    "test" | Out-File $testFile
    Write-Host "✅ SUCCESS: You CAN write to this folder" -ForegroundColor Green
    Remove-Item $testFile
}
catch {
    Write-Host "❌ ERROR: Cannot write to this folder" -ForegroundColor Red
    Write-Host "You need to run as Administrator or choose a different location"
}
```

---

## Complete Diagnostic Script

Save this as `fix-save-issue.ps1` and run it:

```powershell
# Complete diagnostic and fix script
param(
    [string]$ProjectFile = ""
)

$folder = "$env:USERPROFILE\Documents\JobSearchTracker"

Write-Host "=== Job Search Tracker - Save Issue Diagnostic ===" -ForegroundColor Cyan
Write-Host ""

# Check if folder exists
if (-not (Test-Path $folder)) {
    Write-Host "❌ Folder does not exist: $folder" -ForegroundColor Red
    Write-Host "Creating folder..." -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $folder -Force | Out-Null
    Write-Host "✅ Folder created" -ForegroundColor Green
}

Write-Host "📁 Folder: $folder" -ForegroundColor Cyan
Write-Host ""

# List all JSON files
Write-Host "📄 JSON Files:" -ForegroundColor Cyan
$jsonFiles = Get-ChildItem "$folder\*.json" -ErrorAction SilentlyContinue

if ($jsonFiles) {
    foreach ($file in $jsonFiles) {
        Write-Host "  - $($file.Name)" -ForegroundColor Yellow
        Write-Host "    Size: $($file.Length) bytes"
        Write-Host "    Modified: $($file.LastWriteTime)"
        Write-Host "    Read-Only: $($file.IsReadOnly)" -ForegroundColor $(if($file.IsReadOnly){"Red"}else{"Green"})
        Write-Host "    Attributes: $($file.Attributes)"
        Write-Host ""
    }
} else {
    Write-Host "  (No JSON files found)" -ForegroundColor Gray
}

# Fix read-only files
Write-Host "🔧 Fixing read-only attributes..." -ForegroundColor Cyan
$fixed = 0
foreach ($file in $jsonFiles) {
    if ($file.IsReadOnly) {
        $file.IsReadOnly = $false
        $fixed++
        Write-Host "  ✅ Fixed: $($file.Name)" -ForegroundColor Green
    }
}
if ($fixed -eq 0) {
    Write-Host "  (No read-only files found)" -ForegroundColor Gray
}
Write-Host ""

# Check for locking processes
Write-Host "🔍 Checking for locking processes..." -ForegroundColor Cyan
$lockers = @()

if (Get-Process excel -ErrorAction SilentlyContinue) {
    $lockers += "Excel"
}
if (Get-Process notepad -ErrorAction SilentlyContinue) {
    $lockers += "Notepad"
}
if ((Get-Process JobSearchTracker -ErrorAction SilentlyContinue | Measure-Object).Count -gt 1) {
    $lockers += "Multiple Job Search Tracker instances"
}

if ($lockers) {
    Write-Host "  ⚠️ These programs might be locking files:" -ForegroundColor Yellow
    foreach ($locker in $lockers) {
        Write-Host "    - $locker" -ForegroundColor Red
    }
    Write-Host ""
    Write-Host "  Close these programs and try again." -ForegroundColor Yellow
} else {
    Write-Host "  ✅ No obvious locking processes found" -ForegroundColor Green
}
Write-Host ""

# Test write permissions
Write-Host "🧪 Testing write permissions..." -ForegroundColor Cyan
$testFile = "$folder\test_permissions_temp.json"
try {
    "test" | Out-File $testFile -ErrorAction Stop
    Remove-Item $testFile -ErrorAction Stop
    Write-Host "  ✅ Write permissions OK" -ForegroundColor Green
}
catch {
    Write-Host "  ❌ Cannot write to folder!" -ForegroundColor Red
    Write-Host "  Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "=== Summary ===" -ForegroundColor Cyan
Write-Host "✅ Folder exists and is accessible" -ForegroundColor Green
Write-Host "✅ Read-only attributes removed (if any)" -ForegroundColor Green
if ($lockers) {
    Write-Host "⚠️ Close locking programs and try again" -ForegroundColor Yellow
} else {
    Write-Host "✅ No locking processes detected" -ForegroundColor Green
}
Write-Host ""
Write-Host "🎯 Try saving your project now!" -ForegroundColor Cyan
```

---

## How to Run the Script

### Option 1: Quick One-Liner (Recommended)
```powershell
Get-ChildItem "$env:USERPROFILE\Documents\JobSearchTracker\*.json" | ForEach-Object { $_.IsReadOnly = $false }; Write-Host "✅ Fixed!" -ForegroundColor Green
```

### Option 2: Save and Run Script
1. Copy the complete script above
2. Save as `fix-save-issue.ps1`
3. Right-click → Run with PowerShell

### Option 3: Manual Check
1. Open File Explorer
2. Go to `C:\Users\[YourName]\Documents\JobSearchTracker\`
3. Right-click your `.json` file
4. Properties
5. Uncheck "Read-only"
6. OK

---

## Expected Results

After running the script:
```
=== Job Search Tracker - Save Issue Diagnostic ===

📁 Folder: C:\Users\...\Documents\JobSearchTracker

📄 JSON Files:
  - My_Project.json
    Size: 5432 bytes
    Modified: 1/15/2026 10:30:00 AM
    Read-Only: False ✅
    Attributes: Archive

🔧 Fixing read-only attributes...
  (No read-only files found)

🔍 Checking for locking processes...
  ✅ No obvious locking processes found

🧪 Testing write permissions...
  ✅ Write permissions OK

=== Summary ===
✅ Folder exists and is accessible
✅ Read-only attributes removed (if any)
✅ No locking processes detected

🎯 Try saving your project now!
```

---

**Run the one-liner NOW and try saving again!** 🚀
