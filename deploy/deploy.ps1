# Turn off echoing of commands
$VerbosePreference = 'SilentlyContinue'
$storagezonename = "fluffydice-storagezone3"

# Evaluate variables at runtime
Set-StrictMode -Version Latest

#Stop execution on error
$ErrorActionPreference = 'Stop'

#Get Keys
$accesskey = $env:KIWIJAM_BUNNY_ACCESS
$storagekey = $env:KIWIJAM_BUNNY_STORAGE

# Call other scripts
$outcome = Invoke-Expression -Command ".\deploy\_build.ps1"

if($outcome -ne 0) {
    Write-Host "build failed."
    Exit
}

Write-Host "loading all files to Bunny Storage Zone..."

# Load CDN endpoint
Start-Process -Wait "bunny-deploy" -ArgumentsList "deploy --storagekey" $storagekey "--storagezone" $storagezonename "./build"

Write-Host "purging all files from Bunny Endpoints..."
    
# Purge CDN endpoint
Start-Process -Wait "bunny-deploy" -ArgumentsList "purge --accesskey " $accesskey

# Pause the script execution to view the output
Read-Host -Prompt "Deployment complete. Press Enter to continue"