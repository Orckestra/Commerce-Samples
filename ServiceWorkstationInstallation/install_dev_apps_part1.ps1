#####
# This script installs various software needed on development machines.
# After the reboot of the machine, please run install2.ps1 to complete the installation.
#####

param (
	[string]$projectScript
)

$ErrorActionPreference="SilentlyContinue"
Stop-Transcript | out-null
$ErrorActionPreference = "Continue"
Start-Transcript -path C:\install_result.txt -append

# Install Chocolatey
Set-ExecutionPolicy Bypass -Scope Process -Force;
Invoke-Expression -Command ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'));

# Install IIS and related software
Install-WindowsFeature -name Web-Server -IncludeManagementTools
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
	$resultIIsInstall = Dism /online /get-featureinfo /featurename:$iisFeature
	if(-Not($resultIIsInstall.Contains("State : Enabled"))) {
		Dism /Online /Enable-Feature /FeatureName:$iisFeature /All /NoRestart
	}
}

choco install urlrewrite -y --ignore-detected-reboot

# Install Python 2
choco install python2 -y --ignore-detected-reboot

# Install browsers
choco install googlechrome -y --ignore-detected-reboot
choco install firefox -y --ignore-detected-reboot

# Install text editors
choco install notepadplusplus -y --ignore-detected-reboot
choco install vscode -y --ignore-detected-reboot

# Install Git and related software
choco install git -y --ignore-detected-reboot
choco install git-fork -y --ignore-detected-reboot

# Install NodeJS
choco install nodejs --version 10.16.3 -y --ignore-detected-reboot

# Install Silverlight
choco install silverlight -y --ignore-detected-reboot

# Install Visual Studio and its workloads
# Set timeout to 2h = 7200 seconds
choco install visualstudio2019professional --norestart --execution-timeout=7200 --wait  --ignore-detected-reboot --params "--add Microsoft.VisualStudio.Workload.Azure --add Microsoft.VisualStudio.Workload.NetWeb --add Microsoft.VisualStudio.Workload.Node --add Microsoft.VisualStudio.Workload.ManagedDesktop --add Microsoft.VisualStudio.Workload.Office --add Microsoft.VisualStudio.Workload.NativeDesktop --add Microsoft.VisualStudio.Component.VC.140 --add Microsoft.VisualStudio.Component.VC.Tools.x86.x64 --add Microsoft.Net.Component.4.6.1.SDK --add Microsoft.Net.Component.4.6.1.TargetingPack --add Microsoft.Net.Component.4.7.1.TargetingPack --includeRecommended" -y

# Build.ps1 uses MsBuild 14.0 at the moment
# This is needed to update the MsBuild 14.0 that comes with VS2017
choco install microsoft-build-tools --version 14.0.25420.1 -y --ignore-detected-reboot

# Install Nuget
choco install nuget.commandline -y --ignore-detected-reboot

# Install utilities
choco install 7zip -y --ignore-detected-reboot
choco install winmerge -y --ignore-detected-reboot
choco install microsoftazurestorageexplorer -y --ignore-detected-reboot
choco install servicebusexplorer -y --ignore-detected-reboot

# Install Azure Powershell
choco install azurepowershell -y --ignore-detected-reboot

# Install Visual C++ Runtime
#choco install vcredist-all -y --ignore-detected-reboot

# Install JDK
choco install jdk8 -y --ignore-detected-reboot

# Enable Search
Install-WindowsFeature Search-Service

# Install database-related software
choco install sql-server-management-studio -y --ignore-detected-reboot
choco install sql-server-2017 --params="'/FEATURES:FullText'" -y --ignore-detected-reboot

#Set SQLSERVERAGENT to start automatic
Set-Service -Name "SQLSERVERAGENT" -StartupType automatic
net start SQLSERVERAGENT

#Disable Internet explorer enhance security
Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Active Setup\Installed Components\{A509B1A7-37EF-4b3f-8CFC-4F3A74704073}" -Name "IsInstalled" -Value 0
Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Active Setup\Installed Components\{A509B1A8-37EF-4b3f-8CFC-4F3A74704073}" -Name "IsInstalled" -Value 0

choco install webdeploy -y --ignore-detected-reboot

# Create tools folder
New-Item -ItemType directory -Path C:\tools

#Install cosmodb
$ProgressPreference = 'SilentlyContinue'
$cosmoUrl = "https://aka.ms/cosmosdb-emulator"
		$outputComsmoFile = "C:\tools\azure-cosmodb-emulator.msi"
		$start_time = Get-Date
		Invoke-WebRequest -Uri $cosmoUrl -OutFile $outputComsmoFile 
$ProgressPreference = 'Continue'

Start-Process msiexec.exe -Wait -ArgumentList '/I c:\tools\azure-cosmodb-emulator.msi /passive'

#Install Beta version of Git credential manager for windows to fix authentication error
$ProgressPreference = 'SilentlyContinue'
$gcmUrl = "https://github.com/microsoft/Git-Credential-Manager-Core/releases/download/v2.0.124-beta/gcmcore-win-x64-2.0.124.60385.exe"
		$outputgcmFile = "C:\tools\gcmcore-win-x64-2.0.124.60385.exe"
		$start_time = Get-Date
		Invoke-WebRequest -Uri $gcmUrl -OutFile $outputgcmFile 
$ProgressPreference = 'Continue'
Start-Process C:\tools\gcmcore-win-x64-2.0.124.60385.exe -Wait -ArgumentList '/VERYSILENT /NORESTART /CLOSEAPPLICATIONS'

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

Stop-Transcript