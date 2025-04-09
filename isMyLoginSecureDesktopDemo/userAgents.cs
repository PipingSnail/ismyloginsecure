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

namespace isMyLoginSecureDesktopDemo
{
    class userAgents
    {
        /// <summary>
        /// Simple struct to hold two items of information together - a business name and a website.
        /// </summary>
        /// <remarks>Arrays of this structure will be used to setup lists of institutions for automatic testing.</remarks>
        public struct userAgentInfo
        {
            public userAgentInfo(string theAgentName,
                                 string theUserAgentString)
            {
                userAgent = theAgentName;
                userAgentString = theUserAgentString;
            }

            public string userAgent { get; }
            public string userAgentString { get; }
        }
        // TODO: need to expand to:
        // Windows
        // Macintosh
        // Linux
        // iPhone
        // iPad
        // Android phone
        // Android tablet
        // Windows phone

        public static readonly userAgentInfo[] theUserAgents =
        {
            new userAgentInfo("Chrome",                 "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.78 Safari/537.36"),                   // windows
            new userAgentInfo("Firefox",                "Mozilla/5.0 (Windows NT x.y; rv:10.0) Gecko/20100101 Firefox/10.0"),
            new userAgentInfo("Opera",                  "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1500.52 Safari/537.36 OPR/15.0.1147.100"),
            new userAgentInfo("Edge",                   "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246"),
            new userAgentInfo("Internet Explorer 11",   "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko"),
            new userAgentInfo("Internet Explorer 10",   "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)"),
            new userAgentInfo("Internet Explorer 9",    "Mozilla/5.0 (Windows; U; MSIE 9.0; WIndows NT 9.0; en-US))"),
            new userAgentInfo("Internet Explorer 8",    "Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; GTB7.4; InfoPath.2; SV1; .NET CLR 3.3.69573; WOW64; en-US)"),
            new userAgentInfo("Internet Explorer 7",    "Mozilla/5.0 (Windows; U; MSIE 7.0; Windows NT 6.0; en-US)"),
            new userAgentInfo("Internet Explorer 6",    "Mozilla/5.0 (Windows; U; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)"),
            new userAgentInfo("Safari",                 "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1"),                   // phone
            new userAgentInfo("Android",                "Mozilla/5.0 (Linux; Android 4.2.1; en-us; Nexus 5 Build/JOP40D) AppleWebKit/535.19 (KHTML, like Gecko; googleweblight) Chrome/38.0.1025.166 Mobile Safari/535.19"),                  // phone
            new userAgentInfo("Windows Phone 8.1",      "Mozilla/5.0 (Mobile; Windows Phone 8.1; Android 4.0; ARM; Trident/7.0; Touch; rv:11.0; IEMobile/11.0; NOKIA; Lumia 635) like iPhone OS 7_0_3 Mac OS X AppleWebKit/537 (KHTML, like Gecko) Mobile Safari/537"),
        };
    }
}
