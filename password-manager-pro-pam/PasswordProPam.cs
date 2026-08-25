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
using Keyfactor.Platform.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace Keyfactor.Extensions.Pam.PasswordManagerPro
{
    public class PasswordManagerProException : Exception
    {
        public PasswordManagerProException(string msg) : base(msg) { }
    }
    public class PasswordManagerPAM : IPAMProvider
    {
        public string Name => "Password-Manager-Pro";

        public string GetPassword(Dictionary<string, string> instanceParameters, Dictionary<string, string> initializationInfo)
        {
            ILogger logger = LogHandler.GetClassLogger<PasswordManagerPAM>();
            logger.MethodEntry(LogLevel.Trace);
            logger.LogDebug("Password Manager Pro Starting");
            string lookupType = instanceParameters["LookupType"].Trim();
            if (lookupType.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogDebug("Returning Username");
                return instanceParameters["accountName"];
            }
            else if (lookupType.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                return PasswordManagerAPI.GetPasswordManagerValue(Name, instanceParameters, new Uri(initializationInfo["Host"]), initializationInfo["Authtoken"]);
            }
            else
            {
                logger.LogError($"PAM extension Lookup type {instanceParameters["LookupType"]} is invalid. Options: Username, Password");
                throw new PasswordManagerProException($"PAM extension Lookup type {instanceParameters["LookupType"]} is invalid. Options: Username, Password");
            }
        }
    }
}
