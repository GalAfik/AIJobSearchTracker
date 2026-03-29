# 🧪 URGENT TEST - Project Loading Fix

## Critical Bug Fixed

**Issue:** Opening different projects showed jobs from the WRONG project

**Fix:** Properly clear all collections when loading new project

**Priority:** 🔴 **TEST IMMEDIATELY**

---

## Quick Test (2 Minutes)

### Step 1: Create Two Projects

**Project A:**
```
1. Click "New Project"
2. Name: "Project A"
3. Create
4. Add 2 jobs:
   - Microsoft - Engineer
   - Google - Developer
5. Save Project (note the location)
```

**Project B:**
```
6. Click "New Project" (creates fresh project)
7. Name: "Project B"
8. Create
9. Add 2 DIFFERENT jobs:
   - Apple - Designer
   - Amazon - Manager
10. Save Project (note the location)
```

### Step 2: Test Loading

**Test A → B:**
```
11. Click "Open Project"
12. Load "Project A"

Expected: See ONLY "Microsoft" and "Google" ✅

13. Click "Open Project"
14. Load "Project B"

Expected: See ONLY "Apple" and "Amazon" ✅
         (NO Microsoft or Google!)
```

**Test B → A:**
```
15. Click "Open Project"
16. Load "Project A" again

Expected: See ONLY "Microsoft" and "Google" ✅
         (NO Apple or Amazon!)
```

---

## What You Should See

### ✅ CORRECT Behavior (After Fix)

**When loading Project A:**
```
┌────────────────────────────────┐
│ 📋 Jobs: 2                     │
├────────────────────────────────┤
│ Microsoft                      │
│ Engineer                       │
├────────────────────────────────┤
│ Google                         │
│ Developer                      │
└────────────────────────────────┘
```

**When loading Project B:**
```
┌────────────────────────────────┐
│ 📋 Jobs: 2                     │
├────────────────────────────────┤
│ Apple                          │
│ Designer                       │
├────────────────────────────────┤
│ Amazon                         │
│ Manager                        │
└────────────────────────────────┘
```

### ❌ WRONG Behavior (Before Fix)

**When loading Project B (showing old jobs):**
```
┌────────────────────────────────┐
│ 📋 Jobs: 4                     │ ← WRONG! Should be 2
├────────────────────────────────┤
│ Microsoft                      │ ← From Project A! ❌
│ Engineer                       │
├────────────────────────────────┤
│ Google                         │ ← From Project A! ❌
│ Developer                      │
├────────────────────────────────┤
│ Apple                          │ ← From Project B ✅
│ Designer                       │
├────────────────────────────────┤
│ Amazon                         │ ← From Project B ✅
│ Manager                        │
└────────────────────────────────┘
```

---

## Additional Tests

### Test 3: Empty Project
```
1. Load Project A (has jobs)
2. Create New Project (empty)

Expected:
- Job count: 0
- Job list: Empty
- Detail panel: "Select a job to view details"
```

### Test 4: With Filters
```
1. Load Project A
2. Apply filter (e.g., search for "Microsoft")
3. See filtered result
4. Load Project B

Expected:
- Filter is RESET (search box empty)
- All Project B jobs visible
- NO Project A jobs visible
```

### Test 5: Selected Job
```
1. Load Project A
2. Click on first job (Microsoft)
3. See details in right panel
4. Load Project B

Expected:
- Selection cleared (no job selected)
- Detail panel shows "Select a job to view details"
- Right panel is empty
```

---

## Pass/Fail Criteria

### ✅ PASS if:
- Loading Project A shows ONLY Project A's jobs
- Loading Project B shows ONLY Project B's jobs
- Job count is correct for each project
- Switching between projects shows correct jobs every time
- No mixing of jobs from different projects
- Filter resets when loading new project
- Selection clears when loading new project

### ❌ FAIL if:
- Any jobs from old project remain visible
- Job count is wrong
- Jobs from multiple projects mix together
- Old selected job still shows after loading new project
- Filter doesn't reset

---

## If Test FAILS

**Report immediately with:**

1. **Which step failed?** (Step number)
2. **What did you see?** (screenshot if possible)
3. **Expected vs Actual:**
   ```
   Expected: See only Apple and Amazon
   Actual: Saw Microsoft, Google, Apple, Amazon
   ```
4. **Job count:** What number showed in "📋 Jobs: X"?

---

## If Test PASSES

**Confirm:**
```
✅ Tested loading different projects
✅ Each project shows ONLY its own jobs
✅ No mixing of jobs from different projects
✅ Job count is always correct
✅ Filter and selection reset properly

APPROVED FOR DEPLOYMENT ✅
```

---

## Why This Test Is Critical

This bug could cause users to:
- ❌ See wrong data
- ❌ Edit jobs from the wrong project
- ❌ Delete jobs they didn't intend to delete
- ❌ Confuse projects and lose data

**This is a data integrity issue!**

---

## Quick PowerShell Test (Alternative)

If you want to test from command line:

```powershell
# Create test projects
$projectA = @{
    Name = "Project A"
    Jobs = @(
        @{ CompanyName = "Microsoft"; JobTitle = "Engineer" },
        @{ CompanyName = "Google"; JobTitle = "Developer" }
    )
}

$projectB = @{
    Name = "Project B"
    Jobs = @(
        @{ CompanyName = "Apple"; JobTitle = "Designer" },
        @{ CompanyName = "Amazon"; JobTitle = "Manager" }
    )
}

# Save to JSON
$projectA | ConvertTo-Json -Depth 10 | Out-File "TestProjectA.json"
$projectB | ConvertTo-Json -Depth 10 | Out-File "TestProjectB.json"

Write-Host "Test files created: TestProjectA.json and TestProjectB.json"
Write-Host "Load them in the app and verify only correct jobs show!"
```

---

## Expected Timeline

- ⏱️ **2 minutes** to create test projects
- ⏱️ **1 minute** to test loading
- ⏱️ **1 minute** to verify additional tests

**Total: 4 minutes to verify critical fix**

---

## TL;DR

1. ✅ Create Project A with 2 jobs (Microsoft, Google)
2. ✅ Create Project B with 2 jobs (Apple, Amazon)
3. ✅ Load Project A → Should see ONLY Microsoft + Google
4. ✅ Load Project B → Should see ONLY Apple + Amazon
5. ✅ Load Project A again → Should see ONLY Microsoft + Google

**If all steps show ONLY the correct jobs → ✅ FIX WORKS!**

**If you see mixed jobs → ❌ FIX FAILED, REPORT IMMEDIATELY**

---

**RUN THIS TEST NOW!** 🚀

The fix is critical for data integrity. Test thoroughly before deployment!
