# Nuget pack
dotnet build
dotnet pack

# Installation
dotnet tool install --tool-path c:\files\dotnet-tools --add-source .\nupkg\ MigrationNameGenerator
path c:\files\dotnet-tools

# Uninstallation
dotnet tool uninstall --tool-path c:\files\dotnet-tools MigrationNameGenerator

# Usage
gr migrate
grb migrateandupdatedatabase