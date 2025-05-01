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

namespace isMyLoginSecure
{
    /// <summary>
    /// This class provides information about the web page security headers.
    /// </summary>
    /// <remarks>This class currently tracks the following security headers: 
    /// X-Content-Type-Options 
    /// X-XSS-Protection 
    /// X-Frame-Options 
    /// Strict-Transport-Security 
    /// Content-Security-Policy 
    /// Referrer-Policy
    /// </remarks>
    [Serializable]
    public class securityHeadersInfo
    {
        /// <summary>
        /// The value of the X-Content-Type-Options security header
        /// </summary>
        private string securityHeaderXContentTypeOptions;

        /// <summary>
        /// The value of the X-XSS-Protection security header
        /// </summary>
        private string securityHeaderXXSSProtection;

        /// <summary>
        /// The value of the X-Frame-Options security header
        /// </summary>
        private string securityHeaderXFrameOptions;

        /// <summary>
        /// The value of the Strict-Transport-Security security header
        /// </summary>
        private string securityHeaderStrictTransportSecurity;

        /// <summary>
        /// The value of the Content-Security-Policy security header
        /// </summary>
        private string securityHeaderContentSecurityPolicy;

        /// <summary>
        /// The value of the Referrer-Policy security header
        /// </summary>
        private string securityHeaderReferrerPolicy;

        /// <summary>
        /// Reset the security headers to a reset state, as if it had just been created.
        /// </summary>
        public void reset()
        {
            securityHeaderXContentTypeOptions = null;
            securityHeaderXXSSProtection = null;
            securityHeaderXFrameOptions = null;
            securityHeaderStrictTransportSecurity = null;
            securityHeaderContentSecurityPolicy = null;
            securityHeaderReferrerPolicy = null;
        }

        /// <summary>
        /// This method examines a web response for the header values we are interested in.
        /// </summary>
        /// <param name="response">A HttpWebResponse header from an attempt to read a web page.</param>
        public void processSecurityHeaders(System.Net.HttpWebResponse response)
        {
            reset();

            for (int i = 0; i < response.Headers.Count; ++i)
            {
                //Console.WriteLine("\nHeader Name:{0}, Value :{1}", response.Headers.Keys[i], response.Headers[i]);

                if (response.Headers.Keys[i] == "X-Content-Type-Options")
                    securityHeaderXContentTypeOptions = response.Headers[i];
                else if (response.Headers.Keys[i] == "X-XSS-Protection")
                    securityHeaderXXSSProtection = response.Headers[i];
                else if (response.Headers.Keys[i] == "X-Frame-Options")
                    securityHeaderXFrameOptions = response.Headers[i];
                else if (response.Headers.Keys[i] == "Strict-Transport-Security")
                    securityHeaderStrictTransportSecurity = response.Headers[i];
                else if (response.Headers.Keys[i] == "Content-Security-Policy")
                    securityHeaderContentSecurityPolicy = response.Headers[i];
                else if (response.Headers.Keys[i] == "Referrer-Policy")
                    securityHeaderReferrerPolicy = response.Headers[i];
            }
        }

        /// <summary>
        /// Count how many security headers were found.
        /// </summary>
        /// <returns>Number of security headers returned by the web page.</returns>
        private int getSecurityHeaderCount()
        {
            int count = 0;

            if (securityHeaderXContentTypeOptions != null)
                count++;

            if (securityHeaderXXSSProtection != null)
                count++;

            if (securityHeaderXFrameOptions != null)
                count++;

            if (securityHeaderStrictTransportSecurity != null)
                count++;

            if (securityHeaderContentSecurityPolicy != null)
                count++;

            if (securityHeaderReferrerPolicy != null)
                count++;

            return count;
        }

        /// <summary>
        /// Determines if all security headers have been provided by this web page.
        /// </summary>
        /// <returns>true - not all security headers have been provided for this web page. false - all security headers have been provided for this web page.</returns>
        public bool hasImperfectSecurityHeaders()
        {
            return getSecurityHeaderCount() < 6;
        }

