// Copyright 2023 Keyfactor
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
    public class PasswordManagerPAM : IPAMProvider
    {
        public string Name => "Password-Manager-Pro";

        public string GetPassword(Dictionary<string, string> instanceParameters, Dictionary<string, string> initializationInfo)
        {
            ILogger logger = LogHandler.GetClassLogger<PasswordManagerPAM>();
            logger.LogDebug("Password Manager Pro Starting");
            logger.MethodEntry(LogLevel.Trace);
            if (instanceParameters["LookupType"].Equals("Username"))
            {
                logger.LogDebug($"Returning: {instanceParameters["accountName"]}");
                return instanceParameters["accountName"];
            }
            else if (instanceParameters["LookupType"].Equals("Password"))
            {
                return PasswordManagerAPI.GetPasswordManagerValue(Name, instanceParameters, new Uri(initializationInfo["Host"]), initializationInfo["Authtoken"]);
            }
            else
            {
                logger.LogError("PAM extension Lookup type parameter must be defined. Options: Username, Password");
                return "NULL";
            }
        }
    }
}
