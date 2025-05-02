// Implementation: Stephen Kellett 28 December 2017..10 January 2018 and March/April 2025
// Copyright (c) Software Verify, IsMyLoginSecure 2017-2025.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// The above licence is the MIT Licence. https://opensource.org/license/MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using Microsoft.VisualBasic.FileIO;                             // for the CSV reader
using isMyLoginSecure;

// tests:                                                       Status
// 1) HTTP, no HTTPS            www.objmedia.demon.co.uk        Not secure
// 2) HTTP and HTTPS            www1.firstdirect.com/1/2/       Not secure
// 3) HTTPS with invalid cert   bankofirelanduk.com/            Not secure
// 4) HTTP redirects to HTTPS   www.softwareverify.com          Secure

// TODO, identify any hidden login forms injected into a page by AdThink etc - this prevents violation of GDPR
// See https://flipboard.com/@flipboard/-ad-targeters-are-pulling-data-from-your/f-1775c6c6f3%2Ftheverge.com
// https://www.bleepingcomputer.com/news/security/web-trackers-exploit-flaw-in-browser-login-managers-to-steal-usernames/

namespace isMyLoginSecureDesktopDemo
{
    /// <summary>
    /// This is the main GUI for the application that demonstrates how to use the isMyLoginSecure assembly.
    /// </summary>
    /// <remarks>Most of this class is concerned with providing data to work with the test and driving the GUI.
    /// The actual methods that drive IsMyLoginSecure are testURL(), testURL_internal(), displaySecurityHeaders() 
    /// and displaySecurityCertificate().</remarks>
    public partial class MainGUI : Form
    {
        // Various inspiration for multithreaded processing, but the simplest seemed to be dedicated worker thread and BeginInvoke for async GUI updates
        //
        //      https://stackoverflow.com/questions/661561/how-do-i-update-the-gui-from-another-thread
        //      https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1.report?view=net-9.0
        //      https://lostechies.com/gabrielschenker/2009/01/23/synchronizing-calls-to-the-ui-in-a-multi-threaded-application/
        //      this.BeginInvoke(new Action<websiteStatus>(displayResult), {ws});

        private bool processURLs = true;
        private int numToProcess = 0;
        private int numProcessed = 0;
        private bool runURLProcessingThread = true;
        private Thread urlProcessingWorker;
        private Queue<websiteStatus> urlsToProcess;

        // counts for each statistic that fails. These counts are so that each column header can be updated with
        // the failure counts so that you can at a glance see how many failed, without needing to manually count them.

        private int httpTransportCount = 0;
        private int isNotSecureCount = 0;
        private int isSecureCount = 0;
        private int failedToFetchCount = 0;
        private int doesNotRedirectCount = 0;
        private int sslPolicyErrorCount = 0;
        private int mixedContentCount = 0;
        private int imperfectSecurityHeaderCount = 0;
        private int badCertificateCount = 0;

        // column headers so that we can easily modify the text that is displayed.

        private ColumnHeader chID;
        private ColumnHeader chName;
        private ColumnHeader chTransport;
        private ColumnHeader chSecure;
        private ColumnHeader chProtocol;
        private ColumnHeader chRedirect;
        private ColumnHeader chSSLCertificate;
        private ColumnHeader chSecureContent;
        private ColumnHeader chSecurityHeaders;
        private ColumnHeader chTestURL;
        private ColumnHeader chActualURL;
        private ColumnHeader chUserAgent;
        private ColumnHeader chUserAgentString;

        private ListViewColumnSorter lvwColumnSorter;

        private string userAgent;

        /// <summary>
        /// Set up the main GUI, add columns to the list of results, create menus on the display.
        /// </summary>
        public MainGUI()
        {
            InitializeComponent();

            // track resize so that we can ensure the form is redrawn after a resize (required because of the Table view not drawing labels properly sometimes)

            this.Resize += resizeEventHandler;

            // setup the GUI

            chID = listViewResults.Columns.Add("ID", 60, HorizontalAlignment.Right);
            chName = listViewResults.Columns.Add("Name", 100, HorizontalAlignment.Left);
            chTransport = listViewResults.Columns.Add("Transport", 80, HorizontalAlignment.Left);
            chSecure = listViewResults.Columns.Add("Secure", -2, HorizontalAlignment.Left);
            chProtocol = listViewResults.Columns.Add("Protocol", -2, HorizontalAlignment.Left);
            chRedirect = listViewResults.Columns.Add("Redirect", -2, HorizontalAlignment.Left);
            chSSLCertificate = listViewResults.Columns.Add("SSL Certificate", -2, HorizontalAlignment.Left);
            chSecureContent = listViewResults.Columns.Add("Secure Content", -2, HorizontalAlignment.Left);
            chSecurityHeaders = listViewResults.Columns.Add("Security Headers", -2, HorizontalAlignment.Left);
            chTestURL = listViewResults.Columns.Add("Test URL", -2, HorizontalAlignment.Left);
            chActualURL = listViewResults.Columns.Add("Actual URL", -2, HorizontalAlignment.Left);
            chUserAgent = listViewResults.Columns.Add("User Agent", -2, HorizontalAlignment.Left);
            chUserAgentString = listViewResults.Columns.Add("User Agent String", -2, HorizontalAlignment.Left);

            listViewResults.SelectedIndexChanged += new EventHandler(ListViewResults_SelectedIndexChanged);
            listViewResults.ColumnClick += ListViewResults_ColumnClick;

            // ensure the list knows how to sort

            lvwColumnSorter = new ListViewColumnSorter();
            lvwColumnSorter.ColumnID = chID.DisplayIndex;
            lvwColumnSorter.ColumnSecurity = chSecurityHeaders.DisplayIndex;
            listViewResults.ListViewItemSorter = lvwColumnSorter;

            // add context menu to list view

            listViewResults.ContextMenuStrip = listViewResultsContextMenuStrip;

            urlProgressBar.Step = 0;
            urlProgressBar.Minimum = 0;
            urlProgressBar.Maximum = 0;
            urlProgressBar.Value = 0;
            updateProgressBar();

            // ensure we know when the user closes the form (File->Exit and [x])

            this.FormClosed += new FormClosedEventHandler(FormClosedHandler);

            // set up the User Agents menu

            userAgent = userAgents.theUserAgents[0].userAgent;

            addUserAgentMenuItem("All Browsers");
            addUserAgentMenuItem(null);
            foreach (var theUserAgent in userAgents.theUserAgents)
            {
                addUserAgentMenuItem(theUserAgent.userAgent);
            }

            // setup the Tests menu

            addTestMenuItem("All Tests");
            addTestMenuItem(null);
            addTestMenuItem("Banks");
            addTestMenuItem("Building Societies");
            addTestMenuItem("Casinos");
            addTestMenuItem("Currency Exchanges");
            addTestMenuItem("Ecommerce Vendors");
            addTestMenuItem("Healthcare Companies");
            addTestMenuItem("Insurance Companies");
            addTestMenuItem("Pension Funds");
            addTestMenuItem("Stock Traders");
            addTestMenuItem("Wealth Managers");

            // this will hold the data we're going to process

            urlsToProcess = new Queue<websiteStatus>();

            // timer to update the graphics

            timerGraphicsRefresh.Interval = 250;                         // 250ms, 4 times per second
            timerGraphicsRefresh.Tick += new EventHandler(TimerEventProcessor);
            timerGraphicsRefresh.Start();

            // start thread to do the work processing the URLs without killing UI responsiveness

            startURLProcessingThread();

            // display licence dialog if first time using the software

            if (GetIsFirstTimeRunning())
            {
                LicenceDialog dlg = new LicenceDialog();

                dlg.ShowDialog();
                SetFirstTimeRunning(false);
            }
        }

