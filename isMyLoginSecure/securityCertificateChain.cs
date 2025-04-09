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
using System.Security.Cryptography.X509Certificates;

namespace isMyLoginSecure
{
    /// <summary>
    /// This class provides information about the security certificate chain.
    /// </summary>
    [Serializable]
    public class securityCertificateChain
    {
        /// <summary>
        /// chainStatus is a simple way to store the status and statusInformation fields from
        /// a security certificate chain. The data is stored in string format to facilitate ease of use.
        /// </summary>
        [Serializable]
        struct chainStatus
        {
            public string status;
            public string information;
        };

        /// <summary>
        /// issuer is the security certificate chain issuing authority.
        /// </summary>
        private string issuer;

        /// <summary>
        /// validUntil is the date at which the certificate chain is no longer valid.
        /// </summary>
        private string validUntil;

        /// <summary>
        /// isValid indicates if the certificate chain is valid. 
        /// </summary>
        /// <remarks>Use this value rather than trying to determine
        /// this for yourself by querying the certificate chain. The rules for validity are non-trivial, hence you 
        /// should use isValid as it is a value supplied by the certificate chain.</remarks>
        private bool isValid;

        /// <summary>
        /// information provides additional data about the certificate chain.
        /// </summary>
        private string information;

        /// <summary>
        /// chainInfo holds information about the chain.
        /// </summary>
        /// <remarks>Each chain has optional status information. Use the values stored here to determine that information.</remarks>
        private List<chainStatus> chainInfo;

        /// <summary>
        /// securityCertificateChain constructor. Provides a securityCertificateChain in it's reset condition.
        /// </summary>
        public securityCertificateChain()
        {
            reset();
        }

        /// <summary>
        /// Resets the securityCertificateChain to an empty state.
        /// </summary>
        private void reset()
        {
            issuer = "";
            validUntil = "";
            isValid = false;
            information = "";
            chainInfo = new List<chainStatus>();
        }

        /// <summary>
        /// This queries an X509ChainElement information.
        /// </summary>
        /// <remarks>We query for information so that we don't need to rely on actual security certificate
        /// related objects later on. If we serialise these objects to/from a database this will be useful.</remarks>
        /// <param name="element">A chain element from a security certificate chain.</param>
        /// <remarks>Passing a null element parameter is equivalent to calling reset().</remarks>
        public void getInformation(X509ChainElement element)
        {
            if (element != null)
            {
                issuer = element.Certificate.Issuer;
                validUntil = element.Certificate.NotAfter.ToString();
                isValid = element.Certificate.Verify();
                information = element.Information;

                for (int index = 0; index < element.ChainElementStatus.Length; index++)
                {
                    chainStatus cs = new chainStatus();
                    cs.status = element.ChainElementStatus[index].Status.ToString();
                    cs.information = element.ChainElementStatus[index].StatusInformation;

                    chainInfo.Add(cs);
                }
            }
            else
            {
                reset();
            }
        }

        /// <summary>
        /// Get the security certificate chain issuer.
        /// </summary>
        /// <returns>Security certificate chain issuer.</returns>
        public string getIssuer()
        {
            return issuer;
        }

        /// <summary>
        /// Get the security certificate chain expiry date.
        /// </summary>
        /// <returns>The security certificate chain expiry date.</returns>
        public string getValidUntil()
        {
            return validUntil;
        }

        /// <summary>
        /// Get the status of the security certificate chain. 
        /// </summary>
        /// <remarks>Use this function. Do not try to calculate validity yourself.</remarks>
        /// <returns>true - certificate chain is valid. false - certificate chain is not valid.</returns>
        public bool getIsValid()
        {
            return isValid;
        }

        /// <summary>
        /// Get information about the security certificate chain.
        /// </summary>
        /// <returns>Information about the security certificate chain</returns>
        public string getInformation()
        {
            return information;
        }

        /// <summary>
        /// Query how many chain status entries there are.
        /// </summary>
        /// <returns>Number of chain status entries.</returns>
        public int getNumChains()
        {
            return chainInfo.Count;
        }

        /// <summary>
        /// Get status from a chain status entry.
        /// </summary>
        /// <param name="index">Index of the chain status entry required. Index is >= 0 and less than getNumChains().</param>
        /// <returns>The specified chain status entry.</returns>
        public string getChainStatus(int index)
        {
            string scc = "";

            if (chainInfo != null)
            {
                if (index >= 0 && index < chainInfo.Count)
                    scc = chainInfo[index].status;
            }

            return scc;
        }

        /// <summary>
        /// Get information from a chain status entry.
        /// </summary>
        /// <param name="index">Index of the chain status entry required. Index is >= 0 and less than getNumChains().</param>
        /// <returns>The specified chain information entry.</returns>
        public string getChainInformation(int index)
        {
            string scc = "";

            if (chainInfo != null)
            {
                if (index >= 0 && index < chainInfo.Count)
                    scc = chainInfo[index].information;
            }

            return scc;
        }
    }
}
