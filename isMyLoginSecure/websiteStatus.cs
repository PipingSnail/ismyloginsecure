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
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.IO.Compression;
using HtmlAgilityPack;

namespace isMyLoginSecure
{
    /// <summary>
    /// This is the class to use to perform queries about a website's security status.
    /// </summary>
    /// <remarks>This page provides everything you need to query a website URL about it's security posture. 
    /// Methods are provided to return status as "Yes" and "No", arrange consistently so that "Yes" always 
    /// means "good" and "No" always means "bad". This has been done so that we can have a consistent visual
    /// theme on the website (very good for behavioural economics - more convincing).</remarks>
    [Serializable]
    public class websiteStatus
    {
        /// <summary>
        /// The status of a website query.
        /// </summary>
        public enum securityStatus
        {
            /// <summary>Has not tried to fetch a web page</summary>
            SS_NOT_DETERMINED,
            /// <summary>Querying http web page</summary>
            SS_HTTP_QUERY_IN_PROGRESS,
            /// <summary>Querying https web page</summary>
            SS_HTTPS_QUERY_IN_PROGRESS,
            /// <summary>Querying for mixed content on the page</summary>
            SS_MIXED_CONTENT_IN_PROGRESS,
            /// <summary>Couldn't fetch the page</summary>
            SS_HTTP_EXCEPTION,
            /// <summary>Fetched the page but status code not OK</summary>
            SS_HTTP_FETCH_FAIL,
            /// <summary>Fetched web page as http (https failed)</summary>
            SS_HTTP,
            /// <summary>Fetched web page as http and as https</summary>
            SS_HTTP_AND_HTTPS,
            /// <summary>Fetched web page as https (http failed or http redirected to https)</summary>
            SS_HTTPS
        };

        /// <summary>
        /// Human readable strings for each status in securityStatus enumeration.
        /// </summary>
        [NonSerialized]
        private static readonly string[] statusStrings =
        {
            "Not determined",
            "Querying http://",
            "Querying https://",
            "Querying mixed content",
            "Web server didn't respond",
            "Could not fetch web page",
            "http",
            "http & https",
            "https"
        };

        /// <summary>
        /// The mixed content status of a web page.
        /// </summary>
        public enum mixedContentStatus
        {
            /// <summary>Have not yet tried to determine the mixed content status.</summary>
            NOT_DETERMINED,
            /// <summary>Failed to parse the fetched web page.</summary>
            UNABLE_TO_PARSE,
            /// <summary>No mixed content.</summary>
            NONE,
            /// <summary>Mixed content is present.</summary>
            PRESENT,
        };

        /// <summary>
        /// Index of the webstatus in the list view.
        /// </summary>
        [NonSerialized]
        private int index = -1;             // -1 if not set

        /// <summary>
        /// monotonicID. Arbitrary ID assigned by isMyLoginSecure to give each website a unique value.
        /// </summary>
        [NonSerialized]
        static private int monotonicID = 0;

        /// <summary>
        /// ID. Arbitrary ID assigned by isMyLoginSecure to give each website a unique value.
        /// </summary>
        private int id;

        /// <summary>
        /// Human readable user agent. For example: "Chrome"
        /// </summary>
        private string userFriendlyuserAgent;

        /// <summary>
        /// Actual user agent. For example: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.78 Safari/537.36"
        /// </summary>
        private string userAgent;

        /// <summary>
        /// Business name. For example: "National Westminter Bank Plc".
        /// </summary>
        private string businessName;

        /// <summary>
        /// Website URL. For example: https://www.natwest.com
        /// </summary>
        private string url;

        /// <summary>
        /// The actual URL that was served. For example, you requested http://digital.ulsterbank.co.uk but were returned https://www.ulsterbank.co.uk/
        /// </summary>
        private string responseUrl;

        /// <summary>
        /// The security status. Defaults to securityStatus.SS_NOT_DETERMINED.
        /// </summary>
        private securityStatus status = securityStatus.SS_NOT_DETERMINED;

        /// <summary>
        /// Does the website have a bad security certificate.
        /// </summary>
        /// <remarks>This is only valid for websites loaded via https.</remarks>
        private bool badCertificate = false;

        /// <summary>
        /// Did attempts to load via http result in a secure (https) website being returned?
        /// </summary>
        private bool httpRedirectsToHttps = false;

        /// <summary>
        /// If an exception was thrown attempting to load a website, what was the exception message.
        /// </summary>
        /// <remarks>This is a useful value for debugging website load failures. Use Telerik Fiddler to
        /// inspect a real web browser's headers to tweak them for when this class tried to load the same page.</remarks>
        private string exceptionMessage = "";

        /// <summary>
        /// Information about the security headers for this web page.
        /// </summary>
        private securityHeadersInfo securityHeader = new securityHeadersInfo();

        /// <summary>
        /// Information about the security certificate for this web page.
        /// </summary>
        private securityCertificateInfo securityCertificate = new securityCertificateInfo();

        /// <summary>
        /// Information about the security certificate that we are querying for this web page.
        /// </summary>
        private securityCertificateInfo securityCertificateQuery = new securityCertificateInfo();

        /// <summary>
        /// Does the website contain mixed content. 
        /// </summary>
        /// <remarks>This is only valid for websites loaded via https.</remarks>
        private mixedContentStatus mixedContent = mixedContentStatus.NOT_DETERMINED;

        /// <summary>
        /// If the web page has mixed content, this collection stores the URLs that are not secure.
        /// </summary>
        private StringCollection mixedContentURLs = new StringCollection();

        /// <summary>
        /// Constructor. Sets the object to the default state with a business name and website.
        /// </summary>
        /// <remarks>This does not query the website.</remarks>
        /// <param name="theBusinessName">The name of the business. Example: "National Westminster Bank Plc"</param>
        /// <param name="theURL">The website. Example: www.natwest.com</param>
        public websiteStatus(string theBusinessName,
                             string theURL)
        {
            reset();
            businessName = theBusinessName;
            url = theURL;

            id = ++monotonicID;
        }

