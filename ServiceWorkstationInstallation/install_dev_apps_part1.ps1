#####
# This script installs various software needed on development machines.
# After the reboot of the machine, please run install2.ps1 to complete the installation.
#####

param (
	[string]$projectScript
)


# Install Chocolatey
iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

# Install browsers
choco install googlechrome -y
choco install firefox -y

# Install text editors
choco install notepadplusplus -y
choco install vscode -y

# Install Git and related software
choco install git -y
choco install sourcetree -y

# Install NodeJS
choco install nodejs -y

# Install Silverlight
choco install silverlight -y

# Install database-related software
choco install sql-server-management-studio -y
choco install sql-server-2017 --params="'/FEATURES:FullText'" -y

# Install Visual Studio and its workloads
choco install visualstudio2017enterprise --params "--add Microsoft.VisualStudio.Workload.Azure --add Microsoft.VisualStudio.Workload.NetWeb --add Microsoft.VisualStudio.Workload.Node --add Microsoft.VisualStudio.Workload.ManagedDesktop --add Microsoft.VisualStudio.Workload.Office --add Microsoft.VisualStudio.Workload.NativeDesktop --add Microsoft.VisualStudio.Component.VC.140 --add Microsoft.VisualStudio.Component.VC.Tools.x86.x64 --includeRecommended" -y

# Build.ps1 uses MsBuild 14.0 at the moment
# This is needed to update the MsBuild 14.0 that comes with VS2017
choco install microsoft-build-tools --version 14.0.25420.1 -y

# Install Nuget
choco install nuget.commandline -y

# Install utilities
choco install 7zip -y
choco install winmerge -y
choco install microsoftazurestorageexplorer -y
choco install servicebusexplorer -y

# Install Azure Powershell
choco install azurepowershell -y

# Install Visual C++ Runtime
choco install vcredist-all -y

# Install JDK
choco install jdk8 -y

# Install Python 2
choco install python2 -y

# Install IIS and related software
Install-WindowsFeature -name Web-Server -IncludeManagementTools
choco install urlrewrite -y
$iisFeatures = @(
  "IIS-DefaultDocument",
  "IIS-ASPNET45",
  "IIS-Security",
  "IIS-DirectoryBrowsing",
  "IIS-HttpErrors",
  "IIS-StaticContent",
  "IIS-HttpRedirect",
  "IIS-Performance",
  "IIS-NetFxExtensibility",
  "IIS-NetFxExtensibility45",
  "IIS-ISAPIExtensions",
  "IIS-ISAPIFilter",
  "IIS-ManagementConsole",
  "IIS-ManagementScriptingTools",
  "IIS-ManagementService",
  "IIS-HttpLogging",
  "IIS-RequestMonitor",
  "IIS-HttpTracing",
  "IIS-HttpCompressionDynamic",
  "IIS-BasicAuthentication",
  "IIS-ClientCertificateMappingAuthentication",
  "IIS-IISCertificateMappingAuthentication",
  "IIS-DigestAuthentication",
  "IIS-WindowsAuthentication",
  "IIS-URLAuthorization",
  "IIS-IPSecurity",
  "IIS-CertProvider",
  "WCF-HTTP-Activation45",
  "WCF-TCP-Activation45"
)
foreach ($iisFeature in $iisFeatures) {
	Write-Host "Installing " $iisFeature
	$result = Dism /online /get-featureinfo /featurename:$iisFeature
	if(-Not($result.Contains("State : Enabled"))) {
		Dism /Online /Enable-Feature /FeatureName:$iisFeature /All /NoRestart
	}
}

# Enable Search
Install-WindowsFeature Search-Service

#Set SQLSERVERAGENT to start automatic
Set-Service -Name "SQLSERVERAGENT" -StartupType automatic
net start SQLSERVERAGENT

#Disable Internet explorer enhance security
Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Active Setup\Installed Components\{A509B1A7-37EF-4b3f-8CFC-4F3A74704073}" -Name "IsInstalled" -Value 0
Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Active Setup\Installed Components\{A509B1A8-37EF-4b3f-8CFC-4F3A74704073}" -Name "IsInstalled" -Value 0

choco install webdeploy -y

#Prepare the part 2 script to be executed automaticallly
$RunOnceKey = "HKLM:\Software\Microsoft\Windows\CurrentVersion\RunOnce"
$workingDir = (split-path -Path $MyInvocation.MyCommand.Path)

if(![string]::IsNullOrEmpty($projectScript))
{
	set-itemproperty $RunOnceKey "NextRun" ("C:\Windows\System32\WindowsPowerShell\v1.0\Powershell.exe -executionPolicy Unrestricted -File $($workingDir)install_dev_apps_part2.ps1  -projectScript $($projectScript)")
}
else
{
	set-itemproperty $RunOnceKey "NextRun" ("C:\Windows\System32\WindowsPowerShell\v1.0\Powershell.exe -executionPolicy Unrestricted -File $($workingDir)install_dev_apps_part2.ps1")
}
#Reboot
Restart-Computer