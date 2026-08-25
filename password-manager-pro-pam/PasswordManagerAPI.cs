// Copyright 2026 Keyfactor 
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Keyfactor.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;


namespace Keyfactor.Extensions.Pam.PasswordManagerPro
{
    internal class PasswordManagerAPI
    {
        internal static AccountResourceLookup GetResourceAccountID(string name, Dictionary<string, string> instanceParameters, Uri host, string Authtoken)
        {
            ILogger logger = LogHandler.GetClassLogger<PasswordManagerAPI>();
            logger.LogDebug($"PAM Provider {name} - Getting resource and account ids.");

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create($"{host}restapi/json/v1/resources/getResourceIdAccountId?RESOURCENAME={instanceParameters["resourceName"]}&ACCOUNTNAME={instanceParameters["accountName"]}");
            req.Method = "GET";
            req.Headers.Add("AUTHTOKEN", Authtoken);
            logger.LogDebug($"PAM Provider {name} - requesting secret located at {req.RequestUri}");

            req.ServerCertificateValidationCallback = (sender, cert, chain, errors) =>
            {
                var filtered = errors & ~System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch;
                return filtered == System.Net.Security.SslPolicyErrors.None;
            };

            Stream responseStream;
            try
            {
                HttpWebResponse WebResponse = (HttpWebResponse)req.GetResponse();
                logger.LogDebug($"Response status: {(int)WebResponse.StatusCode} {WebResponse.StatusDescription}");
                responseStream = WebResponse.GetResponseStream();
            }
            catch (WebException ex)
            {
                Exception current = ex;
                int depth = 0;
                while (current != null && depth <= 4)
                {
                    logger.LogError($"Exception[{depth}]: {current.GetType().Name}: {current.Message}");
                    current = current.InnerException;
                    depth++;
                }
                throw;
            }

            logger.LogTrace($"PAM Provider {name} - received response to ids request");

            string strResponse = new StreamReader(responseStream).ReadToEnd();

            PMPResourceAccountResponse response = JsonConvert.DeserializeObject<PMPResourceAccountResponse>(strResponse);

            if (response.Operation.Result.Status != "Success")
                throw new PasswordManagerProException($"PAM Provider {name} - PMP API error: {response.Operation.Result.Message}");

            AccountResourceLookup accountResources = new AccountResourceLookup();

            accountResources.ResourceId = response.Operation.Details["RESOURCEID"];
            accountResources.AccountId = response.Operation.Details["ACCOUNTID"];

            logger.LogDebug($"PAM Provider {name} - resolved RESOURCEID: {accountResources.ResourceId} and ACCOUNTID: {accountResources.AccountId}");
            return accountResources;
        }

        internal static string GetPasswordManagerValue(string name, Dictionary<string, string> instanceParameters, Uri host, string Authtoken)
        {
            ILogger logger = LogHandler.GetClassLogger<PasswordManagerAPI>();
            logger.LogDebug($"PAM Provider {name} - Beginning secret fetch.");

            AccountResourceLookup accountIds = GetResourceAccountID(name, instanceParameters, host, Authtoken);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create($"{host}restapi/json/v1/resources/{accountIds.ResourceId}/accounts/{accountIds.AccountId}/password");
            req.Method = "GET";
            req.Headers.Add("AUTHTOKEN", Authtoken);
            logger.LogDebug($"PAM Provider {name} - requesting secret located at {req.RequestUri}");

            req.ServerCertificateValidationCallback = (sender, cert, chain, errors) =>
            {
                var filtered = errors & ~System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch;
                return filtered == System.Net.Security.SslPolicyErrors.None;
            };

            Stream responseStream;
            try
            {
                HttpWebResponse WebResponse = (HttpWebResponse)req.GetResponse();
                logger.LogDebug($"Response status: {(int)WebResponse.StatusCode} {WebResponse.StatusDescription}");
                responseStream = WebResponse.GetResponseStream();
            }
            catch (WebException ex)
            {
                Exception current = ex;
                int depth = 0;
                while (current != null && depth <= 4)
                {
                    logger.LogError($"Exception[{depth}]: {current.GetType().Name}: {current.Message}");
                    current = current.InnerException;
                    depth++;
                }
                throw;
            }

            logger.LogTrace($"PAM Provider {name} - received response to password request");

            string strResponse = new StreamReader(responseStream).ReadToEnd();

            PMPResourceAccountResponse response = JsonConvert.DeserializeObject<PMPResourceAccountResponse>(strResponse);

            if (response.Operation.Result.Status != "Success")
                throw new Exception($"PAM Provider {name} - PMP API error: {response.Operation.Result.Message}");

            logger.LogDebug($"PAM Provider {name} - returning password from Password Manager Pro");
            return response.Operation.Details["PASSWORD"];
        }
    }
    public class AccountResourceLookup
    {
        public string ResourceId { get; set; }
        public string AccountId { get; set; }
    }
    internal class PMPResourceAccountResponse
    {
        [JsonProperty("operation")]
        public PMPOperation Operation { get; set; }
    }

    internal class PMPOperation
    {
        [JsonProperty("result")]
        public PMPResult Result { get; set; }

        [JsonProperty("Details")]
        public Dictionary<string, string> Details { get; set; }
    }

    internal class PMPResult
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
