# kiwijam-2024
Submission for the KiwiJam Auckland Hackathon 2024

# Contribution Conventions

- Use pascal case (SomeFileNameLikeThis) for all folders and files in `Scenes` and `Scripts` folders. If the name is an acronym, use all upper case.

# Setting up your development environment

- Install [Godot 3.6.beta4.mono](https://godotengine.org/download/archive/3.6-beta4/).

- Create a symlink to the godot3.6.beta4.mono.exe and add its absolute path to your PATH environment variable.

    ```
    open cmd.exe and enter the following command:

    mklink godot3.6.4.exe Godot_v3.6-beta4_mono_win64.exe
    ```

- Install Powershell:

    ```
    winget install --id Microsoft.Powershell --source winget
    ```

- Open a Powershell terminal as Administrator and run the following commands:

    ```
    Set-ExecutionPolicy Unrestricted -Scope Process
    
    Set-ExecutionPolicy Unrestricted -Scope LocalMachine
    
    Set-ExecutionPolicy Unrestricted -Scope CurrentUser
    ```

- Download the [Large assets bundle](https://mega.nz/file/UwIEjZID#OZRhYEqhMU6m_A6Hjvr6-DVW04Oq_4vkjXwn-LQAMhw) and decompress it straight into the root folder.

# Deploying the project

- Make a folder in the root called `build`.

- Download and install [bunnydeploy.exe](https://github.com/DoubleCouponDay/bunny-deploy/releases/tag/1.0.0). Add it to your PATH environment variable.

- Create a [Bunny CDN](https://bunny.net/) and Bunny CDN Storage Zone.

    Ensure that the Storage Zones default replication zone is left as default.

    You should probably add the Sydney replication zone for speed.

- Create two environment variables: `KIWIJAM_BUNNY_ACCESS` containing your Bunny CDN API Key, `KIWIJAM_BUNNY_STORAGE` containing your read/write Bunny CDN StorageZone Password.

- Run the script `deploy\deploy.ps1`.