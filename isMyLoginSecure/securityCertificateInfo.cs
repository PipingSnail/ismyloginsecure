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
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace isMyLoginSecure
{
    /// <summary>
    /// This class provides information about the web page security certificate.
    /// </summary>
    [Serializable]
    public class securityCertificateInfo
    {
        /// <summary>
        /// The security certificate from the web page negotiation.
        /// </summary>
        /// <remarks>This certificate will be set when a https page is loaded (or a http page is requested by results in a https page).</remarks>
        [NonSerialized]
        private X509Certificate sslCertificate = null;

        /// <summary>
        /// The security certificate chain from the web page negotiation.
        /// </summary>
        /// <remarks>This certificate chain will be set when a https page is loaded (or a http page is requested by results in a https page).</remarks>
        [NonSerialized]
        private X509Chain sslChain = null;

        /// <summary>
        /// The security certificate error status from the web page negotiation.
        /// </summary>
        /// <remarks>This certificate error status will be set when a https page is loaded (or a http page is requested by results in a https page).</remarks>
        private System.Net.Security.SslPolicyErrors sslPolicyErrors = System.Net.Security.SslPolicyErrors.None;

        /// <summary>
        /// The security protocol from the web page negotiation.
        /// </summary>
        /// <remarks>This protocol will be set for any successful fetch of a secure page.</remarks>
        private System.Net.SecurityProtocolType securityProtocol = SecurityProtocolType.Ssl3;   // assume the worst

        // data that is obtained by querying the certificate while it is valid

        /// <summary>
        /// This value indicates if a certificate was ever available for this web page.
        /// </summary>
        /// <remarks>true - a certificate was provided (it may be a good or bad certificate). false - no certificate was provided.</remarks>
        private bool hasCertificate = false;

        /// <summary>
        /// This is the security certificate issuer. 
        /// </summary>
        /// <remarks>Set by processCertificate().</remarks>
        private string issuer;

        /// <summary>
        /// This is the security certificate subject. 
        /// </summary>
        /// <remarks>Set by processCertificate().</remarks>
        private string subject;

        /// <summary>
        /// This is the security certificate hash. 
        /// </summary>
        /// <remarks>Set by processCertificate().</remarks>
        private string hash;

        /// <summary>
        /// This is the security certificate start date. 
        /// </summary>
        /// <remarks>Set by processCertificate().</remarks>
        private string validFrom;

        /// <summary>
        /// This is the security certificate expiry date. 
        /// </summary>
        /// <remarks>Set by processCertificate().</remarks>
        private string validUntil;

        /// <summary>
        /// This is the security certificate format. 
        /// </summary>
        /// <remarks>Set by processCertificate().</remarks>
        private string format;

        /// <summary>
        /// This is the security certificate key algorithm. 
        /// </summary>
        /// <remarks>Set by processCertificate().</remarks>
        private string keyAlgorithm;

        /// <summary>
        /// This is the security certificate key algorithm parameters. 
        /// </summary>
        /// <remarks>Set by processCertificate().</remarks>
        private string keyAlgorithmParams;

        /// <summary>
        /// This is the security certificate public key. 
        /// </summary>
        /// <remarks>Set by processCertificate().</remarks>
        private string publicKey;

        /// <summary>
        /// This is the security certificate raw data. 
        /// </summary>
        /// <remarks>Set by processCertificate().</remarks>
        private string rawData;

        /// <summary>
        /// This is the security certificate serial number. 
        /// </summary>
        /// <remarks>Set by processCertificate().</remarks>
        private string serialNumber;

        // certificate chain info

        /// <summary>
        /// This is the security certificate chain revocation flag. 
        /// </summary>
        /// <remarks>Set by processCertificateChain().</remarks>
        private string chainRevocationFlag;

        /// <summary>
        /// This is the security certificate chain revocation mode. 
        /// </summary>
        /// <remarks>Set by processCertificateChain().</remarks>
        private string chainRevocationMode;

        /// <summary>
        /// This is the security certificate chain verification flags. 
        /// </summary>
        /// <remarks>Set by processCertificateChain().</remarks>
        private string chainVerificationFlags;

        /// <summary>
        /// This is the security certificate chain verification time. 
        /// </summary>
        /// <remarks>The verification time is effectively the time the certificate was fetched.
        /// Set by processCertificateChain().</remarks>
        private string chainVerificationTime;

        /// <summary>
        /// This is the security certificate chain status length. 
        /// </summary>
        /// <remarks>Set by processCertificateChain().</remarks>
        private string chainStatusLength;

        /// <summary>
        /// This is the security certificate chain application policy count. 
        /// </summary>
        /// <remarks>Set by processCertificateChain().</remarks>
        private string chainApplicationPolicyCount;

        /// <summary>
        /// This is the security certificate chain certificate policy count. 
        /// </summary>
        /// <remarks>Set by processCertificateChain().</remarks>
        private string chainCertificatePolicyCount;

        /// <summary>
        /// This is the security certificate chain synchronized status. 
        /// </summary>
        /// <remarks>Set by processCertificateChain().</remarks>
        private string chainElementsSynchronized;

        /// <summary>
        /// This is the list of security certificate chain status information. 
        /// </summary>
        /// <remarks>Set by processCertificateChain().</remarks>
        private List<securityCertificateChain> chain = new List<securityCertificateChain>();

        /// <summary>
        /// Constructor for securityCertificateInfo.
        /// </summary>
        public securityCertificateInfo()
        {
            resetCertData();
            resetCertChainData();
        }

        /// <summary>
        /// Resets the object back to it's initial state.
        /// </summary>
        public void reset()
        {
            sslCertificate = null;
            sslChain = null;
            sslPolicyErrors = System.Net.Security.SslPolicyErrors.None;
            securityProtocol = SecurityProtocolType.Ssl3;
            hasCertificate = false;

            resetCertData();
            resetCertChainData();
        }

        /// <summary>
        /// Resets the certificate data back to their default states.
        /// </summary>
        /// <remarks>Does not reset the security certificate related objects.</remarks>
        private void resetCertData()
        {
            issuer = "";
            subject = "";
            hash = "";
            validFrom = "";
            validUntil = "";
            format = "";
            keyAlgorithm = "";
            keyAlgorithmParams = "";
            publicKey = "";
            rawData = "";
            serialNumber = "";
        }

        /// <summary>
        /// Resets the certificate chain data back to their default states.
        /// </summary>
        /// <remarks>Does not reset the security certificate related objects.</remarks>
        private void resetCertChainData()
        {
            chainRevocationFlag = "";
            chainRevocationMode = "";
            chainVerificationFlags = "";
            chainVerificationTime = "";
            chainStatusLength = "";
            chainApplicationPolicyCount = "";
            chainCertificatePolicyCount = "";
            chainElementsSynchronized = "";

            if (chain != null)
                chain.Clear();
        }

        /// <summary>
        /// Set the security certificate from a web page negotiation.
        /// </summary>
        /// <remarks>This also queries the certificate for any data needed by the class.</remarks>
        /// <param name="certification">The security certificate</param>
        public void setSSLCertificate(System.Security.Cryptography.X509Certificates.X509Certificate certification)
        {
            sslCertificate = certification;
            processCertificate();
        }

        /// <summary>
        /// Set the security certificate chain from a web page negotiation.
        /// </summary>
        /// <remarks>This also queries the certificate chain for any data needed by the class.</remarks>
        /// <param name="chain">The security certificate chain</param>
        public void setSSLChain(System.Security.Cryptography.X509Certificates.X509Chain chain)
        {
            sslChain = chain;
            processCertificateChain();
        }

        /// <summary>
        /// Set the security policy errors value from the web page negotiation.
        /// </summary>
        /// <param name="policyErrors">The security policy errors status.</param>
        public void setSSLPolicyErrorStatus(System.Net.Security.SslPolicyErrors policyErrors)
        {
            sslPolicyErrors = policyErrors;
        }

        /// <summary>
        /// Query if a certificate was provided.
        /// </summary>
        /// <remarks>Do not use this to check if the certificate is good or bad, only use to check if there was a certificate at all.</remarks>
        /// <returns>true - has a certificate. false - no certificate.</returns>
        public bool getHasCertificate()
        {
            return hasCertificate;
        }

        /// <summary>
        /// Query if the security certificate is bad.
        /// </summary>
        /// <returns>true - security certificate has errors. false - security certificate is good.</returns>
        public bool getIsBadCertificate()
        {
            return sslPolicyErrors != System.Net.Security.SslPolicyErrors.None;
        }

        /// <summary>
        /// Query for a human readable error status.
        /// </summary>
        /// <returns>A human readable string containing the error status.</returns>
        public string getSSLStatus()
        {
            string sslStatus;

            if (hasCertificate)
            {
                if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
                {
                    sslStatus = "No certificate errors.";
                }
                else
                {
                    sslStatus = "";

                    if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
                        sslStatus += "Remote certificate chain errors. ";

                    if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch) != 0)
                        sslStatus += "Remote certificate name mismatch. ";

                    if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateNotAvailable) != 0)
                        sslStatus += "Remote certificate not available. ";
                }
            }
            else
            {
                sslStatus = "No certificate.";
            }

            return sslStatus;
        }

        /// <summary>
        /// Set the security protocol that loaded this page.
        /// </summary>
        /// <param name="protocol">The security protocol.</param>
        public void setSecurityProtocol(System.Net.SecurityProtocolType protocol)
        {
            securityProtocol = protocol;
        }

        /// <summary>
        /// Get the security protocol that loaded this page.
        /// </summary>
        /// <returns>The security protocol.</returns>
        public System.Net.SecurityProtocolType getSecurityProtocol()
        {
            return securityProtocol;
        }

        /// <summary>
        /// Get a human readable string representing the security protocol that loaded this page.
        /// </summary>
        /// <returns>A human readable string containing the security protocol.</returns>
        public string getSecurityProtocolAsString()
        {
            string str;

            if (hasCertificate)
            {
                switch (securityProtocol)
                {
                    case SecurityProtocolType.Ssl3:
                        str = "SSL3";
                        break;

                    case SecurityProtocolType.Tls:
                        str = "TLS 1.0";
                        break;

                    case SecurityProtocolType.Tls11:
                        str = "TLS 1.1";
                        break;

                    case SecurityProtocolType.Tls12:
                        str = "TLS 1.2";
                        break;

                    default:
                        str = "Default";
                        break;
                }
            }
            else
            {
                str = "-";
            }

            return str;
        }

        /// <summary>
        /// Query the security certificate to get the data needed by the class.
        /// </summary>
        private void processCertificate()
        {
            if (sslCertificate != null)
            {
                hasCertificate = true;

                issuer = sslCertificate.Issuer;
                subject = sslCertificate.Subject;
                hash = sslCertificate.GetCertHashString();
                validFrom = sslCertificate.GetEffectiveDateString();
                validUntil = sslCertificate.GetExpirationDateString();
                format = sslCertificate.GetFormat();
                keyAlgorithm = sslCertificate.GetKeyAlgorithm();
                keyAlgorithmParams = sslCertificate.GetKeyAlgorithmParametersString();
                publicKey = sslCertificate.GetPublicKeyString();
                rawData = sslCertificate.GetRawCertDataString();
                serialNumber = sslCertificate.GetSerialNumberString();
            }
            else
            {
                hasCertificate = false;

                resetCertData();
            }
        }

        /// <summary>
        /// Query the security certificate chain to get the data needed by the class.
        /// </summary>
        private void processCertificateChain()
        {
            if (sslChain != null)
            {
                chainRevocationFlag = sslChain.ChainPolicy.RevocationFlag.ToString();
                chainRevocationMode = sslChain.ChainPolicy.RevocationMode.ToString();
                chainVerificationFlags = sslChain.ChainPolicy.VerificationFlags.ToString();
                chainVerificationTime = sslChain.ChainPolicy.VerificationTime.ToString();
                chainStatusLength = sslChain.ChainStatus.Length.ToString();
                chainApplicationPolicyCount = sslChain.ChainPolicy.ApplicationPolicy.Count.ToString();
                chainCertificatePolicyCount = sslChain.ChainPolicy.CertificatePolicy.Count.ToString();
                chainElementsSynchronized = sslChain.ChainElements.IsSynchronized.ToString();

                foreach (X509ChainElement element in sslChain.ChainElements)
                {
                    securityCertificateChain scc = new securityCertificateChain();

                    scc.getInformation(element);

                    chain.Add(scc);
                }
            }
            else
            {
                resetCertChainData();
            }
        }

        /// <summary>
        /// Get the chain revocation flag.
        /// </summary>
        /// <returns>Human readable chain revocation flag.</returns>
        public string getChainRevocationFlag()
        {
            return chainRevocationFlag;
        }

        /// <summary>
        /// Get the chain revocation mode.
        /// </summary>
        /// <returns>Human readable chain revocation mode.</returns>
        public string getChainRevocationMode()
        {
            return chainRevocationMode;
        }

        /// <summary>
        /// Get the chain verification flags.
        /// </summary>
        /// <returns>Human readable chain verification flags.</returns>
        public string getChainVerificationFlags()
        {
            return chainVerificationFlags;
        }

        /// <summary>
        /// Get the chain verification time. This is effectively when the security certificate chain was queried.
        /// </summary>
        /// <returns>Human readable verification time.</returns>
        public string getChainVerificationTime()
        {
            return chainVerificationTime;
        }

        /// <summary>
        /// Get the chain status length.
        /// </summary>
        /// <returns>Human readable chain status length.</returns>
        public string getChainStatusLength()
        {
            return chainStatusLength;
        }

        /// <summary>
        /// Get the chain application policy count.
        /// </summary>
        /// <returns>Human readable chain application policy count.</returns>
        public string getChainApplicationPolicyCount()
        {
            return chainApplicationPolicyCount;
        }

        /// <summary>
        /// Get the chain certificate policy count.
        /// </summary>
        /// <returns>Human readable chain certificate policy count.</returns>
        public string getChainCertificatePolicyCount()
        {
            return chainCertificatePolicyCount;
        }

        /// <summary>
        /// Query are the security certificate chain elements synchronized?
        /// </summary>
        /// <returns>Human readable chain synchronization status.</returns>
        public string getChainElementsSynchronized()
        {
            return chainElementsSynchronized;
        }

        /// <summary>
        /// Query how many security certificate chains there are.
        /// </summary>
        /// <returns>Number of security certificate chains.</returns>
        public int getNumChains()
        {
            return chain.Count;
        }

        // warning, can return null

        /// <summary>
        /// Get a security certificate chain.
        /// </summary>
        /// <param name="index">Index of the security certificate chain. Index is &gt;= 0 and &lt; getNumChains().</param>
        /// <returns>A security certificate chain.</returns>
        public securityCertificateChain getChain(int index)
        {
            securityCertificateChain scc = null;

            if (chain != null)
            {
                if (index >= 0 && index < chain.Count)
                    scc = chain[index];
            }

            return scc;
        }

        /// <summary>
        /// Force the "has certificate" status to true.
        /// We need this when using the query status to get a useful human readable string.
        /// </summary>
        public void forceHasCertificate()
        {
            hasCertificate = true;
        }
    }
}

