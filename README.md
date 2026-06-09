<h1 align="center" style="border-bottom: none">
    Password Manager Pro Pam Provider
</h1>

<p align="center">
  <!-- Badges -->
<img src="https://img.shields.io/badge/integration_status-production-3D1973?style=flat-square" alt="Integration Status: production" />
<a href="https://github.com/Keyfactor/password-manager-pro-pam/releases"><img src="https://img.shields.io/github/v/release/Keyfactor/password-manager-pro-pam?style=flat-square" alt="Release" /></a>
<img src="https://img.shields.io/github/issues/Keyfactor/password-manager-pro-pam?style=flat-square" alt="Issues" />
<img src="https://img.shields.io/github/downloads/Keyfactor/password-manager-pro-pam/total?style=flat-square&label=downloads&color=28B905" alt="GitHub Downloads (all assets, all releases)" />
</p>

<p align="center">
  <!-- TOC -->
  <a href="#support">
    <b>Support</b>
  </a>
  ·
  <a href="#getting-started">
    <b>Installation</b>
  </a>
  ·
  <a href="#license">
    <b>License</b>
  </a>
  ·
  <a href="https://github.com/orgs/Keyfactor/repositories?q=pam">
    <b>Related Integrations</b>
  </a>
</p>

## Overview

The Password Manager Pro PAM Provider allows for the retrieval of stored account credentials from a Password Manager Pro instance. An API User, created in Password Manager Pro, is required and the users access token is used to retrieve secrets from Password Manager Pro.

## Installation and Configuration

#### In ManageEngine Password Manager Pro
When configuring ManageEngine Password Manager Pro (PMP) for use as a PAM Provider with Keyfactor, you will need to create an API user and generate an API token with the appropriate permissions. Navigate to Users → Add User → Add Api user in the PMP web interface to create the user and token. Ensure the associated user account has at minimum read access to the resources you intend to retrieve credentials from, and the host name field is set to the ip of your KeyFactor Command instance.
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

## Support
The Password Manager Pro Pam Provider is supported by Keyfactor for Keyfactor customers. If you have a support issue, please open a support ticket via the Keyfactor Support Portal at https://support.keyfactor.com.

> To report a problem or suggest a new feature, use the **[Issues](../../issues)** tab. If you want to contribute actual bug fixes or proposed enhancements, use the **[Pull requests](../../pulls)** tab.

## Getting Started

The Password Manager Pro Pam Provider is used by Command to resolve PAM-eligible credentials for Universal Orchestrator extensions and for accessing Certificate Authorities. When configured, Command will use the Password Manager Pro Pam Provider to retrieve credentials needed to communicate with the target system. There are two ways to install the Password Manager Pro Pam Provider, and you may elect to use one or both methods:

1. **Locally on the Keyfactor Command server**: PAM credential resolution via the Password Manager Pro Pam Provider will occur on the Keyfactor Command server each time an elegible credential is needed.
2. **Remotely On Universal Orchestrators**: When Jobs are dispatched to Universal Orchestrators, the associated Certificate Store extension assembly will use the Password Manager Pro Pam Provider to resolve eligible PAM credentials.

Before proceeding with installation, you should consider which pattern is best for your requirements and use case.

### Installation

