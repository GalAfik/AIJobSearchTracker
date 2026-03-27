# GitHub Sponsors Setup Guide

## Overview
This guide will help you set up GitHub Sponsors to accept donations for the Job Search Tracker project.

## Part 1: GitHub Sponsors Setup

### Prerequisites
✅ GitHub account in good standing for at least 90 days  
✅ Two-Factor Authentication (2FA) enabled  
✅ Located in a supported region (see below)  
✅ Email verified on GitHub  

### Supported Regions
GitHub Sponsors is available in these countries:
- United States
- United Kingdom
- Canada
- Germany
- France
- Spain
- Italy
- Netherlands
- Australia
- Japan
- And many more...

Check the full list: https://docs.github.com/en/sponsors/receiving-sponsorships-through-github-sponsors/about-github-sponsors#supported-regions

### Step-by-Step Setup

#### 1. Apply for GitHub Sponsors
1. Visit: https://github.com/sponsors
2. Click **"Join the waitlist"** or **"Set up GitHub Sponsors"**
3. Read and accept the GitHub Sponsors terms

#### 2. Connect Payment Provider
You'll need to connect **Stripe** to receive payments:

1. Click **"Connect with Stripe"**
2. Create a Stripe account or log in to existing one
3. Complete Stripe's identity verification:
   - Provide legal name
   - Date of birth
   - Last 4 digits of SSN (US) or equivalent ID
   - Bank account information for payouts

#### 3. Set Up Tax Information
Depending on your location:

**For US residents:**
- Complete Form W-9
- Provide your SSN or EIN

**For non-US residents:**
- Complete Form W-8BEN
- Provide Tax ID if applicable
- Claim tax treaty benefits if eligible

#### 4. Create Your Sponsor Profile

Fill out your profile at `https://github.com/sponsors/GalAfik`:

**Profile Information:**
- **Display name**: Gal Afik
- **Bio**: Brief description of yourself
- **Featured work**: Link to JobSearchTracker repository
- **Sponsor button**: Enable on your profile

**Sponsorship Tiers:**

Here are suggested tiers for your project:

| Tier | Amount | Name | Benefits |
|------|--------|------|----------|
| 1 | $1/month | Coffee Supporter ☕ | • Name in SUPPORTERS.md<br>• My eternal gratitude |
| 2 | $5/month | Bug Fixer 🐛 | • All previous benefits<br>• Priority bug reports<br>• Sponsor badge on profile |
| 3 | $10/month | Feature Sponsor ⭐ | • All previous benefits<br>• Vote on new features<br>• Early access to releases |
| 4 | $25/month | Premium Sponsor 💎 | • All previous benefits<br>• Name in app credits<br>• Direct support via email |
| 5 | $50/month | Gold Sponsor 🏆 | • All previous benefits<br>• 1-hour consulting session<br>• Custom feature requests |

**One-time Sponsorships:**
Enable custom amounts: $5, $10, $25, $50, $100

**Goals (Optional):**
- Set funding goals (e.g., "Cover hosting costs - $50/month")
- Show progress to potential sponsors

#### 5. Review and Submit
1. Review all information
2. Click **"Submit for review"**
3. Wait for GitHub approval (usually 1-3 business days)

### Step 5: After Approval

Once approved, your sponsor page will be live at:
**https://github.com/sponsors/GalAfik**

#### Enable Sponsor Button on Repository
1. Go to your repository: https://github.com/GalAfik/AIJobSearchTracker
2. Click **Settings** → **General**
3. Scroll to **Sponsorships**
4. Enable **"Display a sponsor button"**
5. Add your GitHub username: `GalAfik`
6. Save changes

This adds a "Sponsor" button next to "Watch" and "Star" on your repo!

## Part 2: Application Integration

### What We've Implemented

