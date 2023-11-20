## Instructions - Gitler Sync Service

### Publish the app as a self-contained app
`dotnet publish --runtime win-x64 --configuration Release --self-contained true`

### Create the service
After publishing, use the sc.exe command to create a new Windows Service that points to your executable. You'll need to open the Command Prompt as an Administrator and run:

`sc create "GitBackgroundService" binPath= "C:\path\to\your\published\app\YourApp.exe"`

Replace C:\path\to\your\published\app\YourApp.exe with the actual path to your published executable.

### Configure the Service Start Type

Configure the service to start automatically on system startup:

`sc config "GitBackgroundService" start= auto`

### Start the service
Start the service using the Services management console (services.msc) or with the following command:

`sc start "GitBackgroundService"`

### Handle User Permissions (Important):
Ensure the service runs under a user account that has the necessary permissions to access the Git repository and perform the required Git operations.

Remember to test your service thoroughly to make sure it works as expected when the system starts. Also, keep in mind that managing credentials securely is crucial. Avoid storing credentials in plain text and consider using environment variables, Windows Credential Store, or other secure credential management strategies.

## Instructions - Gitler Sync PowerShell Module

### Move the module to the PowerShell Modules folder
#### Windows (usually this, or similar)
`C:\Users\<username>\Documents\PowerShell\Modules\Gitler\Gitler.psm1`
Make sure to add the necessary folder structure if it doesn't exist.

#### Linux (usually this, or similar)
`/home/<username>/.local/share/powershell/Modules/Gitler/Gitler.psm1`

### Adjust the module script paths accordingly

## Import the module
`Import-Module Gitler`
Move this to your PowerShell profile if you want to import it automatically.

## Run the module
Run `Gitler-AddDirectory` to git directories you want to sync

Run `Gitler-RemoveDirectory` to remove directories you no longer want to sync