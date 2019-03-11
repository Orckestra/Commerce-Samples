# Clone the repository
git clone https://orckestra002.visualstudio.com/DefaultCollection/Hach/_git/Hach

# Checkout the develop branch
cd Hach
git checkout develop

# Restore Nuget packages
nuget restore Hach.sln

# MSBuild
& 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe' Hach.sln

# npm install
cd src/Hach.Website
npm install

# gulp build
gulp build

# Remove *.sln files from node_modules
Remove-Item ./node_modules/ -recurse -include *.sln

# Build
cd ../../build
./build.ps1 -t all

# Create tools folder
New-Item -ItemType directory -Path C:\tools

# Add db_admin user with sysadmin role to SQL Server
sqlcmd -Q "CREATE LOGIN db_admin WITH PASSWORD = 'P@ssw0rd'; EXEC sp_addsrvrolemember @loginame = 'db_admin', @rolename = 'sysadmin';"

Add-Type -AssemblyName System.IO.Compression.FileSystem
$ProgressPreference = 'SilentlyContinue'
$url = "http://bin.orckestra.com/Assets.zip"
		$output = "C:\tools\Assets.zip"
		$start_time = Get-Date
		Invoke-WebRequest -Uri $url -OutFile $output
$ProgressPreference = 'Continue'
[System.IO.Compression.ZipFile]::ExtractToDirectory($output, "c:\tools")

# Integration
./Integration.ps1 -t all
