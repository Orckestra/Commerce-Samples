#####
# This script continues the installation of various software needed on development machines.
# Please run this script after running install1.ps1.
#####

param (
	[string]$projectScript
)

$workingDir = (split-path -Path $MyInvocation.MyCommand.Path)
cd $workingDir

# Install npm packages
npm install -g npm
npm install -g gulp-cli

# Setup Authentication Method SQL Server
sqlcmd -Q "EXEC xp_instance_regwrite N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer', N'LoginMode', REG_DWORD, 2"
net stop mssqlserver /y
net start mssqlserver
net start SQLSERVERAGENT

if(![string]::IsNullOrEmpty($projectScript))
{
	Invoke-Expression -Command "$($workingDir)$($projectScript)"
}