        private void ListViewResults_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            listViewResults.Sort();
        }

        /// <summary>
        /// Start a thread to process the data
        /// </summary>
        private void startURLProcessingThread()
        {
            urlProcessingWorker = new Thread(() => urlProcessingThread(this));
            urlProcessingWorker.SetApartmentState(ApartmentState.STA);
            urlProcessingWorker.Start();
        }

        /// <summary>
        /// Helper method to add an item to the User Agent menu.
        /// </summary>
        /// <param name="title">The name of the item to add to the menu.</param>
        private void addUserAgentMenuItem(string title)
        {
            if (title != null)
            {
                ToolStripMenuItem tsmi;

                tsmi = new ToolStripMenuItem(title);
                tsmi.Click += new EventHandler(userAgentToolStripMenuItem_Click);
                if (title == userAgent)
                    tsmi.Checked = true;

                this.userAgentToolStripMenuItem.DropDownItems.Add(tsmi);
            }
            else
            {
                this.userAgentToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
            }
        }

        /// <summary>
        /// Helper method to add an item to the Test menu.
        /// </summary>
        /// <param name="title">The name of the item to add to the menu.</param>
        private void addTestMenuItem(string title)
        {
            if (title != null)
            {
                ToolStripMenuItem tsmi;

                tsmi = new ToolStripMenuItem(title);
                tsmi.Click += new EventHandler(testToolStripMenuItem_Click);

                this.testToolStripMenuItem.DropDownItems.Add(tsmi);
            }
            else
            {
                this.testToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
            }
        }

        /// <summary>
        /// Helper method to turn a human readable user agent into a user agent string.
        /// </summary>
        /// <param name="userAgent">The human readable user agent. Example: "Chrome".</param>
        /// <returns>The user agent. Example: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.78 Safari/537.36".</returns>
        private string getUserAgentString(string userAgent)
        {
            int i, n;

            n = userAgents.theUserAgents.Length;
            for (i = 0; i < n; i++)
            {
                if (userAgents.theUserAgents[i].userAgent == userAgent)
                    return userAgents.theUserAgents[i].userAgentString;
            }

            // shouldn't happen

            return "";
        }

        /// <summary>
        /// Helper method to test a given user with the specified user agent.
        /// </summary>
        /// <param name="businessName">The name of the business.</param>
        /// <param name="url">The website URL.</param>
        private void testURL(string businessName,
                             string url)
        {
            // remove any http:// or https:// prefix

            url = removeWebTransportDeclaration(url);

            string theUserAgent;

            if (userAgent == "All Browsers")
            {
                // if all browsers, test the URL with ever user agent we have

                int i, n;

                n = userAgents.theUserAgents.Length;
                for (i = 0; i < n; i++)
                {
                    testURL_internal(userAgents.theUserAgents[i].userAgentString, userAgents.theUserAgents[i].userAgent, businessName, url);
                }
            }
            else
            {
                // test the website with the specified user agent

                theUserAgent = getUserAgentString(userAgent);

                testURL_internal(theUserAgent, userAgent, businessName, url);
            }
        }

        /// <summary>
        /// The thread that tests each URL to see if it passes the security tests.
        /// </summary>
        /// <param name="gui">The user interface.</param>
        static private void urlProcessingThread(MainGUI gui)
        {
            while(gui.runURLProcessingThread)
                gui.doWork();
        }

        /// <summary>
        /// Test each URL to see if it passes the security tests.
        /// </summary>
        private void doWork()
        { 
            while (processURLs)
            {
                websiteStatus ws = null;

                // get data from shared resource and hold lock for as short as possible

                lock (urlsToProcess)
                {
                    if (urlsToProcess.Count() > 0)
                    {
                        ws = urlsToProcess.Dequeue();
                    }
                }

                // do the work on the URL we're working on

                if (ws != null)
                {
                    if (processURLs)
                    {
                        numProcessed++;
                        this.BeginInvoke(new Action<websiteStatus, bool>(addDisplayResult), new object[] { ws, false });
                    }

                    if (ws.getURL().Length > 0)
                    {
                        ws.testHTTP(false, ws.getUserAgent(), ws.getUserFriendlyUserAgent());
                        ws.testHTTP(true, ws.getUserAgent(), ws.getUserFriendlyUserAgent());
                    }

                    // update the GUI with this data now we've got something to work with
                    // we check processURLs because this may be reset while the testHTTP calls are running

                    if (processURLs)
                    {
                        this.BeginInvoke(new Action<websiteStatus>(updateResult), new [] { ws});
                    }
                }
                else
                {
                    // no data, sleep for a while (better implementation would wakeup on an event being signalled)

                    System.Threading.Thread.Sleep(50);
                }
            }
        }

        /// <summary>
        /// Helper method to test a website with a specified user agent.
        /// </summary>
        /// <param name="theUserAgent">The user agent. Example: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.78 Safari/537.36".</param>
        /// <param name="theUserFriendlyUserAgent">Human readable user agent. Example: "Chrome".</param>
        /// <param name="businessName">The business name.</param>
        /// <param name="url">The website URL.</param>
        private void testURL_internal(string theUserAgent,
                                      string theUserFriendlyUserAgent,
                                      string businessName,
                                      string url)
        {
            processURLs = true;

            websiteStatus ws;

            ws = new websiteStatus(businessName, url);
            ws.setUserAgent(theUserAgent);
            ws.setUserFriendlyUserAgent(theUserFriendlyUserAgent);

            lock (urlsToProcess)
            {
                urlsToProcess.Enqueue(ws);
                numToProcess++;
            }
        }

        /// <summary>
        /// Helper method to strip any http:// or https:// transport declaration from a url.
        /// </summary>
        /// <param name="url">The url to test.</param>
        /// <returns>URL without transport declaration.</returns>
        private string removeWebTransportDeclaration(string url)
        {
            string test;

            test = url.ToLower();
            if (test.StartsWith("http://"))
            {
                test = url.Substring(7);
            }
            else if (test.StartsWith("https://"))
            {
                test = url.Substring(8);
            }
            else
            {
                test = url;
            }

            return test;
        }

