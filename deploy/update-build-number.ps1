<#
.SYNOPSIS
Sets buildnumber from version variables

.DESCRIPTION
This script is intended to be used in a azure-pipeline.yml script to set build number from version variables.
The build number format is {$Version}.{$Build.BuildId}
#>

# These enviroment variables is used by the script, can be uncommented and set to test the script
<#
$env:VERSION = '1.2.3-rc1'
$env:BUILD_BUILDID = '123'
#>

# Print out current enviroment variables for debug
Get-ChildItem Env:

$BuildNumber = "$env:VERSION.$env:BUILD_BUILDID"

# This part sets the build number, see https://github.com/microsoft/azure-pipelines-tasks/blob/master/docs/authoring/commands.md
Write-Host "##vso[build.updatebuildnumber]$BuildNumber"

# Print out build number for debug
Write-Host "BuildNumber = $BuildNumber"