        /// <summary>
        /// Resets data to the default state. 
        /// </summary>
        public void reset()
        {
            id = 0;
            url = "";
            status = securityStatus.SS_NOT_DETERMINED;
            mixedContent = mixedContentStatus.NOT_DETERMINED;
            badCertificate = false;
            httpRedirectsToHttps = false;

            securityHeader.reset();
            securityCertificate.reset();
            securityCertificateQuery.reset();

            resetMixedContentURLs();
        }

        /// <summary>
        /// Reset the mixed content URL data to the default state.
        /// </summary>
        static public void resetMonotonicID()
        {
            monotonicID = 0;
        }

        /// <summary>
        /// Reset the mixed content URL data to the default state.
        /// </summary>
        private void resetMixedContentURLs()
        {
            mixedContentURLs.Clear();
        }

        /// <summary>
        /// Query the number of mixed content URLs.
        /// </summary>
        /// <returns>The number of mixed content URLs.</returns>
        public int getNumMixedContentURLs()
        {
            return mixedContentURLs.Count;
        }

        /// <summary>
        /// Query a mixed content URL.
        /// </summary>
        /// <param name="index">Index into the list of mixed content URLs. Index &gt;= 0 and &lt; getNumMixedContentURLs().</param>
        /// <returns>URL. Caution will return null for out of range requests.</returns>
        public string getMixedContentURL(int index)
        {
            if (index >= 0 && index < mixedContentURLs.Count)
                return mixedContentURLs[index];

            return null;
        }

        /// <summary>
        /// Set the URL to be tested.
        /// </summary>
        /// <param name="p_url">The URL of the website.</param>
        public void setURL(string p_url)
        {
            url = p_url;
        }

        /// <summary>
        /// Query the ID of this website.
        /// </summary>
        /// <returns>The website id.</returns>

        public int getID()
        {
            return id;
        }

        /// <summary>
        /// Query the URL that has been tested (that was requested from the webserver).
        /// </summary>
        /// <returns>The queried website URL.</returns>
        public string getURL()
        {
            return url;
        }

        /// <summary>
        /// Query the URL that was returned from the webserver.
        /// </summary>
        /// <returns>The returned website URL.</returns>
        public string getResponseURL()
        {
            return responseUrl;
        }

        /// <summary>
        /// Set the security status of the website.
        /// </summary>
        /// <param name="s">The security status.</param>
        public void setStatus(securityStatus s)
        {
            status = s;
        }

        /// <summary>
        /// Get the security status of the website.
        /// </summary>
        /// <returns>The security status.</returns>
        public securityStatus getStatus()
        {
            return status;
        }

        /// <summary>
        /// Set the mixed content status of the website.
        /// </summary>
        /// <param name="mcs">mixed content status</param>
        public void setHasMixedContent(mixedContentStatus mcs)
        {
            mixedContent = mcs;
        }

        /// <summary>
        /// Get the mixed content status of the website.
        /// </summary>
        /// <returns>mixed content status</returns>
        public mixedContentStatus getHasMixedContent()
        {
            return mixedContent;
        }

        /// <summary>
        /// Set if the website has a bad security certificate.
        /// </summary>
        /// <param name="bad">true - bad security certificate. false - good security certificate.</param>
        public void setHasBadCertificate(bool bad)
        {
            badCertificate = bad;
        }

        /// <summary>
        /// Query if the website has a bad security certificate.
        /// </summary>
        /// <returns>true - bad security certificate. false - good security certificate.</returns>
        public bool getHasBadCertificate()
        {
            return badCertificate;
        }

        /// <summary>
        /// Set if the website redirects from http to https.
        /// </summary>
        /// <param name="redirect">true - redirects from http to http. false - does not redirect from http to https.</param>
        public void setRedirectsHTTPtoHTTPS(bool redirect)
        {
            httpRedirectsToHttps = redirect;
        }

        /// <summary>
        /// Query if the website redirects from http to https.
        /// </summary>
        /// <returns>true - redirects from http to http. false - does not redirect from http to https.</returns>
        public bool getRedirectsHTTPtoHTTPS()
        {
            return httpRedirectsToHttps;
        }

        /// <summary>
        /// Sets the result of a request to fetch a http web page.
        /// </summary>
        /// <remarks>This method should be called even when requesting a http web page results in a https web page.</remarks>
        /// <param name="response">The response from the web server</param>
        public void addHTTPResult(System.Net.HttpWebResponse response)
        {
            if (isStatusCodeOK(response))
            {
                // this could have redirected to https, need to check that

                if (response.ResponseUri.Scheme == "https")
                {
                    status = securityStatus.SS_HTTPS;
                    setRedirectsHTTPtoHTTPS(true);
                }
                else
                {
                    status = securityStatus.SS_HTTP;
                }
            }
            else
            {
                status = securityStatus.SS_HTTP_FETCH_FAIL;
            }

            securityHeader.processSecurityHeaders(response);
        }

        /// <summary>
        /// Sets the result of a failure to request to fetch a http web page.
        /// </summary>
        /// <param name="exception">The exception thrown when trying to fetch the web page.</param>
        public void addHTTPResult(System.Net.WebException exception)
        {
            status = securityStatus.SS_HTTP_EXCEPTION;
            exceptionMessage = exception.Message;
        }

        /// <summary>
        /// Sets the result of a request to fetch a https web page.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="response">The response from the web server</param>
        public void addHTTPSResult(System.Net.HttpWebResponse response)
        {
            if (isStatusCodeOK(response))
            {
                if (status == securityStatus.SS_HTTP)
                    status = securityStatus.SS_HTTP_AND_HTTPS;
                else
                    status = securityStatus.SS_HTTPS;
            }
            else
            {
                status = securityStatus.SS_HTTP_FETCH_FAIL;
            }

            securityHeader.processSecurityHeaders(response);
        }

