# TechStackDemo

Note: Very raw HTML ahead!

I'm also really happy to hear "I have no idea" or "I've never used that before".  I can (try) to teach if you're keen to learn and can apply knowledge (for example, some people like to think of reactive data binding as though they were Excel workbook updates).

The goal of this repository is to serve as a discussion around
* async / await
* concurrency
* SignalR
* TypeScript
* MVVM - Knockout, data-binding, components
* ASP.Net Bundling
* RX
* Client-side / server-side repository
* How observables work.  Why we keep them in a dictionary client-side.  Why the "reset" method doesn't just clear the dictionary in this case
* The remaining bug.  Hint: There's a CounterMutator doing something
* Don't worry about the styling - I promise our HTML is better than this :D
* The client-side code is also better structured usually.  I planned to make this a larger example project but some bike-shedding and rabbit holes got in the way.
* Apologies for lack of extra explanatory comments

Exploration:
Check out the code.  Let NuGet restore packages.  VS 2015 community edition with IIS Express ought to be sufficient to run the code.  .NET 4.6.1 was targeted but it could be targeted back to an earlier version if required.

Open a couple of pages simultaneously.  Click increment / decrement buttons to see what happens.  Check the two *.html files to see what's expected.  Open a new page after 10-15 seconds - is it reflecting the same data as the other pages?  What's wrong?  How would you go about diagnosing this?  I'm rather close to thinking [select is actually broken](http://blog.codinghorror.com/the-first-rule-of-programming-its-always-your-fault/) since I've got the same code working in our usual software but I've probably overlooked something :)

Try changing reset client-side to just reset the dictionary.  Why doesn't this have the desired effect?


Enjoy!


PS: 
TypeScript 1.8 beta came out after I wrote this.  It should still compile but it's extra checking did catch a couple of silly issues in our main codebase so that may be the case here too.
