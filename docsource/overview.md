## Overview

The Password Manager Pro PAM Provider allows for the retrieval of stored account credentials from a Password Manager Pro instance. An API User, created in Password Manager Pro, is required and the users access token is used to retrieve secrets from Password Manager Pro.

## Installation and Configuration
#### In ManageEngine Password Manager Pro
When configuring ManageEngine Password Manager Pro (PMP) for use as a PAM Provider with Keyfactor, you will need to create an API user and generate an API token with the appropriate permissions. Navigate to Users → Add User → Add Api user in the PMP web interface to create the user and token. Ensure the associated user account has at minimum read access to the resources you intend to retrieve credentials from, and the host name field is set to the IP of your Keyfactor Command instance.

After ensuring the API User exists and has access to the resource and account you wish to retrieve from PMP, you can use the resource's name (the "Resource Name") and the account name (the "Account Name") to retrieve credentials via the PMP PAM Provider extension.

#### On the Universal Orchestrator
Configuring the UO to use the Password Manager Pro PAM Provider requries first installing it as an extension by copying the release contents into a new extension folder named `Password-Manager-Pro`.
A `manifest.json` file is included in the release. This file needs to be edited to enter in the "initialization" parameters for the PAM Provider. Specifically values need to be entered for the parameters in the `manifest.json` of the __PAM Provider extension__:

~~~ json
"Keyfactor:PAMProviders:Password-Manager-Pro:InitializationInfo": {
    "Host": "https://localhost:7272",
    "Authtoken": "xxxxxxx"
  }
~~~

## SSL Certificate Configuration
ManageEngine Password Manager Pro uses HTTPS with a self-signed or internally-signed certificate. On the machine running the Keyfactor PAM Provider, .NET will reject this certificate by default with an UntrustedRoot error. You must add the PMP certificate to the Windows trust store before the extension can connect.
Exporting the certificate

Navigate to the PMP URL in Chrome or Edge
Click the lock icon → Connection is secure → Certificate is valid
Go to the Details tab → Copy to File
Export as DER encoded binary (.cer)

Importing into the Windows trust store

Open certmgr.msc (run as Administrator for the machine store)
Navigate to Trusted Root Certification Authorities → Certificates
Right-click → All Tasks → Import
Select the exported .cer file and complete the wizard

If PMP is running on the same machine as the extension and you are connecting via localhost, the certificate's hostname will not match. This is expected and is handled automatically by the extension; no additional configuration is required.