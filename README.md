# Commerce Samples
This repository provides samples for the Orckestra Commerce Cloud (OCC). 

## Getting Started


### Workstation Installation Scripts

There are three powershell (.ps1) scripts located in the ~/ServiceWorkstationInstallation folder:

1. install_dev_apps_part1.ps1
2. install_dev_apps_part2.ps1
3. install-project.ps1

#### _Install_dev_apps_part{x}.ps1_
These are scripts that should install all programs, licenses, and extensions necessary to start developing locally with the OCC.

#### install-project.ps1 
This is a powershell script that is meant to be configured by a Tech Lead, then pushed to the main client repo for development. It covers the following items:

- git clone
- develop brnach checkout
- MSBuild process
- npm install
- gulp build
- Removal of *.sln files from node_modules
- Execution of build script.
- Execution of integration script.

### PowerShell Installation

For more information on the installation, please see the Developer Portal link here.
