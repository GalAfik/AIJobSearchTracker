# Job Search Tracker

**Your all-in-one solution for managing your job search journey** 🎯

Version 0.1.0 | Free & Open Source

---

## 📥 Quick Start

1. **Run the Application**
   - Double-click `JobSearchTracker.exe`
   - Click "Get Started!" on the welcome screen

2. **Create Your First Project**
   - Go to `File → New Project`
   - Give it a name and save location
   - Start tracking!

3. **Add Your First Job**
   - Click `➕ Add Job` in the toolbar
   - Fill in the details
   - Click Save

---

## ✨ Key Features

### 📋 Job Tracking
Track unlimited job applications with:
- Company name and job title
- Location and salary range
- Application status and dates
- Job posting URL
- Detailed notes and descriptions

### 🤖 AI-Powered Job Scraping
Paste any job posting URL and let AI automatically extract:
- Company name
- Job title
- Location
- Full job description

**Supported AI Providers:**
- Claude (Anthropic)
- OpenAI (ChatGPT)
- Google Gemini

*Note: Requires free API keys (see setup below)*

### 💼 Interview Management
- Schedule interview rounds
- Track interview types (Phone, Video, In-Person, Technical)
- Record interviewer contacts
- Note outcomes and feedback

### 👤 Contact Tracking
- Save recruiter and hiring manager details
- Email and phone contacts
- LinkedIn profiles
- Position and notes

### 📊 Analytics Dashboard
View comprehensive insights:
- Total applications and active jobs
- Interview and offer rates
- Response time averages
- Activity patterns and streaks
- Platform success metrics

### 📁 Import/Export
- **Import** existing data from CSV
- **Export** to Excel or CSV
- Backup your data anytime

### 🎨 Customization
- Light and Dark themes
- Compact or detailed job list view
- Customizable sorting and filtering
- Personalized preferences

---

## 🔑 Setting Up AI Features (Optional)

AI job scraping is **optional** but powerful. To enable it:

### 1. Get Free API Keys

Choose one or more providers:

**Claude (Anthropic)** - Recommended
1. Visit: https://console.anthropic.com/
2. Sign up for a free account
3. Create an API key
4. Copy the key (starts with `sk-ant-`)

**OpenAI (ChatGPT)**
1. Visit: https://platform.openai.com/
2. Sign up for a free account
3. Go to API Keys section
4. Create a new key
5. Copy the key (starts with `sk-`)

**Google Gemini**
1. Visit: https://makersuite.google.com/
2. Sign up with Google account
3. Get API key
4. Copy the key

### 2. Enter Keys in Application

1. Open Job Search Tracker
2. Go to `Help → Preferences`
3. Click on `AI Configuration` tab
4. Paste your API key(s)
5. Select your preferred provider
6. Click Save

### 3. Use AI Scraping

1. Click `🤖 AI Add` in the toolbar
2. Paste a job posting URL
3. Wait a few seconds
4. Review and save the extracted information

---

## 📖 Common Tasks

### Creating and Managing Projects

**New Project:**
`File → New Project`

**Open Existing:**
`File → Open Project` (select `.json` file)

**Save Changes:**
`File → Save Project` or `Ctrl+S`

**Import CSV:**
`File → Import from CSV` (adds to current project)

### Working with Jobs

**Add Job Manually:**
1. Toolbar: `➕ Add Job`
2. Fill in details
3. Click Save

**Add Job with AI:**
1. Toolbar: `🤖 AI Add`
2. Paste job URL
3. Review extracted data
4. Click Save

**Edit Job:**
- Click on a job in the list
- Edit fields directly in the right panel
- Click `💾 Save Changes`

**Delete Job:**
1. Select a job
2. Toolbar: `🗑️ Delete Job`
3. Confirm deletion

### Adding Interviews & Contacts

**Add Interview:**
1. Select a job
2. Scroll to Interviews section
3. Click `➕ Add Interview`
4. Fill in details
5. Save

**Add Contact:**
1. Select a job
2. Scroll to Contacts section
3. Click `➕ Add Contact`
4. Fill in details
5. Save

### Using Analytics

**View Analytics:**
`Help → Analytics`

See insights including:
- Application totals and rates
- Interview conversion rates
- Activity streaks
- Platform performance
- Time-based metrics

