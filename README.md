# Easy Encounters

## What is it?

A tool for managing, running, and designing combat encounters for D&D 5th edition. A Windows desktop app using WinUI3 and Entity Framework Core along with SQLite. It's grown much larger than my initial intent, and now that some friends have expressed interest in using the tool I intend to rebuild it as an ASP.NET Core project; it'd be much more accessible as a SPA than something that needs downloading and installation.


## Why?

Two reasons: Firstly, I was looking for a good project to experiment with WinUI3 / spend some time learning the framework, just to get a feel for the direction that Windows Desktop development might move in the future. 
Secondly, I wanted a tool that I might actually use myself; existing products didn't fit my desired use-case for something that would be reasonably unobtrusive and flexible when running encounters, but more fully-featured and searchable than a spreadsheet.


## Any takeaways from the project?

WinUI3, at least at the time of writing this application, is stil quite immature in some areas. No input validation or built-in way to display feedback on input fields out-of-the-box was an interesting feature to lose from WPF, but as it was on the development roadmap it didn't seem worth implementing from scratch on a small personal use project. Some controls (notably TabView) were less stable than I'd anticipated; there were known issues related to dragging Tabs, and I encountered an issue where re-ordering the tabs programmatically could sometimes cause tabs to point at the wrong content ( and then crash the app ). The MVVM Community Toolkit roslyn features are in a really good place though, annotating properties to avoid boilerplate feels like a real productivity boost.
Certainly will be differences to how I approach this particular model and database in the ASP.NET core version; this version has some obvious pollution between VM and Model purely as a result of some prototyping around switch to EFCore + SQLite. It doesn't affect functionality, but it feels bad to have that lack of clean architecture separation of concerns.