        /// <summary>
        /// Sets the result of a failure to request to fetch a https web page.
        /// </summary>
        /// <param name="exception">The exception thrown when trying to fetch the web page.</param>
        public void addHTTPSResult(System.Net.WebException exception)
        {
            if (status != securityStatus.SS_HTTP)
            {
                status = securityStatus.SS_HTTP_EXCEPTION;
                exceptionMessage = exception.Message;
            }
        }

        /// <summary>
        /// Helper method for determining if we have breaking parse errors, or just badly formatted HTML
        /// </summary>
        /// <param name="doc">HtmlAgility pack document</param>
        /// <returns>true - all ok. false - couldn't read the document.</returns>
        private bool checkAcceptableParseErrors(HtmlDocument doc)
        {
            bool noErrors = true;    // assume OK

            if (doc.ParseErrors != null &&
                doc.ParseErrors.Count() > 0)
            {
                foreach(var error in doc.ParseErrors)
                {
                    if (error.Code == HtmlParseErrorCode.CharsetMismatch)
                        noErrors = false;
                }
            }

            return noErrors;
        }

        /// <summary>
        /// Analyse a web page to determine if it has mixed content.
        /// </summary>
        /// <remarks>This method makes use of HTMLAgility Page to make parsing the HTML simpler.. https://www.nuget.org/packages/HtmlAgilityPack/</remarks>
        /// <param name="webPageContent">The web page to be analysed. Supplied as a page of text, not a URL.</param>
        /// <param name="securityProtocol">The security protocol we wish to test.</param>

        public void analyseForMixedContent(string webPageContent,
                                           System.Net.SecurityProtocolType securityProtocol)
        {
            resetMixedContentURLs();

            if (status == securityStatus.SS_HTTPS ||
                status == securityStatus.SS_HTTP_AND_HTTPS)
            {
                // modify status while we are querying the mixed content

                securityStatus backupStatus = status;

                status = securityStatus.SS_MIXED_CONTENT_IN_PROGRESS;

                // parse the loaded page

                HtmlDocument doc = new HtmlDocument();

                doc.OptionFixNestedTags = true; // so that a missing </a> doesn't break stuff, etc
                doc.LoadHtml(webPageContent);

                if (checkAcceptableParseErrors(doc))
                {
                    mixedContent = mixedContentStatus.NONE;

                    //evaluateNodes(doc, "//a", "href");          // anchors, these don't load content

                    evaluateNodes(doc, "//img", "src");         // images
                    evaluateNodes(doc, "//source", "srcset");   // images
                    evaluateNodes(doc, "//iframe", "src");      // windows
                    evaluateNodes(doc, "//script", "src");      // javascript

                    //evaluateNodes(doc, "//html", "xmlns");      // should we check this one? No
                    //evaluateNodes(doc, "//html", "prefix");     // should we check this one? No
                    evaluateNodes(doc, "//base", "href");
                    evaluateNodes(doc, "//link", "href");       // CSS, etc

                    int n;

                    n = mixedContentURLs.Count;
                    if (n > 0)
                    {
                        // here we need to check if https is being redirected to https AND if so are the mixed content URLs pointing to the redirected domain or somewhere else?

                        if (httpRedirectsToHttps)
                        {
                            string testOriginalUrl;

                            testOriginalUrl = "http://" + url;
                            if (testOriginalUrl.EndsWith("/"))
                                testOriginalUrl = testOriginalUrl.Remove(testOriginalUrl.Length - 1);

                            // loop through checking for insecure content URLs that have been securely redirected

                            for (int i = 0; i < n; i++)
                            {
                                string mixedUrl = mixedContentURLs[i];

                                if (String.Equals(testOriginalUrl, mixedUrl, StringComparison.OrdinalIgnoreCase))       // URLs are not case sensitive
                                {
                                    // this URL would be insecure but the redirect makes it safe, so we can remove it from the list of insecure content URLs

                                    mixedContentURLs.RemoveAt(i);
                                    i--;
                                    n--;
                                }
                                else
                                {
                                    if (doesThisURLRedirectToHttps(mixedUrl, securityProtocol))
                                    {
                                        // this URL would be insecure but the redirect makes it safe, so we can remove it from the list of insecure content URLs

                                        mixedContentURLs.RemoveAt(i);
                                        i--;
                                        n--;
                                    }
                                    else
                                    {
                                        mixedContent = mixedContentStatus.PRESENT;
                                    }
                                }
                            }
                        }
                        else
                        {
                            mixedContent = mixedContentStatus.PRESENT;
                        }
                    }
                }
                else
                {
                    mixedContent = mixedContentStatus.UNABLE_TO_PARSE;
                }

                // restore status

                status = backupStatus;
            }
        }