✅ **Sponsor button in footer** - Pink heart button with GitHub Sponsors color (#EA4AAA)  
✅ **Menu item** - "Sponsor this Project" in Help menu  
✅ **Click handling** - Opens your GitHub Sponsors page  
✅ **Error handling** - Shows fallback message if browser fails  

### Testing the Integration

1. Run your application
2. Look for the "❤️ Sponsor" button in the bottom-right of the footer
3. Click it - should open: https://github.com/sponsors/GalAfik
4. Also check **Help → Sponsor this Project** menu

### Customizing the Sponsor Button

The button uses GitHub's official sponsor color: `#EA4AAA`

To customize the button appearance, edit `MainWindow.xaml` around line 463:

```xaml
<Button Grid.Column="2" 
        Content="❤️ Sponsor" 
        Click="SponsorButton_Click"
        ...>
```

## Part 3: Marketing Your Sponsors Page

### Add Sponsor Badge to README

Add this to your README.md:

```markdown
## Support This Project

If you find Job Search Tracker helpful, consider sponsoring its development!

[![Sponsor](https://img.shields.io/badge/Sponsor-❤️-EA4AAA?style=for-the-badge&logo=github-sponsors&logoColor=white)](https://github.com/sponsors/GalAfik)

Your sponsorship helps with:
- 🐛 Bug fixes and maintenance
- ✨ New feature development
- 📚 Documentation improvements
- ☁️ Cloud hosting costs
```

### Promote in Release Notes

When you publish releases, mention sponsorship:

```markdown
## ⭐ Support Development

Enjoying this release? Consider [becoming a sponsor](https://github.com/sponsors/GalAfik)!
```

### Social Media

Share your sponsors page:
- Twitter/X: "Now accepting GitHub Sponsors! Support Job Search Tracker development 💜"
- LinkedIn: Professional post about open-source sustainability
- Dev.to: Write about your journey accepting sponsorships

## Part 4: Best Practices

### Communication
- Thank sponsors publicly (with permission)
- Send update emails to sponsors
- Credit sponsors in release notes

### Transparency
- Share what sponsorship funds are used for
- Post funding goals and progress
- Update your sponsor tiers as the project grows

### Exclusive Perks
Consider offering:
- Early access to beta features
- Private Discord/Slack channel for sponsors
- Monthly development updates
- Priority support on issues
- Logo placement for business sponsors

### Tax Implications
- **US**: Sponsorship income is taxable, report on Schedule C
- **Non-US**: Consult local tax regulations
- Keep records of all transactions
- Consider setting aside ~30% for taxes

## Part 5: Alternative/Additional Platforms

If GitHub Sponsors isn't available in your region, or you want more options:

### Buy Me a Coffee
- Easy setup: https://www.buymeacoffee.com/
- One-time and monthly support
- Lower fees than some platforms
- Quick integration

### Ko-fi
- Similar to Buy Me a Coffee
- https://ko-fi.com/
- Supports donations and selling digital products

### Patreon
- Monthly subscriptions
- Great for ongoing content
- Higher fees but more features

### Open Collective
- Full transparency
- Fiscal sponsorship available
- Great for larger projects

## Support & Resources

### GitHub Documentation
- Sponsors docs: https://docs.github.com/en/sponsors
- Setting up profile: https://docs.github.com/en/sponsors/receiving-sponsorships-through-github-sponsors/setting-up-github-sponsors-for-your-personal-account

### Community
- GitHub Community: https://github.community/
- GitHub Support: https://support.github.com/

### Need Help?
- Check GitHub Sponsors FAQ
- Contact GitHub Support
- Join GitHub Sponsors Discord (if available)

---

## Quick Checklist

Before going live:
- [ ] GitHub Sponsors application approved
- [ ] Stripe account connected and verified
- [ ] Tax information submitted
- [ ] Sponsorship tiers created
- [ ] Profile bio and featured work added
- [ ] Sponsor button enabled on repository
- [ ] Application tested (sponsor button works)
- [ ] README updated with sponsor badge
- [ ] Marketing plan ready

## Your Sponsor URL

Once approved, your sponsor page will be:
**https://github.com/sponsors/GalAfik**

Share it proudly! 🎉