        /// <summary>
        /// Timer callback. Used to update the display (the underlying data can be modified independently)
        /// </summary>
        /// <param name="myObject">The Timer sending this timer event.</param>
        /// <param name="myEventArgs">The event arguments.</param>
        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            listViewResults.Invalidate(false);
            listViewResults.Update();
        }

        /// <summary>
        /// Helper method to display the results of querying a web page on the list.
        /// </summary>
        /// <param name="status">Website test results.</param>
        /// <param name="updateDisplay">If true statistics and column headers will be calculated.</param>
        private void addDisplayResult(websiteStatus status,
                                      bool          updateDisplay)
        {
            ListViewItem item;

            // try to reduce flickering (doesn't really work)

            listViewResults.BeginUpdate();

            // create items to add to a row in the listview
            // even though these sub item values won't be rendered (because it's owner drawn) we need to add them otherwise they don't get drawn

            item = new ListViewItem(status.getBusinessName());
            item.SubItems.Add("");                                      // the ID
            item.SubItems.Add(status.getStatusAsString());
            item.SubItems.Add(status.getIsSecureAsString());
            item.SubItems.Add(status.getSecurityProtocolAsString());
            item.SubItems.Add(status.getHttpRedirectsToHttpsAsString());
            item.SubItems.Add(status.getSSLPolicyErrorStatusAsString());
            item.SubItems.Add(status.getSecureContentAsString());
            item.SubItems.Add(status.getSecurityHeaderGrade());
            item.SubItems.Add(status.getURL());
            item.SubItems.Add(status.getResponseURL());
            item.SubItems.Add(status.getUserFriendlyUserAgent());
            item.SubItems.Add(status.getUserAgent());

            item.Tag = status;                                          // make sure the ownerdrawn code knows what it's working on

            item = listViewResults.Items.Add(item);

            // make the current result visible, tell the control to update itself

            listViewResults.EnsureVisible(listViewResults.Items.Count - 1);
            if (updateDisplay)
            {
                calculateStatistics(status);
                updateColumnHeaders();
            }

            listViewResults.Invalidate(false);
            listViewResults.Update();
            listViewResults.EndUpdate();

            updateProgressBar();

            status.setIndex(item.Index);
        }

        /// <summary>
        /// Helper method to display the results of querying a web page on the list.
        /// </summary>
        /// <param name="status">Website test results.</param>
        private void updateResult(websiteStatus status)
        {
            int n;

            n = listViewResults.Items.Count;
            if (n == 0)
                return;

            // try to reduce flickering (doesn't really work)

            listViewResults.BeginUpdate();

            // make the current result visible, tell the control to update itself

            calculateStatistics(status);
            updateColumnHeaders();

            listViewResults.Invalidate(false);
            listViewResults.Update();
            listViewResults.EndUpdate();
        }

        /// <summary>
        /// Helper method to calculate statistics used in the column headers
        /// </summary>
        /// <param name="status">Website test results.</param>
        private void calculateStatistics(websiteStatus status)
        {
            // check if any stats need updating and if they do, is a column resize required?

            if (status.getHasHTTPTransport())
            {
                httpTransportCount++;
            }

            if (status.getFetchFailed())
            {
                failedToFetchCount++;
            }

            if (!status.getFetchFailed())
            {
                if (!status.getIsSecure())
                {
                    isNotSecureCount++;
                }
                else
                {
                    isSecureCount++;
                }
            }

            if (!status.getRedirectsHTTPtoHTTPS())
            {
                doesNotRedirectCount++;
            }

            if (status.getHasSSLPolicyError())
                sslPolicyErrorCount++;

            if (status.getHasMixedContent() == websiteStatus.mixedContentStatus.PRESENT)
            {
                mixedContentCount++;
            }

            if (!status.getFetchFailed() &&
                status.hasImperfectSecurityHeaders())
            {
                imperfectSecurityHeaderCount++;
            }

            if (status.getHasBadCertificate())
            {
                badCertificateCount++;
            }
        }

        /// <summary>
        /// Helper method to calculate the text for each column header
        /// </summary>
        private void updateColumnHeaders()
        {
            // check if any stats need updating and if they do, is a column resize required?

            chTransport.Text = string.Format("Transport. Http ({0}) Secure ({1}) Insecure ({2}) Unknown ({3})", httpTransportCount, isSecureCount, isNotSecureCount, failedToFetchCount);

            chSecure.Text = string.Format("Secure ({0} / {1})", isNotSecureCount, failedToFetchCount);

            chRedirect.Text = string.Format("Redirect ({0})", doesNotRedirectCount);

            chSecureContent.Text = string.Format("Secure Content ({0})", mixedContentCount);

            chSecurityHeaders.Text = string.Format("Security Headers ({0})", imperfectSecurityHeaderCount);

            chSSLCertificate.Text = string.Format("SSL Certificate ({0})", badCertificateCount);

            listViewResults.AutoResizeBothColumns();
        }

        /// <summary>
        /// Helper method to update the progress bar. 
        /// </summary>
        /// <remarks>        
        /// /// For some reason although the values set are correct the display never updates to the 100% mark.
        /// If you test the same code outside of this code it works correctly, but it doesn't seem to work correctly when called via BeginInvoke(). Weird.
        /// </remarks>
        private void updateProgressBar()
        {
            urlProgressBar.Maximum = numToProcess;
            urlProgressBar.Value = numProcessed;
            urlProgressBar.Refresh();
        }

        /// <summary>
        /// Helper method to reset all the statistics and the display to their default state
        /// </summary>
        private void reset()
        {
            processURLs = false;

            lock (urlsToProcess)
                urlsToProcess.Clear();

            numToProcess = 0;
            numProcessed = 0;
            updateProgressBar();

            listViewResults.Items.Clear();

            httpTransportCount = 0;
            isNotSecureCount = 0;
            isSecureCount = 0;
            failedToFetchCount = 0;
            doesNotRedirectCount = 0;
            sslPolicyErrorCount = 0;
            mixedContentCount = 0;
            imperfectSecurityHeaderCount = 0;
            badCertificateCount = 0;

            chTransport.Text = "Transport";
            chSecure.Text = "Secure";
            chRedirect.Text = "Redirect";
            chSecureContent.Text = "Secure Content";
            chSecurityHeaders.Text = "Security Headers";
            chSSLCertificate.Text = "SSL Certificate";

            websiteStatus.resetMonotonicID();
        }

        /// <summary>
        /// Helper method to test all banks.
        /// </summary>
        private void testBanks()
        {
            foreach (var website in testWebsites.getListOfBanks())
                testURL(website.businessName, website.url);
        }

        /// <summary>
        /// Helper method to test all building societies.
        /// </summary>
        private void testBuildingSocieties()
        {
            foreach (var website in testWebsites.getListOfBuildingSocieties())
                testURL(website.businessName, website.url);
        }

        /// <summary>
        /// Helper method to test all wealth managers.
        /// </summary>
        private void testWealthManagers()
        {
            foreach (var website in testWebsites.getListOfWealthManagers())
                testURL(website.businessName, website.url);
        }

        /// <summary>
        /// Helper method to test all currency exchanges.
        /// </summary>
        private void testCurrencyExchanges()
        {
            foreach (var website in testWebsites.getListOfCurrencyExchanges())
                testURL(website.businessName, website.url);
        }

        /// <summary>
        /// Helper method to test all casinos.
        /// </summary>
        private void testCasinos()
        {
            foreach (var website in testWebsites.getListOfCasinos())
                testURL(website.businessName, website.url);
        }

        /// <summary>
        /// Helper method to test all ecommerce companies.
        /// </summary>
        private void testEcommerceCompanies()
        {
            foreach (var website in testWebsites.getListOfEcommerceCompanies())
                testURL(website.businessName, website.url);
        }

        /// <summary>
        /// Helper method to test all healthcare companies.
        /// </summary>
        private void testHealthcareCompanies()
        {
            foreach (var website in testWebsites.getListOfHealthcareCompanies())
                testURL(website.businessName, website.url);
        }

        /// <summary>
        /// Helper method to test all pension funds.
        /// </summary>
        private void testPensionFunds()
        {
            foreach (var website in testWebsites.getListOfPensionFunds())
                testURL(website.businessName, website.url);
        }

        /// <summary>
        /// Helper method to test all insurance companies.
        /// </summary>
        private void testInsuranceCompanies()
        {
            foreach (var website in testWebsites.getListOfInsuranceCompanies())
                testURL(website.businessName, website.url);
        }

        /// <summary>
        /// Helper method to test all stock traders.
        /// </summary>
        private void testStockTraders()
        {
            foreach (var website in testWebsites.getListOfStockTraders())
                testURL(website.businessName, website.url);
        }

        /// <summary>
        /// Helper method to enable/disable display various items on the display.
        /// </summary>
        /// <param name="enable">true - enable controls. false - disable controls.</param>
        private void enableControls(bool enable)
        {
            listViewResults.Enabled = enable;
        }

        /// <summary>
        /// Event handler for Form close.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FormClosedHandler(object sender, FormClosedEventArgs e)
        {
            reset();
            runURLProcessingThread = false;
        }

        /// <summary>
        /// Event handler for File->Exit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reset();
            runURLProcessingThread = false;
            Application.Exit();
        }

        /// <summary>
        /// Event handler for Test->Test a URL....
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enableControls(false);

            TestAURLDialog dlg = new TestAURLDialog();

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (dlg.getURL().Length > 0)
                    testURL(dlg.getName(), dlg.getURL());
            }

            enableControls(true);
        }

        /// <summary>
        /// Event handler for item selection on User Agent menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userAgentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userAgent = sender.ToString();

            foreach (var item in userAgentToolStripMenuItem.DropDownItems)
            {
                if (item.GetType() == typeof(ToolStripMenuItem))
                {
                    ((ToolStripMenuItem)item).Checked = false;
                }
            }

            ((ToolStripMenuItem)sender).Checked = true;
        }

        /// <summary>
        /// Event handler for Tests menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string testName = sender.ToString();

            reset();

            enableControls(false);

            if (testName == "All Tests")
            {
                testBanks();
                testBuildingSocieties();
                testCasinos();
                testCurrencyExchanges();
                testEcommerceCompanies();
                testHealthcareCompanies();
                testInsuranceCompanies();
                testPensionFunds();
                testStockTraders();
                testWealthManagers();
            }
            else if (testName == "Banks")
            {
                testBanks();
            }
            else if (testName == "Building Societies")
            {
                testBuildingSocieties();
            }
            else if (testName == "Casinos")
            {
                testCasinos();
            }
            else if (testName == "Currency Exchanges")
            {
                testCurrencyExchanges();
            }
            else if (testName == "Ecommerce Vendors")
            {
                testEcommerceCompanies();
            }
            else if (testName == "Healthcare Companies")
            {
                testHealthcareCompanies();
            }
            else if (testName == "Insurance Companies")
            {
                testInsuranceCompanies();
            }
            else if (testName == "Pension Funds")
            {
                testPensionFunds();
            }
            else if (testName == "Stock Traders")
            {
                testStockTraders();
            }
            else if (testName == "Wealth Managers")
            {
                testWealthManagers();
            }

            enableControls(true);
        }

        /// <summary>
        /// Helper method to return the websiteStatus information connected to a give line in the display
        /// </summary>
        /// <param name="row">Row that we wish to inspect</param>
        /// <returns>Website status.</returns>
        private websiteStatus getStatusFromRow(int row)
        {
            if (listViewResults.Items.Count > 0 &&
                row < listViewResults.Items.Count)
            {
                ListViewItem item = listViewResults.Items[row];

                if (item != null)
                {
                    return (websiteStatus)item.Tag;
                }
            }

            return null;
        }

        /// <summary>
        /// Event handler for clicking on an item in the list. This will cause the security header and security certificate
        /// details to be displayed in the appropriate text boxes below the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewResults.SelectedItems.Count > 0)
            {
                ListViewItem item = listViewResults.SelectedItems[listViewResults.SelectedItems.Count - 1];

                if (item != null)
                {
                    websiteStatus ws = (websiteStatus)item.Tag;

                    displaySecurityHeaders(ws);
                    displaySecurityCertificate(ws);
                    displayMixedContent(ws);
                }
            }
        }

        /// <summary>
        /// Helper method to concatenate two security header options for display.
        /// </summary>
        /// <param name="headerName">Name of the header value.</param>
        /// <param name="headerValue">Value of the header value.</param>
        /// <returns>Concatenated result.</returns>
        private string addSecurityHeaderOption(string headerName,
                                               string headerValue)
        {
            string str;

            str = headerName;
            str += ": ";
            if (headerValue != null)
            {
                str += headerValue;
            }
            else
            {
                str += "<missing>";
            }

            return str;
        }

        /// <summary>
        /// Helper method to display security header values for the selected web page.
        /// </summary>
        /// <param name="ws">The web page status to display.</param>
        private void displaySecurityHeaders(websiteStatus ws)
        {
            securityHeadersInfo shi = ws.getSecurityHeaders();
            string str = "";

            if (shi != null)
            {
                str += addSecurityHeaderOption("Grade", shi.getSecurityHeaderGrade());
                str += "\r\n";

                str += addSecurityHeaderOption("X-Content-Type-Options", shi.getSecurityHeaderXContentTypeOptions());
                str += "\r\n";
                str += addSecurityHeaderOption("X-XSS-Protection", shi.getSecurityHeaderXXSSProtection());
                str += "\r\n";
                str += addSecurityHeaderOption("X-Frame-Options", shi.getSecurityHeaderXFrameOptions());
                str += "\r\n";
                str += addSecurityHeaderOption("Strict-Transport-Security", shi.getSecurityHeaderStrictTransportSecurity());
                str += "\r\n";
                str += addSecurityHeaderOption("Content-Security-Policy", shi.getSecurityHeaderContentSecurityPolicy());
                str += "\r\n";
                str += addSecurityHeaderOption("Referrer-Policy", shi.getSecurityHeaderReferrerPolicy());
                str += "\r\n";
            }

            textBoxSecurityHeader.Text = str;
            textBoxSecurityHeader.Invalidate();
            textBoxSecurityHeader.Update();
        }

        /// <summary>
        /// Helper method to display security certificate data for the selected web page.
        /// </summary>
        /// <param name="ws">The web page status to display.</param>
        private void displaySecurityCertificate(websiteStatus ws)
        {
            securityCertificateInfo sci = ws.getSecurityCertificateInfo();
            string str = "";

            if (sci != null)
            {
                str = "Bad certificate: ";
                if (sci.getIsBadCertificate())
                    str += "Yes";
                else
                    str += "No";
                str += "\r\n";

                str += addSecurityHeaderOption("SSL status", sci.getSSLStatus());
                str += "\r\n";

                str += addSecurityHeaderOption("Security protocol", sci.getSecurityProtocolAsString());
                str += "\r\n";

                str += addSecurityHeaderOption("Chain revocation flag", sci.getChainRevocationFlag());
                str += "\r\n";

                str += addSecurityHeaderOption("Chain revocation mode", sci.getChainRevocationMode());
                str += "\r\n";

                str += addSecurityHeaderOption("Chain verification flags", sci.getChainVerificationFlags());
                str += "\r\n";

                str += addSecurityHeaderOption("Chain verification time", sci.getChainVerificationTime());
                str += "\r\n";

                str += addSecurityHeaderOption("Chain status length", sci.getChainStatusLength());
                str += "\r\n";

                str += addSecurityHeaderOption("Chain application policy count", sci.getChainApplicationPolicyCount());
                str += "\r\n";

                str += addSecurityHeaderOption("Chain certificate policy count", sci.getChainCertificatePolicyCount());
                str += "\r\n";

                str += addSecurityHeaderOption("Chain elements synchronized", sci.getChainElementsSynchronized());
                str += "\r\n";

                str += "Chain information:\r\n";

                int i, n;

                n = sci.getNumChains();
                for (i = 0; i < n; i++)
                {
                    securityCertificateChain scc;

                    scc = sci.getChain(i);
                    if (scc != null)
                    {
                        str += String.Format("\tChain {0}\r\n", i);

                        str += addSecurityHeaderOption("\tIssuer", scc.getIssuer());
                        str += "\r\n";

                        str += addSecurityHeaderOption("\tExpiry date", scc.getValidUntil());
                        str += "\r\n";

                        str += "\tIs Valid: ";
                        if (scc.getIsValid())
                            str += "Yes";
                        else
                            str += "No";
                        str += "\r\n";

                        str += addSecurityHeaderOption("\tInformation", scc.getInformation());
                        str += "\r\n";

                        int nc, c;

                        nc = scc.getNumChains();
                        for (c = 0; c < nc; c++)
                        {
                            str += addSecurityHeaderOption("\t\tStatus", scc.getChainStatus(c));
                            str += "\r\n";

                            str += addSecurityHeaderOption("\t\tInformation", scc.getChainInformation(c));
                            str += "\r\n";
                        }
                    }
                }
            }

            textBoxSecurityCertificate.Text = str;
            textBoxSecurityCertificate.Invalidate();
            textBoxSecurityCertificate.Update();
        }

        /// <summary>
        /// Helper method to display mixed content values for the selected web page.
        /// </summary>
        /// <param name="ws">The web page status to display.</param>
        private void displayMixedContent(websiteStatus ws)
        {
            string str = "";
            int i, n;

            n = ws.getNumMixedContentURLs();
            for (i = 0; i < n; i++)
            {
                str += ws.getMixedContentURL(i);
                str += "\r\n";
            }

            textBoxMixedContentURLs.Text = str;
            textBoxMixedContentURLs.Invalidate();
            textBoxMixedContentURLs.Update();
        }

        /// <summary>
        /// Event handler for File->Clear Results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reset();
        }

        /// <summary>
        /// Event handler for File->Export HTML Results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [STAThread]
        private void exportAsHTMLToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveExportHTMLDialog = new SaveFileDialog();
            saveExportHTMLDialog.Filter = "HTML (*.html)|*.html";
            saveExportHTMLDialog.Title = "Export to HTML";
            saveExportHTMLDialog.FileName = "testResults.html";
            saveExportHTMLDialog.ShowHelp = false;
            saveExportHTMLDialog.CreatePrompt = false;
            saveExportHTMLDialog.OverwritePrompt = false;   // prevent the "Do you want to overwrite this prompt?" from being shown - somehow when enabled this causes the app to hang. Weird.
            saveExportHTMLDialog.RestoreDirectory = true;
            if (saveExportHTMLDialog.ShowDialog() == DialogResult.OK)
            {
                // If the file name is not an empty string open it for saving.  

                if (saveExportHTMLDialog.FileName != "")
                {
                    System.IO.FileStream fs;

                    fs = (System.IO.FileStream)saveExportHTMLDialog.OpenFile();

                    using (System.IO.StreamWriter file =
                               new System.IO.StreamWriter(fs))
                    {
                        // for each row in the list view, export it as HTML

                        int i, n;

                        n = listViewResults.Items.Count;
                        for (i = 0; i < n; i++)
                        {
                            exportRowAsHTML(file, i);
                        }
                    }
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// Event handler for File->Export XML Results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [STAThread]
        private void exportAsXMLToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveExportXMLDialog = new SaveFileDialog();
            saveExportXMLDialog.Filter = "XML (*.xml)|*.xml";
            saveExportXMLDialog.Title = "Export to XML";
            saveExportXMLDialog.FileName = "testResults.xml";
            saveExportXMLDialog.ShowHelp = false;
            saveExportXMLDialog.CreatePrompt = false;
            saveExportXMLDialog.OverwritePrompt = false;    // prevent the "Do you want to overwrite this prompt?" from being shown - somehow when enabled this causes the app to hang. Weird.
            saveExportXMLDialog.RestoreDirectory = true;
            if (saveExportXMLDialog.ShowDialog() == DialogResult.OK)
            {
                // If the file name is not an empty string open it for saving.  

                if (saveExportXMLDialog.FileName != "")
                {
                    System.IO.FileStream fs;

                    fs = (System.IO.FileStream)saveExportXMLDialog.OpenFile();

                    using (System.IO.StreamWriter file =
                               new System.IO.StreamWriter(fs))
                    {
                        // for each row in the list view, export it as XML

                        int i, n;

                        n = listViewResults.Items.Count;
                        for (i = 0; i < n; i++)
                        {
                            exportRowAsXML(file, i);
                        }
                    }
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// Helper method for exporting data as HTML.
        /// </summary>
        /// <param name="file">StreamWrite to write to.</param>
        /// <param name="row">Row in the listview that is being exported.</param>
        private void exportRowAsHTML(System.IO.StreamWriter file,
                                     int row)
        {
            string htmlFragment;
            int c, numColumns;

            htmlFragment = "<tr>";

            numColumns = listViewResults.Columns.Count;
            for (c = 0; c < numColumns; c++)
            {
                htmlFragment += "<td>";
                htmlFragment += listViewResults.Items[row].SubItems[c].Text;
                htmlFragment += "</td>";
            }
            htmlFragment += "</tr>";
            htmlFragment += "\r\n";

            file.WriteLine(htmlFragment);
        }

        /// <summary>
        /// Helper method for exporting data as XML.
        /// </summary>
        /// <param name="file">StreamWrite to write to.</param>
        /// <param name="row">Row in the listview that is being exported.</param>
        private void exportRowAsXML(System.IO.StreamWriter file,
                                    int row)
        {
            string xmlFragment;

            xmlFragment = "<test>";
            xmlFragment += getXMLRowColumn(row, 0, "<Name>");
            xmlFragment += getXMLRowColumn(row, 1, "<Transport>");
            xmlFragment += getXMLRowColumn(row, 2, "<Secure>");
            xmlFragment += getXMLRowColumn(row, 3, "<Protocol>");
            xmlFragment += getXMLRowColumn(row, 4, "<Redirect>");
            xmlFragment += getXMLRowColumn(row, 5, "<SSL>");
            xmlFragment += getXMLRowColumn(row, 6, "<SecureContent>");
            xmlFragment += getXMLRowColumn(row, 7, "<URL>");
            xmlFragment += getXMLRowColumn(row, 8, "<UserFriendlyUserAgent>");
            xmlFragment += getXMLRowColumn(row, 9, "<UserAgent>");
            xmlFragment += "</test>";
            xmlFragment += "\r\n";

            file.WriteLine(xmlFragment);
        }

        /// <summary>
        /// Helper method for exporting a list item to XML.
        /// </summary>
        /// <param name="row">Row in the listview.</param>
        /// <param name="column">Column in the listview.</param>
        /// <param name="tag">The tag.</param>
        /// <returns>A string containing the data</returns>
        private string getXMLRowColumn(int row,
                                       int column,
                                       string tag)
        {
            string xml;

            xml = tag;
            xml += listViewResults.Items[row].SubItems[column].Text;
            tag = tag.Insert(1, "/");
            xml += tag;

            return xml;
        }

        /// <summary>
        /// Event handler for File->Save.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [STAThread]
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveResultsDialog = new SaveFileDialog();
            saveResultsDialog.Filter = "Is My Login Secure (*.ismls)|*.ismls";
            saveResultsDialog.Title = "Save Test Results";
            saveResultsDialog.ShowHelp = false;
            saveResultsDialog.CreatePrompt = false;
            saveResultsDialog.OverwritePrompt = false;  // prevent the "Do you want to overwrite this prompt?" from being shown - somehow when enabled this causes the app to hang. Weird.
            saveResultsDialog.RestoreDirectory = true;

            if (saveResultsDialog.ShowDialog() == DialogResult.OK)
            {
                // If the file name is not an empty string open it for saving.  

                if (saveResultsDialog.FileName != "")
                {
                    System.IO.File.Delete(saveResultsDialog.FileName);

                    BinaryFormatter formatter = new BinaryFormatter();
                    System.IO.FileStream fs;

                    fs = (System.IO.FileStream)saveResultsDialog.OpenFile();
                    if (fs != null)
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(fs))
                        {
                            // for each row in the list view, get the websiteStatus result for that row and save it

                            int i, n;

                            n = listViewResults.Items.Count;
                            formatter.Serialize(fs, n);

                            for (i = 0; i < n; i++)
                            {
                                websiteStatus ws;

                                ws = (websiteStatus)listViewResults.Items[i].Tag;
                                if (ws != null)
                                    formatter.Serialize(fs, ws);
                            }
                        }
                        fs.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Event handler for File->Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog loadResultsDialog;

            loadResultsDialog = new OpenFileDialog();
            loadResultsDialog.Filter = "Is My Login Secure (*.ismls)|*.ismls";
            loadResultsDialog.Title = "Load Test Results";
            if (loadResultsDialog.ShowDialog() == DialogResult.OK)
            {
                reset();

                // If the file name is not an empty string open it for reading.  

                if (loadResultsDialog.FileName != "")
                {
                    System.IO.FileStream fs;

                    fs = (System.IO.FileStream)loadResultsDialog.OpenFile();
                    if (fs != null)
                    {
                        using (System.IO.StreamReader file = new System.IO.StreamReader(fs))
                        {
                            int n;
                            BinaryFormatter formatter = new BinaryFormatter();
                            
                            n = (int)formatter.Deserialize(fs);

                            // setup progress bar

                            numToProcess = n;
                            numProcessed = 0;

                            // process the data we've just loaded

                            for (int i = 0; i < n; i++)
                            {
                                websiteStatus ws;

                                numProcessed++;         // move progress bar along

                                ws = (websiteStatus)formatter.Deserialize(fs);
                                addDisplayResult(ws, true);
                            }

                            // ensure the data display is up to date

                            updateColumnHeaders();
                            listViewResults.Invalidate(false);
                            listViewResults.Update();
                        }
                        fs.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Event handler for File->Load URLs from a file...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadURLsFromAFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog loadResultsDialog = new OpenFileDialog();
            loadResultsDialog.Filter = "CSV File (*.csv)|*.csv";
            loadResultsDialog.Title = "Load a list of URLs";
            if (loadResultsDialog.ShowDialog() == DialogResult.OK)
            {
                // If the file name is not an empty string open it for reading.  

                if (loadResultsDialog.FileName != "")
                {
                    using (TextFieldParser parser = new TextFieldParser(loadResultsDialog.FileName))
                    {
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");
                        while (!parser.EndOfData)
                        {
                            // one row at a time

                            string[] fields = parser.ReadFields();

                            // 2 columns per row
                            // 1st column is bank name
                            // 2nd column is bank URL to test
                            // 3rd will typically be empty if 2 columns in the file (for some reason it returns 3 columns here)

                            if (fields.Length >= 2)
                            {
                                testURL(fields[0], fields[1]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Event handler for Help->Help Manual...
        /// Opens the PDF help manual (most likely in your web browser)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName;

            fileName = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            fileName += "\\isMyLoginSecure.pdf";

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = fileName;
            psi.UseShellExecute = true;
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.Verb = "open";
            Process.Start(psi);
        }

        /// <summary>
        /// Event handler for Help->About Is My Login Secure...
        /// Displays the About Box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutIsMyLoginSecureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox dlg = new AboutBox();

            dlg.ShowDialog(this);
        }

        /// <summary>
        /// Event handler for form resized.
        /// This is required because of the Table view not drawing labels properly sometimes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resizeEventHandler(Object sender, EventArgs e)
        {
            Invalidate(false);
            Update();
        }

        /// <summary>
        /// Helper method: Get the registry key for storing data under
        /// </summary>
        /// <returns>String identify the part of the Registry.</returns>
        private string getRegistryKey()
        {
            const string userRoot = "HKEY_CURRENT_USER";
            const string subkey = "IsMyLoginSecure";
            const string keyName = userRoot + "\\" + subkey;

            return keyName;
        }
        
        /// <summary>
        /// Get the first time running status
        /// </summary>
        /// <returns>true if this is the first time this software has run. false otherwise.</returns>
        private bool GetIsFirstTimeRunning()
        {
            string keyName = getRegistryKey();
            object firstTime;

            firstTime = Registry.GetValue(keyName, "FirstTimeRun", 0);
            if (firstTime != null)
            {
                bool b;

                if (Boolean.TryParse((string)firstTime, out b))
                    return b;
            }

            // no value found, assume not present, therefore first time

            return true;
        }

        /// <summary>
        /// Set if the first time running status
        /// </summary>
        /// <param name="b">First time running status.</param>
        private void SetFirstTimeRunning(bool b)
        {
            string keyName = getRegistryKey();

            Registry.SetValue(keyName, "FirstTimeRun", b);
        }

        /// <summary>
        /// Event handler for Help->Software Licence.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void softwareLicenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LicenceDialog dlg = new LicenceDialog();

            dlg.ShowDialog();
        }

        /// <summary>
        /// Determine which row is selected. For use with context menu functions.
        /// </summary>
        /// <returns>Index of the first selected row, else -1</returns>
        private int getSelectedRow()
        {
            int row = -1;

            if (listViewResults.SelectedItems.Count > 0)
            {
                row = listViewResults.SelectedItems[0].Index;
            }

            return row;
        }

        /// <summary>
        /// Event handler for Context Menu->Open Test Url...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void openTestUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = getSelectedRow();

            if (row != -1)
            {
                string url = listViewResults.Items[row].SubItems[chTestURL.DisplayIndex].Text;

                Process.Start(url);
            }
        }

        /// <summary>
        /// Get text from row/column in the data, with optional prefix and postfix for each cell.
        /// </summary>
        /// <param name="row">Row to query</param>
        /// <param name="columns">Columns in each row to get text for</param>
        /// <param name="prefix">Optional prefix to place before text from each row/column. Ignored if null</param>
        /// <param name="postfix">Optional postfix to place after text from each row/column. Ignored if null</param>
        /// <param name="dontAddPostFixToLastColumn">If true the postfix is ignored for the last column</param>
        /// <returns>The text for the row/column plus optional prefix and postfix</returns>
        private string getTextFromRowInResults(int row,
                                               int[] columns,
                                               string prefix,  // can be null,
                                               string postfix, // can be null
                                               bool dontAddPostFixToLastColumn)
        {
            string text;

            text = "";
            if (row != -1)
            {
                int c, numColumns;

                numColumns = columns.Length;
                for (c = 0; c < numColumns; c++)
                {
                    // add prefix

                    if (prefix != null)
                    {
                        text += prefix;
                    }

                    // get the text for the row/column

                    text += listViewResults.Items[row].SubItems[columns[c]].Text;

                    // add postfix

                    if (postfix != null)
                    {
                        if (dontAddPostFixToLastColumn)
                        {
                            if (c != numColumns - 1)
                                text += postfix;
                        }
                        else
                            text += postfix;
                    }
                }
            }

            return text;
        }

        /// <summary>
        /// Get HTML link from row/column in the data, with optional prefix and postfix for each cell.
        /// </summary>
        /// <param name="row">Row to query</param>
        /// <param name="columns">Columns in each row to get text for</param>
        /// <param name="prefix">Optional prefix to place before text from each row/column. Ignored if null</param>
        /// <param name="postfix">Optional postfix to place after text from each row/column. Ignored if null</param>
        /// <returns>The text for the row/column plus optional prefix and postfix</returns>
        private string getLinkFromRowInResults(int row,
                                               int[] columns,
                                               string prefix,
                                               string postfix)
        {
            string text;

            text = "";
            if (row != -1)
            {
                int c, numColumns;

                numColumns = columns.Length;
                for (c = 0; c < numColumns; c++)
                {
                    // add prefix

                    if (prefix != null)
                    {
                        text += prefix;
                    }

                    // open <a> tag

                    text += "<a href=\"";
                    text += listViewResults.Items[row].SubItems[columns[c]].Text;
                    text += "\">";

                    // the text inside the <a> tag

                    text += listViewResults.Items[row].SubItems[columns[c]].Text;

                    // close the <a> tag

                    text += "</a>";

                    // add postfix

                    if (postfix != null)
                    {
                        text += postfix;
                    }
                }
            }

            return text;
        }

        /// <summary>
        /// Event handler for Context Menu->Copy All
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = getSelectedRow();

            if (row != -1)
            {
                string text;
                int c, numColumns;
                int[] columns;

                // get indexes for all columns

                numColumns = listViewResults.Columns.Count;
                columns = new int[numColumns];
                for (c = 0; c < numColumns; c++)
                {
                    columns[c] = c;
                }

                // get the text for all columns

                text = getTextFromRowInResults(row, columns, null, ", ", true);
                Clipboard.SetText(text);
            }
        }

        /// <summary>
        /// Event handler for Context Menu->Copy Security
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void copySecurityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = getSelectedRow();

            if (row != -1)
            {
                string text;
                int[] columns;
                int c = 0;

                columns = new int[10];

                // get the indexes for specific columns

                columns[c++] = chName.DisplayIndex;
                columns[c++] = chTransport.DisplayIndex;
                columns[c++] = chSecure.DisplayIndex;
                columns[c++] = chProtocol.DisplayIndex;
                columns[c++] = chRedirect.DisplayIndex;
                columns[c++] = chSSLCertificate.DisplayIndex;
                columns[c++] = chSecureContent.DisplayIndex;
                columns[c++] = chSecurityHeaders.DisplayIndex;
                columns[c++] = chTestURL.DisplayIndex;
                columns[c++] = chActualURL.DisplayIndex;

                // get text for each of the columns

                text = getTextFromRowInResults(row, columns, null, ", ", true);
                Clipboard.SetText(text);
            }
        }

        /// <summary>
        /// Event handler for Context Menu->Copy All As HTML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void copyAllAsHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = getSelectedRow();

            if (row != -1)
            {
                string text;
                int c, numColumns;
                int[] columns;

                // get the indexes of all columns

                numColumns = listViewResults.Columns.Count;
                columns = new int[numColumns];
                for (c = 0; c < numColumns; c++)
                {
                    columns[c] = c;
                }

                // get text for each of the columns

                text = getTextFromRowInResults(row, columns, "<td>", "</td>", false);
                Clipboard.SetText(text);
            }
        }

        /// <summary>
        /// Event handler for Context Menu->Copy Security As HTML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void copySecurityAsHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = getSelectedRow();

            if (row != -1)
            {
                string text;
                int[] columns;
                int c = 0;

                columns = new int[10];

                // get the indexes for specific columns

                columns[c++] = chName.DisplayIndex;
                columns[c++] = chTransport.DisplayIndex;
                columns[c++] = chSecure.DisplayIndex;
                columns[c++] = chProtocol.DisplayIndex;
                columns[c++] = chRedirect.DisplayIndex;
                columns[c++] = chSSLCertificate.DisplayIndex;
                columns[c++] = chSecureContent.DisplayIndex;
                columns[c++] = chSecurityHeaders.DisplayIndex;
                columns[c++] = chTestURL.DisplayIndex;
                columns[c++] = chActualURL.DisplayIndex;

                // get text for each of the columns

                text = getTextFromRowInResults(row, columns, "<td>", "</td>", false);
                Clipboard.SetText(text);
            }
        }

        /// <summary>
        /// Event handler for File->Export->Export Certificate Errors as HTML...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void exportCertificateErrorsAsHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveExportHTMLDialog = new SaveFileDialog();
            saveExportHTMLDialog.Filter = "HTML (*.html)|*.html";
            saveExportHTMLDialog.Title = "Export to Security Certificate Errors to HTML";
            saveExportHTMLDialog.FileName = "certificateErrors.html";
            saveExportHTMLDialog.ShowHelp = false;
            saveExportHTMLDialog.CreatePrompt = false;
            saveExportHTMLDialog.OverwritePrompt = false;   // prevent the "Do you want to overwrite this prompt?" from being shown - somehow when enabled this causes the app to hang. Weird.
            saveExportHTMLDialog.RestoreDirectory = true;
            if (saveExportHTMLDialog.ShowDialog() == DialogResult.OK)
            {
                // If the file name is not an empty string open it for saving.  

                if (saveExportHTMLDialog.FileName != "")
                {
                    System.IO.FileStream fs;

                    fs = (System.IO.FileStream)saveExportHTMLDialog.OpenFile();

                    using (System.IO.StreamWriter file =
                               new System.IO.StreamWriter(fs))
                    {
                        // for each row in the list view, get the websiteStatus result for that row and process it

                        int row, numRows;

                        numRows = listViewResults.Items.Count;
                        for (row = 0; row < numRows; row++)
                        {
                            websiteStatus ws;

                            ws = getStatusFromRow(row);
                            if (ws != null)
                            {
                                if (ws.getHasBadCertificate())
                                {
                                    // only report data if the certificate had problems

                                    string text;
                                    int[] columns;
                                    int c = 0;

                                    columns = new int[2];

                                    // start row

                                    text = "<tr>";

                                    // name and Security Certificate status

                                    columns[c++] = chName.DisplayIndex;
                                    columns[c++] = chSSLCertificate.DisplayIndex;

                                    text += getTextFromRowInResults(row, columns, "<td>", "</td>", false);

                                    // URL

                                    columns = new int[1];
                                    columns[0] = chTestURL.DisplayIndex;
                                    text += getLinkFromRowInResults(row, columns, "<td>", "</td>");

                                    // end row

                                    text += "</tr>";
                                    text += "\r\n";

                                    file.WriteLine(text);
                                }
                            }
                        }
                    }
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// Event handler for File->Export->Export Insecure Content Errors as HTML...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void exportInsecureContentErrorsAsHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveExportHTMLDialog = new SaveFileDialog();
            saveExportHTMLDialog.Filter = "HTML (*.html)|*.html";
            saveExportHTMLDialog.Title = "Export to Insecure Content Errors to HTML";
            saveExportHTMLDialog.FileName = "insecureContentErrors.html";
            saveExportHTMLDialog.ShowHelp = false;
            saveExportHTMLDialog.CreatePrompt = false;
            saveExportHTMLDialog.OverwritePrompt = false;   // prevent the "Do you want to overwrite this prompt?" from being shown - somehow when enabled this causes the app to hang. Weird.
            saveExportHTMLDialog.RestoreDirectory = true;
            if (saveExportHTMLDialog.ShowDialog() == DialogResult.OK)
            {
                // If the file name is not an empty string open it for saving.  

                if (saveExportHTMLDialog.FileName != "")
                {
                    System.IO.FileStream fs;

                    fs = (System.IO.FileStream)saveExportHTMLDialog.OpenFile();

                    using (System.IO.StreamWriter file =
                               new System.IO.StreamWriter(fs))
                    {
                        // for each row in the list view, get the websiteStatus result for that row and process it

                        int row, numRows;

                        numRows = listViewResults.Items.Count;
                        for (row = 0; row < numRows; row++)
                        {
                            websiteStatus ws;

                            ws = getStatusFromRow(row);
                            if (ws != null)
                            {
                                if (ws.getNumMixedContentURLs() > 0)
                                {
                                    // only report if there were some insecure URLs

                                    string text;
                                    int[] columns;

                                    // start row

                                    text = "<tr>";

                                    // name

                                    columns = new int[1];

                                    columns[0] = chName.DisplayIndex;
                                    text += getTextFromRowInResults(row, columns, "<td>", "</td>", false);

                                    // URL

                                    columns[0] = chTestURL.DisplayIndex;
                                    text += getLinkFromRowInResults(row, columns, "<td>", "</td>");

                                    // last table cell lists all insecure content URLs,
                                    // each one separated by a line break <br>

                                    int n, i;

                                    text += "<td>";
                                    n = ws.getNumMixedContentURLs();
                                    for (i = 0; i < n; i++)
                                    {
                                        text += "<a href=\"";
                                        text += ws.getMixedContentURL(i);
                                        text += "\">";
                                        text += ws.getMixedContentURL(i);
                                        text += "</a>";

                                        if (i < (n - 1))
                                            text += "<br>";
                                    }

                                    text += "</td>";

                                    // end of row

                                    text += "</tr>";
                                    text += "\r\n";

                                    file.WriteLine(text);
                                }
                            }
                        }
                    }
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// Event handler for File->Export->Export Security Headers as HTML...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void exportSecurityHeadersAsHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveExportHTMLDialog = new SaveFileDialog();
            saveExportHTMLDialog.Filter = "HTML (*.html)|*.html";
            saveExportHTMLDialog.Title = "Export to Security Header Errors to HTML";
            saveExportHTMLDialog.FileName = "securityHeaderErrors.html";
            saveExportHTMLDialog.ShowHelp = false;
            saveExportHTMLDialog.CreatePrompt = false;
            saveExportHTMLDialog.OverwritePrompt = false;   // prevent the "Do you want to overwrite this prompt?" from being shown - somehow when enabled this causes the app to hang. Weird.
            saveExportHTMLDialog.RestoreDirectory = true;
            if (saveExportHTMLDialog.ShowDialog() == DialogResult.OK)
            {
                // If the file name is not an empty string open it for saving.  

                if (saveExportHTMLDialog.FileName != "")
                {
                    System.IO.FileStream fs;

                    fs = (System.IO.FileStream)saveExportHTMLDialog.OpenFile();

                    using (System.IO.StreamWriter file =
                               new System.IO.StreamWriter(fs))
                    {
                        // for each row in the list view, get the websiteStatus result for that row and process it

                        int row, numRows;

                        numRows = listViewResults.Items.Count;
                        for (row = 0; row < numRows; row++)
                        {
                            websiteStatus ws;

                            ws = getStatusFromRow(row);
                            if (ws != null)
                            {
                                if (!ws.getFetchFailed() &&
                                    !ws.getIsSecurityHeaderGradePerfect())
                                {
                                    // only report if the fetch succeeded and the security header grade isn't perfect (not 100%)

                                    string text;
                                    int[] columns;
                                    int c = 0;

                                    columns = new int[1];

                                    // start row

                                    text = "<tr>";

                                    // Name

                                    columns[c++] = chName.DisplayIndex;
                                    text += getTextFromRowInResults(row, columns, "<td>", "</td>", false);

                                    // security header grade

                                    text += "<td>";
                                    text += ws.getSecurityHeaderGrade();

                                    // each missing security option

                                    List<string> missingOptions;

                                    missingOptions = ws.getMissingSecurityHeaderOptions();
                                    if (missingOptions.Count > 0)
                                    {
                                        foreach (string option in missingOptions)
                                        {
                                            text += "<br>";
                                            text += option;
                                        }
                                    }
                                    text += "</td>";

                                    // URL

                                    columns = new int[1];

                                    columns[0] = chTestURL.DisplayIndex;
                                    text += getLinkFromRowInResults(row, columns, "<td>", "</td>");

                                    // end row

                                    text += "</tr>";
                                    text += "\r\n";

                                    file.WriteLine(text);
                                }
                            }
                        }
                    }
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// Event handler for File->Export->Export Failure to query banks as HTML...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void exportFailureToQueryBanksAsHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveExportHTMLDialog = new SaveFileDialog();
            saveExportHTMLDialog.Filter = "HTML (*.html)|*.html";
            saveExportHTMLDialog.Title = "Export to Failure to Query to HTML";
            saveExportHTMLDialog.FileName = "failedToQueryErrors.html";
            saveExportHTMLDialog.ShowHelp = false;
            saveExportHTMLDialog.CreatePrompt = false;
            saveExportHTMLDialog.OverwritePrompt = false;   // prevent the "Do you want to overwrite this prompt?" from being shown - somehow when enabled this causes the app to hang. Weird.
            saveExportHTMLDialog.RestoreDirectory = true;
            if (saveExportHTMLDialog.ShowDialog() == DialogResult.OK)
            {
                // If the file name is not an empty string open it for saving.  

                if (saveExportHTMLDialog.FileName != "")
                {
                    System.IO.FileStream fs;

                    fs = (System.IO.FileStream)saveExportHTMLDialog.OpenFile();

                    using (System.IO.StreamWriter file =
                               new System.IO.StreamWriter(fs))
                    {
                        // for each row in the list view, get the websiteStatus result for that row and process it

                        int row, numRows;

                        numRows = listViewResults.Items.Count;
                        for (row = 0; row < numRows; row++)
                        {
                            websiteStatus ws;

                            ws = getStatusFromRow(row);
                            if (ws != null)
                            {
                                if (ws.getFetchFailed())
                                {
                                    // only report if unable to fetch the web page

                                    string text;
                                    int[] columns;
                                    int c = 0;

                                    columns = new int[2];

                                    // start row

                                    text = "<tr>";

                                    // Name and Transport (fetch error)

                                    columns[c++] = chName.DisplayIndex;
                                    columns[c++] = chTransport.DisplayIndex;

                                    text += getTextFromRowInResults(row, columns, "<td>", "</td>", false);

                                    // URL

                                    columns = new int[1];
                                    columns[0] = chTestURL.DisplayIndex;
                                    text += getLinkFromRowInResults(row, columns, "<td>", "</td>");

                                    // end row

                                    text += "</tr>";
                                    text += "\r\n";

                                    file.WriteLine(text);
                                }
                            }
                        }
                    }
                    fs.Close();
                }
            }
        }
    }
}
