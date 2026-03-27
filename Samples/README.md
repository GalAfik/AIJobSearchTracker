# Sample Files

This folder contains sample files to help you get started with Job Search Tracker.

---

## 📄 Sample_Job_Search_Project.json

**What it is:** A complete sample project file with realistic job search data.

**What it contains:**
- 5 sample job applications in various stages:
  - TechCorp Solutions (Interviewed - 2 interviews scheduled)
  - InnovateLabs (Applied)
  - Global Finance Corp (Rejected - with interview history)
  - CloudNine Technologies (Offered! - full interview process)
  - DataVision Analytics (Not Applied - saved for later)
- Sample interviews with different types (Phone, Video, In-Person, Technical)
- Sample contacts (recruiters, hiring managers)
- Realistic notes and status tracking

**How to use it:**
1. Open Job Search Tracker
2. Go to `File → Open Project`
3. Select `Sample_Job_Search_Project.json`
4. Explore the features with pre-loaded data!

**Why it's useful:**
- See what a real project looks like
- Learn how to organize your data
- Try out all features without entering your own data first
- Understand the JSON structure if you want to edit files manually

---

## 📊 Sample_Job_Import.csv

**What it is:** A sample CSV file for testing the import feature.

**What it contains:**
- 10 sample job applications from well-known tech companies
- Various statuses: Applied, Interviewed, Offered, Rejected, Withdrawn, NotApplied
- Different application platforms: LinkedIn, Indeed, Company Website, Glassdoor, Referral
- Realistic salary ranges and job descriptions
- Sample notes for each application

**How to use it:**
1. Create a new project or open an existing one
2. Go to `File → Import from CSV`
3. Select `Sample_Job_Import.csv`
4. Jobs will be added to your current project!

**Why it's useful:**
- Test the CSV import feature
- See the correct CSV format for importing your own data
- Understand which fields are supported
- Quickly populate a project with sample data for testing

---

## 📝 CSV Import Format

When creating your own CSV files to import, use this format:

```csv
Company,Title,Location,Status,DateApplied,Platform,SalaryRange,JobUrl,Description,Notes
```

**Required Columns:**
- `Company` - Company name
- `Title` - Job title

**Optional Columns:**
- `Location` - Job location (city, state, or remote)
- `Status` - NotApplied, Applied, Interviewed, Offered, Accepted, Rejected, or Withdrawn
- `DateApplied` - Date in format YYYY-MM-DD (e.g., 2026-01-15)
- `Platform` - LinkedInEasyApply, Indeed, Glassdoor, ZipRecruiter, Handshake, CompanyWebsite, Referral, or Other
- `SalaryRange` - Salary range as text (e.g., "$100,000 - $150,000")
- `JobUrl` - Link to job posting
- `Description` - Full job description
- `Notes` - Your personal notes

**Tips:**
- Wrap fields containing commas in quotes (e.g., "Company Name, Inc.")
- Use quotes for multi-line descriptions
- Leave optional fields empty if not needed
- The import adds jobs to your existing project (doesn't replace)

---

## 🎯 Getting Started Tips

### New Users:
1. **Start with Sample Project**: Open `Sample_Job_Search_Project.json` to see all features
2. **Explore Features**: Try the Analytics dashboard, edit jobs, add interviews
3. **Create Your Own**: When ready, create a new project for your real job search
4. **Import if Needed**: If you have existing data in Excel/CSV, export to CSV and import it

### Existing Users:
1. **Use CSV Import**: Copy the format from `Sample_Job_Import.csv` for your own data
2. **Backup First**: Always backup your project before importing large amounts of data
3. **Test Import**: Try importing the sample CSV first to verify it works as expected

---

## 🔄 Migrating from Spreadsheets

If you're currently tracking jobs in Excel or Google Sheets:

1. **Export to CSV**: Save your spreadsheet as CSV
2. **Match Column Names**: Rename your columns to match the format above
3. **Clean Data**: Ensure dates are in YYYY-MM-DD format
4. **Import**: Use `File → Import from CSV` in Job Search Tracker
5. **Review**: Check that all data imported correctly
6. **Add Details**: Enhance with interviews, contacts, and notes

---

## 📖 Additional Resources

- **Full Documentation**: See the main README.md in the project root
- **AI Feature Guide**: Learn how to set up AI job scraping
- **Bug Reports**: https://github.com/GalAfik/AIJobSearchTracker/issues
- **Support**: https://github.com/sponsors/GalAfik

---

**Questions?**

If you have questions about these sample files or the import format, please open an issue on GitHub!

https://github.com/GalAfik/AIJobSearchTracker/issues

---

© 2026 Gal Afik - Job Search Tracker Sample Files