        /// <summary>
        /// Returns a grade from 0..100% indicating the security header status.
        /// </summary>
        /// <returns>Security header grade</returns>
        public string getSecurityHeaderGrade()
        {
            int count;

            count = getSecurityHeaderCount();

            string output;

            output = string.Format("{0}%", (100 * count / 6));
            return output;
        }

        /// <summary>
        /// Get the value of the X-Content-Type-Options security header.
        /// </summary>
        /// <remarks>An returned empty string means the header was supplied but had no content. A null returned string means the header was not supplied.</remarks>
        /// <returns>The X-Content-Type-Options security header. Caution, can return null object.</returns>
        public string getSecurityHeaderXContentTypeOptions()
        {
            return securityHeaderXContentTypeOptions;
        }

        /// <summary>
        /// Get the value of the X-XSS-Protection security header.
        /// </summary>
        /// <remarks>An returned empty string means the header was supplied but had no content. A null returned string means the header was not supplied.</remarks>
        /// <returns>The X-XSS-Protection security header. Caution, can return null object.</returns>
        public string getSecurityHeaderXXSSProtection()
        {
            return securityHeaderXXSSProtection;
        }

        /// <summary>
        /// Get the value of the X-Frame-Options security header.
        /// </summary>
        /// <remarks>An returned empty string means the header was supplied but had no content. A null returned string means the header was not supplied.</remarks>
        /// <returns>The X-Frame-Options security header. Caution, can return null object.</returns>
        public string getSecurityHeaderXFrameOptions()
        {
            return securityHeaderXFrameOptions;
        }

        /// <summary>
        /// Get the value of the Strict-Transport-Security security header.
        /// </summary>
        /// <remarks>An returned empty string means the header was supplied but had no content. A null returned string means the header was not supplied.</remarks>
        /// <returns>The Strict-Transport-Security security header. Caution, can return null object.</returns>
        public string getSecurityHeaderStrictTransportSecurity()
        {
            return securityHeaderStrictTransportSecurity;
        }

        /// <summary>
        /// Get the value of the Content-Security-Policy security header.
        /// </summary>
        /// <remarks>An returned empty string means the header was supplied but had no content. A null returned string means the header was not supplied.</remarks>
        /// <returns>The Content-Security-Policy security header. Caution, can return null object.</returns>
        public string getSecurityHeaderContentSecurityPolicy()
        {
            return securityHeaderContentSecurityPolicy;
        }

        /// <summary>
        /// Get the value of the Referrer-Policy security header.
        /// </summary>
        /// <remarks>An returned empty string means the header was supplied but had no content. A null returned string means the header was not supplied.</remarks>
        /// <returns>The Referrer-Policy security header. Caution, can return null object.</returns>
        public string getSecurityHeaderReferrerPolicy()
        {
            return securityHeaderReferrerPolicy;
        }

        /// <summary>
        /// Get a list of all the missing security header options
        /// </summary>
        /// <returns>A list of the missing security header options.</returns>
        public List<string> getMissingSecurityHeaderOptions()
        {
            List<string>    missing = new List<string>();

            if (securityHeaderXContentTypeOptions == null ||
                securityHeaderXContentTypeOptions.Length == 0)
                missing.Add("X-Content-Type-Options");

            if (securityHeaderXXSSProtection == null ||
                securityHeaderXXSSProtection.Length == 0)
                missing.Add("X-XSS-Protection");

            if (securityHeaderXFrameOptions == null ||
                securityHeaderXFrameOptions.Length == 0)
                missing.Add("X-Frame-Options");

            if (securityHeaderStrictTransportSecurity == null ||
                securityHeaderStrictTransportSecurity.Length == 0)
                missing.Add("Strict-Transport-Security");
                    
            if (securityHeaderContentSecurityPolicy == null ||
                securityHeaderContentSecurityPolicy.Length == 0)
                missing.Add("Content-Security-Policy");
                    
            if (securityHeaderReferrerPolicy == null ||
                securityHeaderReferrerPolicy.Length == 0)
                missing.Add("Referrer-Policy");

            return missing;
        }
    }
}
