#####
# This script continues the installation of various software needed on development machines.
# Please run this script after running install1.ps1.
#####

param (
	[string]$projectScript
)

# Install npm packages
npm install -g npm
npm install -g gulp-cli
npm install --global windows-build-tools
RoboCopy "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Microsoft\VisualStudio\v16.0\WebApplications" "C:\Program Files (x86)\MSBuild\Microsoft\VisualStudio\v14.0\WebApplications" *.* 

# Setup Authentication Method SQL Server
sqlcmd -Q "EXEC xp_instance_regwrite N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer', N'LoginMode', REG_DWORD, 2"
sqlcmd -Q "EXEC sp_configure 'show advanced options', 1; RECONFIGURE; EXEC sp_configure 'max server memory', 2048; RECONFIGURE;"

net stop mssqlserver /y
net start mssqlserver
net start SQLSERVERAGENT

if(![string]::IsNullOrEmpty($projectScript))
{
	$workingDir = (split-path -Path $MyInvocation.MyCommand.Path)
	Invoke-Expression -Command "$($workingDir)$($projectScript)"
}