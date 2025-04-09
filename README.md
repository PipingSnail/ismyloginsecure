# IsMyLoginSecure

IsMyLoginSecure is a tool to test that a page linking to a https login page is also https and doesn't have any security weaknesses (bad security certificate, insecure content, etc).

# Background

IsMyLoginSecure started life as a way to automate the checking of various websites to check that the page linking to a login page was https and not http, to prevent man-in-the-middle attacks.

It all started when Liam Blizard noticed Natwest's http pages were linking to their https login page. Then myself and Troy Hunt, Scott Helme and many other people caused a lot of fuss and we got in the national press. Not that that was the intent.

I then spent some time manually curating a list of banks, and after that lists of many types of organisations holding financial or health data, and whether their login pages were secure against man-in-the-middle attacks.

This initial test was done manually, but once we had the lists the next thing to do was to automate the testing. Liam and I intended to turn this into a website, but due to our work commitments this never happened. This desktop demo was side project at
[softwareverify](https://www.softwareverify.com) and has been sitting on a development machine for years and I thought better that someone may benefit from it, as neither of us will ever turn this into a product. I've tidied the code up a bit, documented things
and improved the usability so that if anyone wants to use this to run simple tests on domains of their choice they can do that with the existing binary available from IsMyLoginSecure, or if they want to modify the default lists of organisations that is now split out into it's own file. Adding a new list of organisations is straightforward.

# More Information

All blog articles about the security (or lack of) of various organisations that was on Software Verify's website is now on a dedicated website [ismyloginsecure](https://www.ismyloginsecure.com), as is a downloadable binary for those wishing to execute without building.

Correspondence: [ismyloginsecure@sprezzaturra.com](ismyloginsecure@sprezzaturra.com)