> [!IMPORTANT]
> For the most up-to-date and complete documentation on how to install a PAM provider extension, please visit our [product documentation](https://software.keyfactor.com/Core-OnPrem/Current/Content/ReferenceGuide/Preparing%20Third%20Party%20PAM%20Providers%20to%20Work%20with.htm?Highlight=pam%20provider#InstallingCustomPAMProviderExtensions)


To install Password Manager Pro Pam Provider, it is recommended you install [kfutil](https://github.com/Keyfactor/kfutil). `kfutil` is a command-line tool that simplifies the process of creating PAM Types in Keyfactor Command.



#### Requirements
   This release requires Keyfactor version 9.10 or greater.
   This release was tested against Password Manager Pro 13.2
   Using this on a Universal Orchestrator requires UO version 10.1 or greater.

#### Create PAM type in Keyfactor Command


##### Using `kfutil`
Create the required PAM Types in the connected Command platform.

```shell
# Password-Manager-Pro
kfutil pam-types create -r password-manager-pro-pam -n Password-Manager-Pro
```

##### Using the API
For full API docs please visit our [product documentation](https://software.keyfactor.com/Core-OnPrem/Current/Content/WebAPI/KeyfactorAPI/PAMProvidersPOSTTypes.htm?Highlight=pam%20type)

Below is the payload to `POST` to the Keyfactor Command API
```json
{
  "Name": "Password-Manager-Pro",
  "Parameters": [
    {
      "Name": "Host",
      "DisplayName": "Vault Host",
      "DataType": 1,
      "InstanceLevel": false,
      "Description": "The IP address or URL of the PMP instance, including any port number"
    },
    {
      "Name": "Authtoken",
      "DisplayName": "Authentication Token",
      "DataType": 2,
      "InstanceLevel": false,
      "Description": "The access token for the PMP Api user."
    },
    {
      "Name": "resourceName",
      "DisplayName": "Resource Name",
      "DataType": 1,
      "InstanceLevel": true,
      "Description": "The Resource Name of the secret to retrieve as displayed in PMP"
    },
    {
      "Name": "accountName",
      "DisplayName": "Account Name",
      "DataType": 1,
      "InstanceLevel": true,
      "Description": "The Account Name of the secret to retrieve as displayed in PMP"
    },
    {
      "Name": "LookupType",
      "DisplayName": "Lookup Type",
      "DataType": 1,
      "InstanceLevel": true,
      "Description": "Must be either Username or Password. Defines which value the extension should retrieve"
    }
  ]
}
```

#### Install PAM provider on Keyfactor Command Host (Local)


1. On the server that hosts Keyfactor Command, download and unzip the latest release of the Password Manager Pro Pam Provider from the [Releases](../../releases) page.

2. Copy the assemblies to the appropriate directories on the Keyfactor Command server:

    <details><summary>Keyfactor Command 11+</summary>

    1. Copy the unzipped assemblies to each of the following directories:

        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebAgentServices\Extensions\password-manager-pro-pam`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebConsole\Extensions\password-manager-pro-pam`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\KeyfactorAPI\Extensions\password-manager-pro-pam`

    </details>

    <details><summary>Keyfactor Command 10</summary>

    1. Copy the assemblies to each of the following directories:

        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebAgentServices\bin\password-manager-pro-pam`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\KeyfactorAPI\bin\password-manager-pro-pam`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebConsole\bin\password-manager-pro-pam`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\Service\password-manager-pro-pam`

    2. Open a text editor on the Keyfactor Command server as an administrator and open the `web.config` file located in the `WebAgentServices` directory.

    3. In the `web.config` file, locate the `<container> </container>` section and add the following registration:

        ```xml
        <container>
            ...
            <!--The following are PAM Provider registrations. Uncomment them to use them in the Keyfactor Product:-->

            <!--Add the following line exactly to register the PAM Provider-->
            <register type="IPAMProvider" mapTo="Keyfactor.Extensions.Pam.PMP, Keyfactor.Command.PAMProviders" name="Password-Manager-Pro" />
        </container>
        ```

    4. Repeat steps 2 and 3 for each of the directories listed in step 1. The configuration files are located in the following paths by default:

        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebAgentServices\web.config`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\KeyfactorAPI\web.config`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebConsole\web.config`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\Service\CMSTimerService.exe.config`

    </details>

3. Restart the Keyfactor Command services (`iisreset`).

#### Install PAM provider on a Universal Orchestrator Host (Remote)


1. Install the Password Manager Pro Pam Provider assemblies.

    * **Using kfutil**: On the server that hosts the Universal Orchestrator, run the following command:

        ```shell
        # Windows Server
        kfutil orchestrator extension -e password-manager-pro-pam@latest --out "C:\Program Files\Keyfactor\Keyfactor Orchestrator\extensions"

        # Linux
        kfutil orchestrator extension -e password-manager-pro-pam@latest --out "/opt/keyfactor/orchestrator/extensions"
        ```

    * **Manually**: Download the latest release of the Password Manager Pro Pam Provider from the [Releases](../../releases) page. Extract the contents of the archive to:

        * **Windows Server**: `C:\Program Files\Keyfactor\Keyfactor Orchestrator\extensions\password-manager-pro-pam`
        * **Linux**: `/opt/keyfactor/orchestrator/extensions/password-manager-pro-pam`

2. Included in the release is a `manifest.json` file that contains the following object:
    ```json
    {
      "Keyfactor:PAMProviders:Password-Manager-Pro:InitializationInfo": {
        "Host": "<Host>",
        "Authtoken": "<Authtoken>"
      }
    }
    ```

    Populate the fields in this object with credentials and configuration data collected in the [requirements](docs/password-manager-pro.md#requirements) section.

3. Restart the Universal Orchestrator service.


### Usage


#### From Keyfactor Command Host (Local)

##### Define a PAM provider in Command
1. In the Keyfactor Command Portal, hover over the ⚙️  (settings) icon in the top right corner of the screen and select **Priviledged Access Management**.

2. Select the **Add** button to create a new PAM provider. Click the dropdown for **Provider Type** and select **Password-Manager-Pro**.

> [!IMPORTANT]
> If you're running Keyfactor Command 11+, make sure `Remote Provider` is unchecked.

3. Populate the fields with the necessary information collected in the [requirements](docs/password-manager-pro.md#requirements) section:

| Initialization parameter | Display Name | Description |
| --- | --- | --- |
| Host | Vault Host | The IP address or URL of the PMP instance, including any port number |
| Authtoken | Authentication Token | The access token for the PMP Api user. |


4. Click **Save**. The PAM provider is now available for use in Keyfactor Command.

##### Using the PAM provider

Now, when defining Certificate Stores (**Locations**->**Certificate Stores**), **Password-Manager-Pro** will be available as a PAM provider option. When defining new Certificate Stores, the secret parameter form will display tabs for **Load From Keyfactor Secrets** or **Load From PAM Provider**. 

Select the **Load From PAM Provider** tab, choose the **Password-Manager-Pro** provider from the list of **Providers**, and populate the fields with the necessary information from the table below:

| Instance parameter | Display Name | Description |
| --- | --- | --- |
| resourceName | Resource Name | The Resource Name of the secret to retrieve as displayed in PMP |
| accountName | Account Name | The Account Name of the secret to retrieve as displayed in PMP |
| LookupType | Lookup Type | Must be either Username or Password. Defines which value the extension should retrieve |


#### From a Universal Orchestrator Host (Remote)


<details><summary>Keyfactor Command 11+</summary>

##### Define a remote PAM provider in Command

In Command 11 and greater, before using the Password-Manager-Pro PAM type, you must define a Remote PAM Provider in the Command portal.

1. In the Keyfactor Command Portal, hover over the ⚙️  (settings) icon in the top right corner of the screen and select **Priviledged Access Management**.

2. Select the **Add** button to create a new PAM provider.

3. Make sure that `Remote Provider` is checked.

4. Click the dropdown for **Provider Type** and select **Password-Manager-Pro**. 

5. Give the provider a unique name.

6. Click "Save".

##### Using the PAM provider

When defining Certificate Stores (**Locations**->**Certificate Stores**), **Password-Manager-Pro** can be used as a PAM provider. When defining a new Certificate Store, the secret parameter form will display tabs for **Load From Keyfactor Secrets** or **Load From PAM Provider**.

Select the **Load From PAM Provider** tab, choose the **Password-Manager-Pro** provider from the list of **Providers**, and populate the fields with the necessary information from the table below:

| Instance parameter | Display Name | Description |
| --- | --- | --- |
| resourceName | Resource Name | The Resource Name of the secret to retrieve as displayed in PMP |
| accountName | Account Name | The Account Name of the secret to retrieve as displayed in PMP |
| LookupType | Lookup Type | Must be either Username or Password. Defines which value the extension should retrieve |


</details>

<details><summary>Keyfactor Command 10</summary>

When defining Certificate Stores (**Locations**->**Certificate Stores**), **Password-Manager-Pro** can be used as a PAM provider.

When entering Secret fields, select the **Load From Keyfactor Secrets** tab, and populate the **Secret Value** field with the following JSON object:

```json
{"resourceName":"The Resource Name of the secret to retrieve as displayed in PMP","accountName":"The Account Name of the secret to retrieve as displayed in PMP","LookupType":"Must be either Username or Password. Defines which value the extension should retrieve"}

```

> We recommend creating this JSON object in a text editor, and copying it into the Secret Value field.

</details>


> [!NOTE]
> Additional information on Password-Manager-Pro can be found in the [supplemental documentation](docs/password-manager-pro.md).

## License

Apache License 2.0, see [LICENSE](LICENSE)

## Related Integrations

See all [Keyfactor PAM Provider extensions](https://github.com/orgs/Keyfactor/repositories?q=pam).
