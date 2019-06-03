<#
.SYNOPSIS
Extracts version from source branch

.DESCRIPTION
This script is intended to be used in a azure-pipeline.yml script to extract version from source branch.
If not building from a release tag (eg branch refs/tags/v*.*.*) a default version is created based on
the current date and the source version hash.
#>

# These enviroment variables is used by the script, can be uncommented and set to test the script
<#
$env:BUILD_SOURCEBRANCH = 'refs/tags/v1.2.3-rc4'
$env:BUILD_SOURCEBRANCHNAME = 'v1.2.3-rc4'
$env:BUILD_SOURCEVERSION = '70b9942ae5ecd793746a0d6417957115c291038b'
#>

# Print out current enviroment variables for debug
Get-ChildItem Env:

if ($env:BUILD_SOURCEBRANCH.StartsWith('refs/tags/', 'OrdinalIgnoreCase'))
{
    if ($env:BUILD_SOURCEBRANCHNAME -match "^v(?<VersionPrefix>\d+\.\d+\.\d+)(-(?<VersionSuffix>.*))?$")
    {
        $Version = $env:BUILD_SOURCEBRANCHNAME.Substring(1)
        $VersionPrefix = ([Version]$Matches['VersionPrefix']).ToString(3)
        $VersionSuffix = $Matches['VersionSuffix']
    }
    else
    {
        throw 'Invalid version tag, build can not continue'
    }
}
else
{
    $now = (Get-Date).ToUniversalTime()
    $VersionPrefix = ([Version]$now.ToString("0.yyyy.MMdd.HHmm")).ToString(4)
    $VersionSuffix = "ci$($env:BUILD_SOURCEVERSION.SubString(0,7))"
    $Version = $VersionPrefix + '-' + $VersionSuffix
}

# This part sets the version variables, see https://docs.microsoft.com/en-us/azure/devops/pipelines/process/variables
Write-Host "##vso[task.setvariable variable=VersionPrefix;]$VersionPrefix"
Write-Host "##vso[task.setvariable variable=VersionSuffix;]$VersionSuffix"
Write-Host "##vso[task.setvariable variable=Version;]$Version"

# Print out versions used for debug
Write-Host "VersionPrefix = $VersionPrefix"
Write-Host "VersionSuffix = $VersionSuffix"
Write-Host "Version = $Version"
