<h1 align="center" style="border-bottom: none">
    Password Manager Pro
</h1>

<p align="center">
  <!-- Badges -->
<img src="https://img.shields.io/badge/integration_status-prototype-3D1973?style=flat-square" alt="Integration Status: prototype" />
<a href="https://github.com/Keyfactor/password-manager-pro/releases"><img src="https://img.shields.io/github/v/release/Keyfactor/password-manager-pro?style=flat-square" alt="Release" /></a>
<img src="https://img.shields.io/github/issues/Keyfactor/password-manager-pro?style=flat-square" alt="Issues" />
<img src="https://img.shields.io/github/downloads/Keyfactor/password-manager-pro/total?style=flat-square&label=downloads&color=28B905" alt="GitHub Downloads (all assets, all releases)" />
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

TODO this section is required

## Support
The Password Manager Pro is open source and there is **no SLA**. Keyfactor will address issues as resources become available. Keyfactor customers may request escalation by opening up a support ticket through their Keyfactor representative.

> To report a problem or suggest a new feature, use the **[Issues](../../issues)** tab. If you want to contribute actual bug fixes or proposed enhancements, use the **[Pull requests](../../pulls)** tab.

## Getting Started

The Password Manager Pro is used by Command to resolve PAM-eligible credentials for Universal Orchestrator extensions and for accessing Certificate Authorities. When configured, Command will use the Password Manager Pro to retrieve credentials needed to communicate with the target system. There are two ways to install the Password Manager Pro, and you may elect to use one or both methods:

1. **Locally on the Keyfactor Command server**: PAM credential resolution via the Password Manager Pro will occur on the Keyfactor Command server each time an elegible credential is needed.
2. **Remotely On Universal Orchestrators**: When Jobs are dispatched to Universal Orchestrators, the associated Certificate Store extension assembly will use the Password Manager Pro to resolve eligible PAM credentials.

Before proceeding with installation, you should consider which pattern is best for your requirements and use case.

### Installation

> [!IMPORTANT]
> For the most up-to-date and complete documentation on how to install a PAM provider extension, please visit our [product documentation](https://software.keyfactor.com/Core-OnPrem/Current/Content/ReferenceGuide/Preparing%20Third%20Party%20PAM%20Providers%20to%20Work%20with.htm?Highlight=pam%20provider#InstallingCustomPAMProviderExtensions)


To install Password Manager Pro, it is recommended you install [kfutil](https://github.com/Keyfactor/kfutil). `kfutil` is a command-line tool that simplifies the process of creating PAM Types in Keyfactor Command.



#### Requirements
   TODO Requirements is a required section

#### Create PAM type in Keyfactor Command


##### Using `kfutil`
Create the required PAM Types in the connected Command platform.

```shell
# Password-Manager-Pro
kfutil pam-types create -r password-manager-pro -n Password-Manager-Pro
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


TODO Platform Install is an optional section. If this section doesn't seem necessary, please delete it.


#### Install PAM provider on a Universal Orchestrator Host (Remote)


TODO Orchestrator Install is an optional section. If this section doesn't seem necessary, please delete it.



### Usage


#### From Keyfactor Command Host (Local)

TODO Platform Usage is an optional section. If this section doesn't seem necessary, please delete it.
#### From a Universal Orchestrator Host (Remote)

TODO Orchestrator Usage is an optional section. If this section doesn't seem necessary, please delete it.
> [!NOTE]
> Additional information on Password-Manager-Pro can be found in the [supplemental documentation](docs/password-manager-pro.md).

## License

Apache License 2.0, see [LICENSE](LICENSE)

## Related Integrations

See all [Keyfactor PAM Provider extensions](https://github.com/orgs/Keyfactor/repositories?q=pam).
