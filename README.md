# kiwijam-2024
Submission for the KiwiJam Auckland Hackathon 2024

# Setting up your development environment

- Install Godot 3.6.beta4.mono.

- Create a symlink to the godot3.6.beta4.mono.exe and add its absolute path to your PATH environment variable.

    open cmd.exe and enter the following command:

    mklink godot3.6.4.exe Godot_v3.6-beta4_mono_win64.exe

- Install Powershell and enables scripts to be executed.

    winget install --id Microsoft.Powershell --source winget

    Set-ExecutionPolicy Unrestricted -Scope Process
    
    Set-ExecutionPolicy Unrestricted -Scope LocalMachine
    
    Set-ExecutionPolicy Unrestricted -Scope CurrentUser

# Deploying the project

- Download and install [bunnydeploy.exe](https://github.com/DoubleCouponDay/bunny-deploy/releases/tag/1.0.0). Add it to your PATH environment variable.

- Create a (Bunny CDN)[https://bunny.net/] and Bunny CDN StorageZone.

- Create two environment variables: `KIWIJAM_BUNNY_ACCESS` containing your Bunny CDN API Key, `KIWIJAM_BUNNY_STORAGE` containing your read/write Bunny CDN StorageZone Password.

- Run the script `deploy\deploy.ps1`.