        /// <summary>
        /// Helper method to check if an insecure URL redirects securely (used for analysing mixed content URLs)
        /// </summary>
        /// <remarks>We assume we're on a secure page, thus if we find http:// then that is mixed content.</remarks>
        /// <param name="testUrl">The url to check.</param>
        /// <param name="securityProtocol">The security protocol we wish to test.</param>
        /// <returns>True if the URL exists and is redirected, false otherwise. No attempt to load or parse the web page is made.</returns>
        private bool doesThisURLRedirectToHttps(string testUrl,
                                                System.Net.SecurityProtocolType securityProtocol)
        {
            bool ok = false;
            bool redirected = false;

            // setup security certificate data harvesting callback

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            ServicePointManager.SecurityProtocol = securityProtocol;

            // try to get the data, if it fails set a status based on the exception

            securityCertificateQuery.forceHasCertificate();
            securityCertificateQuery.setSecurityProtocol(securityProtocol);

            try
            {
                Uri uri = new Uri(testUrl);
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);

                // https://stackoverflow.com/questions/26654369/why-im-getting-exception-too-many-automatic-redirections-were-attempted-on-web
                // set a cookie container to handle any redirects the website may do

                webRequest.CookieContainer = new CookieContainer();

                // https://stackoverflow.com/questions/16735042/the-remote-server-returned-an-error-403-forbidden
                // if we don't set these some websites will return 403 Forbidden.

                webRequest.Referer = @"http://www.ismyloginsecure.com/";
                webRequest.UseDefaultCredentials = true;
                //webRequest.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

                // the default timeout of 100 seconds is too long, set it to 20 seconds (DNS timeout is 15 seconds)

                webRequest.Timeout = 30 * 1000;

                // if we don't set a user agent, some of the websites will fail to respond, giving a 403 Forbidden error
                // pretend to be Chrome (the version is important, too low, like 41 will work for most sites and fail for others)

                webRequest.UserAgent = userAgent;

                // also, by spying with Telerik Fiddler I find that I possibly also need these headers

                webRequest.Accept = "text / html,application / xhtml + xml,application / xml; q = 0.9,image / webp,image / apng,*/*;q=0.8";

                webRequest.Headers.Add("Cache-Control", "max-age=0");
                webRequest.Headers.Add("Upgrade-Insecure-Requests", "1");
                webRequest.Headers.Add("Accept-Encoding", "gzip");  // if we include ", deflate" we'll need to modify readWebPageStream()
                webRequest.Headers.Add("Accept-Encoding", "identity");
                webRequest.Headers.Add("Accept-Language", "en-GB,en-US;q=0.9,en;q=0.8");

                // try to get the web page

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                // check if we got a valid response, and did it redirect to https?

                redirected = false;
                if (isStatusCodeOK(webResponse))
                {
                    // this could have redirected to https, need to check that

                    if (webResponse.ResponseUri.Scheme == "https")
                    {
                        redirected = true;
                    }
                }

                ok = true;
            }
            catch (System.Net.WebException)
            {
                // eat the exception, we don't care why it failed
            }

            if (ok)
                return redirected;

            return ok;
        }