### Filtering and Sorting

**Search:**
Type in the Search box to filter by company, title, location, or notes

**Filter by Status:**
Use the Status dropdown to show only:
- Applied, Interviewed, Offered, Rejected, etc.

**Sort:**
Choose sort order:
- Date Added, Company Name, Date Applied, Status

### Exporting Data

**Export to Excel:**
`File → Export to Excel`

**Export to CSV:**
`File → Export to CSV`

Both options create a file with all job data, interviews, and contacts.

---

## ⚙️ Preferences

Access: `Help → Preferences`

### Appearance
- **Theme**: Light or Dark
- **Compact View**: Space-saving job list
- **Show Intro**: Toggle welcome screen on startup

### Directions
- Set your home address
- Click 🗺️ on any job to get directions from home to job location

### Sorting
- Set default sort order for job list

### AI Configuration
- Enter API keys
- Select preferred AI provider

---

## 🆘 Troubleshooting

### "The application won't start"
- Ensure you're on Windows 10 or 11 (64-bit)
- Make sure all files were extracted from the ZIP
- Check Windows Defender didn't block the file

### "AI scraping isn't working"
- Verify your API key is entered correctly
- Check you have internet connection
- Ensure the API key has available credits
- Try a different AI provider

### "My data disappeared"
- Make sure you saved your project (`File → Save`)
- Check the last save location
- Projects are `.json` files—search your computer

### "Export failed"
- Ensure you have write permissions in the target folder
- Try exporting to a different location
- Close any Excel files that might be open

### "The app is slow"
- Try Compact View mode
- Close and reopen the application
- Check if you have very large projects (1000+ jobs)

---

## 💡 Tips & Best Practices

### Organization
- ✅ Create separate projects for different job search campaigns
- ✅ Use meaningful project names (e.g., "Job Search 2026", "Tech Jobs Q1")
- ✅ Keep notes detailed for future reference

### Tracking
- ✅ Update job status as soon as things change
- ✅ Add interviews immediately after scheduling
- ✅ Record interviewer names and contacts
- ✅ Note salary discussions in the Notes field

### Efficiency
- ✅ Use AI scraping for quick job additions
- ✅ Set up filters to focus on active applications
- ✅ Check Analytics weekly to track progress
- ✅ Export data regularly as backup

### Motivation
- ✅ Celebrate the streaks shown in Analytics
- ✅ Watch your interview rate improve over time
- ✅ Read the motivational title bar messages
- ✅ Remember: every application is progress!

---

## 🐛 Found a Bug?

Help improve the application:
1. Go to: https://github.com/GalAfik/AIJobSearchTracker/issues
2. Click "New Issue"
3. Describe what happened
4. Include steps to reproduce

Your feedback makes this better for everyone!

---

## ❤️ Support This Project

Job Search Tracker is **100% free** and always will be.

If you find it helpful:
- ⭐ Star the project on GitHub
- 💬 Share with friends who are job searching
- 💝 Consider sponsoring: https://github.com/sponsors/GalAfik

Your support helps maintain and improve the app!

---

## 📚 Additional Resources

- **Full Documentation**: https://github.com/GalAfik/AIJobSearchTracker
- **AI Setup Guide**: See `AI_FEATURE_GUIDE.md`
- **Report Issues**: https://github.com/GalAfik/AIJobSearchTracker/issues
- **Request Features**: https://github.com/GalAfik/AIJobSearchTracker/discussions

---

## 🙏 Thank You

Thank you for using Job Search Tracker. Job searching is tough, but you're not alone. This tool is here to help you stay organized and motivated.

**You've got this!** 💪

Every application is a step forward. Keep going, and good luck with your search! 🍀

---

## 📄 License & Credits

**Job Search Tracker** is open source software.

Built with ❤️ by a job seeker, for job seekers.

**Version**: 0.1.0  
**Released**: 2026  
**Author**: Gal Afik  
**Repository**: https://github.com/GalAfik/AIJobSearchTracker

© 2026 Gal Afik - All Rights Reserved

---

**Need help? Have questions? Found this useful?**

Open an issue, start a discussion, or just give us a star on GitHub!

https://github.com/GalAfik/AIJobSearchTracker
