$VerbosePreference = 'SilentlyContinue'

Write-Host "building client project..."
$process = Start-Process -Wait "godot3.6.4.exe" -ArgumentList "project.godot --export HTML5 build/index.html --no-window --quit" -NoNewWindow -PassThru #gets stuck sometimes and you have to restart the deployment
Write-Host "outcome:" $process.ExitCode
return $process.ExitCode