        /// <summary>
        /// Helper method to evaluate the nodes of a web page, looking for specific tags with specific attributes.
        /// </summary>
        /// <remarks>This method makes use of HTMLAgility Page to make parsing the HTML simpler.. https://www.nuget.org/packages/HtmlAgilityPack/.
        /// Any matching URLs found in the attributes of matching tags are checked for mixed content and if found added to the list of mixed content URLs.</remarks>
        /// <param name="doc">The document representing the web page.</param>
        /// <param name="htmlTagName">The name of the tag we want to look for. Example: "//img".</param>
        /// <param name="attributeName">The name of the attribute we want to look for in the tag. Example: "src".</param>
        private void evaluateNodes(HtmlDocument doc,
                                   string htmlTagName,
                                   string attributeName)
        {
            HtmlNodeCollection nodes;

            nodes = doc.DocumentNode.SelectNodes(htmlTagName);
            if (nodes != null)
            {
                int n = nodes.Count();
                int i;

                for (i = 0; i < n; i++)
                {
                    IEnumerable<HtmlAttribute> attribs = nodes[i].ChildAttributes(attributeName);

                    if (attribs != null)
                    {
                        foreach (var attr in attribs)
                        {
                            evaluateMixedContentURL(attr.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Helper method to check if a URL is mixed content or not.
        /// </summary>
        /// <remarks>We assume we're on a secure page, thus if we find http:// then that is mixed content.</remarks>
        /// <param name="url">The url to check.</param>
        private void evaluateMixedContentURL(string url)
        {
            url = url.ToLower();

            if (url.StartsWith("http://"))
            {
                // not secure

                mixedContentURLs.Add(url);
            }
            else if (url.StartsWith("https://"))
            {
                // secure
            }
            else
            {
                // will load using whatever status the <base> tag specifies
            }
        }

        /// <summary>
        /// Convenience debugger function for debugging mixed content.
        /// </summary>
        private void printMixedContentNodes()
        {
            foreach (var node in mixedContentURLs)
            {
                Console.Write("mixed content>> ");
                Console.Write(node);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Helper method to determine if a particular web response is good or bad (can we load the web page?).
        /// </summary>
        /// <param name="response">The web response.</param>
        /// <returns>true - good response. false - bad response.</returns>
        private bool isStatusCodeOK(System.Net.HttpWebResponse response)
        {
            bool ok = false;

            switch (response.StatusCode)
            {
                //case HttpStatusCode.Accepted:                     // 200
                case HttpStatusCode.OK:                             // 200
                case HttpStatusCode.Created:                        // 201
                case HttpStatusCode.NoContent:                      // 204
                case HttpStatusCode.NonAuthoritativeInformation:    // 203
                case HttpStatusCode.ResetContent:                   // 205
                case HttpStatusCode.PartialContent:                 // 206
                case HttpStatusCode.Ambiguous:                      // 300
                //case HttpStatusCode.MultipleChoices:              // 300
                case HttpStatusCode.Moved:                          // 301
                //case HttpStatusCode.MovedPermanently:             // 301
                case HttpStatusCode.Found:                          // 302
                //case HttpStatusCode.Redirect:                     // 302
                case HttpStatusCode.RedirectMethod:                 // 303
                //case HttpStatusCode.SeeOther:                     // 303
                case HttpStatusCode.UseProxy:                       // 305
                case HttpStatusCode.RedirectKeepVerb:               // 307
                //case HttpStatusCode.TemporaryRedirect:            // 307
                    ok = true;
                    break;

                default:
                    break;
            }

            return ok;
        }

        /// <summary>
        /// Get the web page security status as human readable string.
        /// </summary>
        /// <remarks>If the web page was not successfully returned, this method returns the message from the exception that was thrown.</remarks>
        /// <returns>Human readable security status</returns>
        public string getStatusAsString()
        {
            if (status == securityStatus.SS_HTTP_EXCEPTION)
                return exceptionMessage;

            return statusStrings[(int)status];
        }

        /// <summary>
        /// Returns if the page failed to be fetched from the web server
        /// </summary>
        /// <returns>true - web page failed to be fetched from webserver. false - web page was fetched from web server.</returns>
        public bool getFetchFailed()
        {
            return status == securityStatus.SS_HTTP_FETCH_FAIL ||
                   status == securityStatus.SS_HTTP_EXCEPTION;
        }

        /// <summary>
        /// Returns if the page should be regarded as secure.
        /// </summary>
        /// <remarks>A secure page uses only https to load, has a valid certificate and does not have mixed content. 
        /// Pages that load via http and https should not be considered secure.</remarks>
        /// <returns>true - web page is secure. false - web page is not secure.</returns>
        public bool getIsSecure()
        {
            if (!badCertificate)
            {
                if (mixedContent != mixedContentStatus.PRESENT)
                {
                    // if there is no mixed content, then if status is HTTPs we are secure

                    return status == securityStatus.SS_HTTPS;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns if the page should be regarded as secure.
        /// </summary>
        /// <remarks>A secure page uses only https to load, has a valid certificate and does not have mixed content. 
        /// Web pages that load via http and https should not be considered secure.</remarks>
        /// <returns>"Yes" - web page is secure. "No" - web page is not secure.</returns>
        public string getIsSecureAsString()
        {
            if (getFetchFailed())
                return "???";

            if (getIsSecure())
                return "Yes";

            return "No";
        }

        /// <summary>
        /// Query if requests to http forms of the web page result in a https web page being returned.
        /// </summary>
        /// <returns>"Yes" - http redirects to https. "No" - http does not redirect to https.</returns>
        public string getHttpRedirectsToHttpsAsString()
        {
            if (httpRedirectsToHttps)
                return "Yes";

            return "No";
        }

        /// <summary>
        /// Does this web page return http versions of itself?
        /// </summary>
        /// <returns>true - Yes, http web pages are supported. No, http web pages are not supported.</returns>
        public bool getHasHTTPTransport()
        {
            if (status == securityStatus.SS_HTTP ||
                status == securityStatus.SS_HTTP_AND_HTTPS)
                return true;

            return false;
        }

        /// <summary>
        /// Does this web page return https versions of itself?
        /// </summary>
        /// <returns>true - Yes, https web pages are supported. No, https web pages are not supported.</returns>
        public bool getHasHTTPSTransport()
        {
            if (status == securityStatus.SS_HTTPS ||
                status == securityStatus.SS_HTTP_AND_HTTPS)
                return true;

            return false;
        }

        /// <summary>
        /// Does this web page have SSL errors?
        /// </summary>
        /// <returns>true - web page has SSL errors. false - web page has no SSL errors.</returns>
        public bool getHasSSLPolicyError()
        {
            bool hasError = false;

            if (status == securityStatus.SS_HTTPS ||
                status == securityStatus.SS_HTTP_AND_HTTPS)
            {
                if (badCertificate)
                {
                    //if (sslPolicyErrors != System.Net.Security.SslPolicyErrors.None)
                        hasError = true;
                }
            }

            return hasError;
        }

        /// <summary>
        /// Query the SSL error as a human readable string.
        /// </summary>
        /// <returns>"-" - doesn't use HTTPS. "Yes" - uses HTTPS with no errors. Otherwise, human readable error string.</returns>
        public string getSSLPolicyErrorStatusAsString()
        {
            string sslStatus = "-";

            //if (status == securityStatus.SS_HTTPS ||
            //    status == securityStatus.SS_HTTP_AND_HTTPS)
            {
                if (badCertificate)
                {
                    sslStatus = securityCertificate.getSSLStatus();
                }
                else
                {
                    // has HTTPS?

                    sslStatus = "Yes";
                }
            }

            return sslStatus;
        }

        /// <summary>
        /// Does this web page have mixed content?
        /// </summary>
        /// <returns>"Yes" - web page has mixed content. "No" - web page does not have mixed content.</returns>
        public string getMixedContentStatusAsString()
        {
            string str = "";

            switch (mixedContent)
            {
                case mixedContentStatus.NOT_DETERMINED:
                case mixedContentStatus.UNABLE_TO_PARSE:
                    str = "-";
                    break;

                case mixedContentStatus.NONE:
                    str = "No";
                    break;

                case mixedContentStatus.PRESENT:
                    str = "Yes";
                    break;
            }

            return str;
        }

        /// <summary>
        /// Does this web page have secure content?
        /// </summary>
        /// <returns>"Yes" - web page has secure content. "No" - web page has mixed content.</returns>
        public string getSecureContentAsString()
        {
            if (status == securityStatus.SS_HTTPS ||
                status == securityStatus.SS_HTTP_AND_HTTPS)
            {
                if (mixedContent == mixedContentStatus.PRESENT)
                    return "No";

                if (mixedContent == mixedContentStatus.NONE)
                    return "Yes";

                return "?";
            }

            return "-";
        }

        /// <summary>
        /// Query the security protocol as a human readable string.
        /// </summary>
        /// <returns>"" - did not fetch web page. Otherwise human readable security protocol. Example: "SSL3".</returns>
        public string getSecurityProtocolAsString()
        {
            string str;

            if (status == securityStatus.SS_HTTPS ||
                status == securityStatus.SS_HTTP_AND_HTTPS)
            {
                return securityCertificate.getSecurityProtocolAsString();
            }
            else
            {
                str = "";
            }

            return str;
        }

        /// <summary>
        /// Get the security protocol we are using to query the web page as a human readable string.
        /// </summary>
        /// <returns>"" - did not fetch web page. Otherwise human readable security protocol. Example: "SSL3".</returns>
        public string getSecurityProtocolQueryAsString()
        {
            return securityCertificateQuery.getSecurityProtocolAsString();
        }
        
        /// <summary>
        /// Query the business name.
        /// </summary>
        /// <returns></returns>
        public string getBusinessName()
        {
            return businessName;
        }

        /// <summary>
        /// Helper method for negotiating the security certificate when fetching a web page.
        /// </summary>
        /// <remarks>This method is needed so that we can accept web pages that have bad certificates etc. 
        /// Without this method we wouldn't be able to harvest security certificate, security certificate chain 
        /// and security policy error information.</remarks>
        /// <param name="sender">The originator of this request</param>
        /// <param name="certification">The security certificate</param>
        /// <param name="chain">The security certificate chain</param>
        /// <param name="sslPolicyErrors">The security policy error status</param>
        /// <returns>true - accept these details. false - reject this certificate.</returns>
        private bool AcceptAllCertifications(object sender,
                                             System.Security.Cryptography.X509Certificates.X509Certificate certification,
                                             System.Security.Cryptography.X509Certificates.X509Chain chain,
                                             System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            securityCertificate.setSSLCertificate(certification);
            securityCertificate.setSSLChain(chain);
            securityCertificate.setSSLPolicyErrorStatus(sslPolicyErrors);

            //if (status == securityStatus.SS_HTTPS ||
            //    status == securityStatus.SS_HTTP_AND_HTTPS)
            {
                badCertificate = securityCertificate.getIsBadCertificate();
            }

            // if you want to accept all requests, regardless of security issues, return true

            return true;

            // comment out previous line to use security
            // in practice for the IsMyLoginSecure use case, we really want this code commented out
            /*
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }

            return false;
            */
        }

        /// <summary>
        /// Test loading a web page over an http:// or https:// connection.
        /// </summary>
        /// <param name="doSecure">true - test https connection. false - test http connection.</param>
        /// <param name="theUserAgent">The user agent we wish to test. Example: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.78 Safari/537.36".</param>
        /// <param name="userFriendlyuserAgent">The human readable user agent we are testing. Example: "Chrome".</param>
        public void testHTTP(bool doSecure,
                             string theUserAgent,
                             string userFriendlyuserAgent)
        {
            string url;

            if (doSecure)
                url = "https://";
            else
                url = "http://";
            url += getURL();

            // keep trying different levels of protocol security until they all fail
            // some sites are still using SSL3. Start at the highest level of security and keep
            // trying until one succeeds or they all fail.

            bool connectFailure = false;

            if (!testHTTP(doSecure, theUserAgent, userFriendlyuserAgent, SecurityProtocolType.Tls12, url, ref connectFailure) && !connectFailure)
            {
                if (!testHTTP(doSecure, theUserAgent, userFriendlyuserAgent, SecurityProtocolType.Tls11, url, ref connectFailure) && !connectFailure)
                {
                    if (!testHTTP(doSecure, theUserAgent, userFriendlyuserAgent, SecurityProtocolType.Tls, url, ref connectFailure) && !connectFailure)
                    {
                        if (!testHTTP(doSecure, theUserAgent, userFriendlyuserAgent, SecurityProtocolType.Ssl3, url, ref connectFailure) && !connectFailure)
                        {
                            // all failed.
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Helper method to decompress any compressed web pages received from web servers.
        /// </summary>
        /// <remarks>Supports gzip (tested) and deflate (untested).
        /// See also https://stackoverflow.com/questions/3722192/how-do-i-use-gzipstream-with-system-io-memorystream </remarks>
        /// <param name="webResponse">Web server header response.</param>
        /// <param name="inputStream">A memory stream holding the raw bytes received for the page.</param>
        /// <returns>The original input stream or a decompressed memory stream.</returns>
        private MemoryStream decompressIfRequired(HttpWebResponse webResponse,
                                                  MemoryStream    inputStream)
        {
            if (webResponse.ContentEncoding == "gzip")
            {
                // if it's gzip encoded, decompress it

                MemoryStream decompressedStream = new MemoryStream();

                using (GZipStream decompressionStream = new GZipStream(inputStream, CompressionMode.Decompress))
                {
                    decompressionStream.CopyTo(decompressedStream);
                }

                return decompressedStream;
            }
            else if (webResponse.ContentEncoding == "deflate")
            {
                // if it's deflate encoded, decompress it

                MemoryStream decompressedStream = new MemoryStream();

                using (DeflateStream decompressionStream = new DeflateStream(inputStream, CompressionMode.Decompress))
                {
                    decompressionStream.CopyTo(decompressedStream);
                }

                return decompressedStream;
            }

            return inputStream;
        }

        /// <summary>
        /// Helper method to read a web page stream. 
        /// </summary>
        /// <remarks>This will read a text stream or a gzipped stream. No support for Deflate at this time as no web page to test against.
        /// A large slice of this code was taken from a sample at https://blogs.msdn.microsoft.com/feroze_daud/2004/03/30/downloading-content-from-the-web-using-different-encodings/ </remarks>
        /// <param name="webResponse">The response from the web server.</param>
        /// <returns>The page content.</returns>
        private string readWebPageStream(HttpWebResponse webResponse)
        {
            // first see if content length header has charset = calue

            String charset = null;
            String ctype = webResponse.Headers["content-type"];
            if (ctype != null)
            {
                int ind = ctype.IndexOf("charset=");
                if (ind != -1)
                {
                    charset = ctype.Substring(ind + 8);
                }
            }

            // save data to a memorystream

            MemoryStream rawdata = new MemoryStream();
            byte[] buffer = new byte[1024];
            Stream rs = webResponse.GetResponseStream();

            int read = rs.Read(buffer, 0, buffer.Length);
            while (read > 0)
            {
                rawdata.Write(buffer, 0, read);
                read = rs.Read(buffer, 0, buffer.Length);
            }

            rs.Close();

            // handle possible compressed response from webserver

            rawdata.Seek(0, SeekOrigin.Begin);
            rawdata = decompressIfRequired(webResponse, rawdata);

            // if ContentType is null, or did not contain charset, we search in body

            if (charset == null)
            {
                MemoryStream ms = rawdata;
                ms.Seek(0, SeekOrigin.Begin);

                StreamReader srr = new StreamReader(ms, Encoding.ASCII);
                String meta = srr.ReadToEnd();

                if (meta != null)
                {
                    int start_ind = meta.IndexOf("charset=");
                    int end_ind = -1;
                    if (start_ind != -1)
                    {
                        end_ind = meta.IndexOf("\"", start_ind);
                        if (end_ind != -1)
                        {
                            int start = start_ind + 8;
                            charset = meta.Substring(start, end_ind - start + 1);
                            charset = charset.TrimEnd(new Char[] { '>', '"' });
                        }
                    }
                }
            }

            // try to create an encoding, handling the various error conditions, falling back to ASCII when failure

            Encoding e = null;

            if (charset == null)
            {
                e = Encoding.GetEncoding(webResponse.CharacterSet);
            }
            else
            {
                try
                {
                    e = Encoding.GetEncoding(charset);
                }
                catch (Exception)
                {
                    // ignore the exception, choose a default encoding

                    e = Encoding.ASCII;
                }
            }

            // final conversion from bytes to string

            rawdata.Seek(0, SeekOrigin.Begin);

            return e.GetString(Encoding.Convert(e, Encoding.UTF8, rawdata.GetBuffer()));
        }

        /// <summary>
        /// Helper method for testing the loading a web page over an http:// or https:// connection.
        /// </summary>
        /// <param name="doSecure">true - test https connection. false - test http connection.</param>
        /// <param name="theUserAgent">The user agent we wish to test. Example: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.78 Safari/537.36".</param>
        /// <param name="theUserFriendlyuserAgent">The human readable user agent we are testing. Example: "Chrome".</param>
        /// <param name="securityProtocol">The security protocol we wish to test.</param>
        /// <param name="url">The url we wish to test, including the http:// or https:// specification.</param>
        /// <param name="connectFailure">Returned as true if unable to connect to the website (long timeout for this failure), false for success and false for any other error.</param>
        /// <returns>true - web page loaded OK. false - web page failed to load.</returns>
        private bool testHTTP(bool doSecure,
                              string theUserAgent,
                              string theUserFriendlyuserAgent,
                              System.Net.SecurityProtocolType securityProtocol,
                              string url,
                              ref bool connectFailure)
        {
            bool ok;

            connectFailure = false;

            if (doSecure)
                status = securityStatus.SS_HTTPS_QUERY_IN_PROGRESS;
            else
                status = securityStatus.SS_HTTP_QUERY_IN_PROGRESS;

            // setup security certificate data harvesting callback

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            ServicePointManager.SecurityProtocol = securityProtocol;

            userAgent = theUserAgent;
            userFriendlyuserAgent = theUserFriendlyuserAgent;

            // try to get the data, if it fails set a status based on the exception

            securityCertificateQuery.forceHasCertificate();
            securityCertificateQuery.setSecurityProtocol(securityProtocol);

            try
            {
                Uri uri = new Uri(url);
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);

                // https://stackoverflow.com/questions/26654369/why-im-getting-exception-too-many-automatic-redirections-were-attempted-on-web
                // set a cookie container to handle any redirects the website may do

                webRequest.CookieContainer = new CookieContainer();

                // https://stackoverflow.com/questions/16735042/the-remote-server-returned-an-error-403-forbidden
                // if we don't set these some websites will return 403 Forbidden.

                webRequest.Referer = @"http://www.ismyloginsecure.com/";
                webRequest.UseDefaultCredentials = true;
                //webRequest.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

                // the default timeout of 100 seconds is too long, set it to 20 seconds (DNS timeout is 15 seconds)

                webRequest.Timeout = 30 * 1000;    

                // if we don't set a user agent, some of the websites will fail to respond, giving a 403 Forbidden error
                // pretend to be Chrome (the version is important, too low, like 41 will work for most sites and fail for others)

                webRequest.UserAgent = theUserAgent;

                // also, by spying with Telerik Fiddler I find that I possibly also need these headers

                webRequest.Accept = "text / html,application / xhtml + xml,application / xml; q = 0.9,image / webp,image / apng,*/*;q=0.8";

                webRequest.Headers.Add("Cache-Control", "max-age=0");
                webRequest.Headers.Add("Upgrade-Insecure-Requests", "1");
                webRequest.Headers.Add("Accept-Encoding", "gzip");  // if we include ", deflate" we'll need to modify readWebPageStream()
                webRequest.Headers.Add("Accept-Encoding", "identity");
                webRequest.Headers.Add("Accept-Language", "en-GB,en-US;q=0.9,en;q=0.8");

                // try to get the web page

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                // log the result

                if (doSecure)
                    addHTTPSResult(webResponse);
                else
                    addHTTPResult(webResponse);

                responseUrl = webResponse.ResponseUri.ToString();

                // read the contents of the web page, then do mixed content analysis

                string content;

                content = readWebPageStream(webResponse);

                analyseForMixedContent(content, securityProtocol);

                // finally record the security protocol we used and set all OK

                securityCertificate.setSecurityProtocol(securityProtocol);
                ok = true;
            }
            catch (System.Net.WebException exception)
            {
                // Log any failure to read this web page.
                // Subsequent failures will overwrite earlier failures. That's OK.
                // A successful web page fetch won't overwrite an earlier failure, but the earlier
                // failure will be ignored. That's OK

                if (doSecure)
                    addHTTPSResult(exception);
                else
                    addHTTPResult(exception);

                connectFailure = (exception.Status == WebExceptionStatus.ConnectFailure);

                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Set the User Agent to be tested.
        /// </summary>
        /// <param name="p_agent">The User Agent visiting the website.</param>
        public void setUserAgent(string p_agent)
        {
            userAgent = p_agent;
        }

        /// <summary>
        /// Query the user agent string used to fetch this web page.
        /// </summary>
        /// <returns>The user agent string. Example: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.78 Safari/537.36".</returns>
        public string getUserAgent()
        {
            return userAgent;
        }

        /// <summary>
        /// Set the User Agent to be tested.
        /// </summary>
        /// <param name="p_userFriendlyuserAgent">The User Agent visiting the website.</param>
        public void setUserFriendlyUserAgent(string p_userFriendlyuserAgent)
        {
            userFriendlyuserAgent = p_userFriendlyuserAgent;
        }

        /// <summary>
        /// Query the human readable user agent used to fetch this web page.
        /// </summary>
        /// <returns>The user agent. Example: "Chrome".</returns>
        public string getUserFriendlyUserAgent()
        {
            return userFriendlyuserAgent;
        }

        /// <summary>
        /// Determines if all security headers have been provided by this web page.
        /// </summary>
        /// <returns>true - not all security headers have been provided for this web page. false - all security headers have been provided for this web page.</returns>
        public bool hasImperfectSecurityHeaders()
        {
            return securityHeader.hasImperfectSecurityHeaders();
        }

        /// <summary>
        /// Returns a grade from 0..100% indicating the security header status.
        /// </summary>
        /// <returns>Security header grade</returns>
        public string getSecurityHeaderGrade()
        {
            return securityHeader.getSecurityHeaderGrade();
        }

        /// <summary>
        /// Get the value of the X-Content-Type-Options security header.
        /// </summary>
        /// <remarks>An returned empty string means the header was supplied but had no content. A null returned string means the header was not supplied.</remarks>
        /// <returns>The X-Content-Type-Options security header. Caution, can return null object.</returns>
        public string getSecurityHeaderXContentTypeOptions()
        {
            return securityHeader.getSecurityHeaderXContentTypeOptions();
        }

        /// <summary>
        /// Get the value of the X-XSS-Protection security header.
        /// </summary>
        /// <remarks>An returned empty string means the header was supplied but had no content. A null returned string means the header was not supplied.</remarks>
        /// <returns>The X-XSS-Protection security header. Caution, can return null object.</returns>
        public string getSecurityHeaderXXSSProtection()
        {
            return securityHeader.getSecurityHeaderXXSSProtection();
        }

        /// <summary>
        /// Get the value of the X-Frame-Options security header.
        /// </summary>
        /// <remarks>An returned empty string means the header was supplied but had no content. A null returned string means the header was not supplied.</remarks>
        /// <returns>The X-Frame-Options security header. Caution, can return null object.</returns>
        public string getSecurityHeaderXFrameOptions()
        {
            return securityHeader.getSecurityHeaderXFrameOptions();
        }

        /// <summary>
        /// Get the value of the Strict-Transport-Security security header.
        /// </summary>
        /// <remarks>An returned empty string means the header was supplied but had no content. A null returned string means the header was not supplied.</remarks>
        /// <returns>The Strict-Transport-Security security header. Caution, can return null object.</returns>
        public string getSecurityHeaderStrictTransportSecurity()
        {
            return securityHeader.getSecurityHeaderStrictTransportSecurity();
        }

        /// <summary>
        /// Get the value of the Content-Security-Policy security header.
        /// </summary>
        /// <remarks>An returned empty string means the header was supplied but had no content. A null returned string means the header was not supplied.</remarks>
        /// <returns>The Content-Security-Policy security header. Caution, can return null object.</returns>
        public string getSecurityHeaderContentSecurityPolicy()
        {
            return securityHeader.getSecurityHeaderContentSecurityPolicy();
        }

        /// <summary>
        /// Get the value of the Referrer-Policy security header.
        /// </summary>
        /// <remarks>An returned empty string means the header was supplied but had no content. A null returned string means the header was not supplied.</remarks>
        /// <returns>The Referrer-Policy security header. Caution, can return null object.</returns>
        public string getSecurityHeaderReferrerPolicy()
        {
            return securityHeader.getSecurityHeaderReferrerPolicy();
        }

        /// <summary>
        /// Return the security header object.
        /// </summary>
        /// <returns>Security header.</returns>
        public securityHeadersInfo getSecurityHeaders()
        {
            return securityHeader;
        }

        /// <summary>
        /// Return the security certificate object.
        /// </summary>
        /// <returns>Security certificate info.</returns>
        public securityCertificateInfo getSecurityCertificateInfo()
        {
            return securityCertificate;
        }

        /// <summary>
        /// Set the index into the list view
        /// </summary>
        /// <param>Index into the list view.</param>
        public void setIndex(int    i)
        {
            index = i;
        }

        /// <summary>
        /// Return the index into the list view
        /// </summary>
        /// <returns>Index into the list view.</returns>
        public int getIndex()
        {
            return index;
        }

        /// <summary>
        /// Determine if we are querying a URL.
        /// </summary>
        /// <returns>Returns true if we are querying a URL, false otherwise.</returns>
        public bool getIsQueryingURL()
        {
            return status == securityStatus.SS_NOT_DETERMINED ||
                   status == securityStatus.SS_HTTP_QUERY_IN_PROGRESS ||
                   status == securityStatus.SS_HTTPS_QUERY_IN_PROGRESS;

        }
    }
}
