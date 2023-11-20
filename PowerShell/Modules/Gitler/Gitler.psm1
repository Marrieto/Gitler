function Gitler-AddDirectory {
    $currentPath = Get-Location
    $filePath = "/home/martin/Dev/Gitler/Gitler/listOfDirectories.txt"
    # This will ensure that $currentPath will be on a new line
    # by prepending it with a newline character if the file exists and is not empty.
    $newline = if ((Test-Path -Path $filePath) -and ((Get-Content -Path $filePath).Count -gt 0)) { "`n" } else { "" }
    Add-Content -Path $filePath -Value ($newline + $currentPath)
}

function Gitler-RemoveDirectory {
    $currentPath = Get-Location
    $filePath = "/home/martin/Dev/Gitler/Gitler/listOfDirectories.txt"
    $fileContent = Get-Content -Path $filePath
    # Filter out the line that matches the current path
    $updatedContent = $fileContent | Where-Object { $_ -ne $currentPath }
    # Write the updated content back to the file
    Set-Content -Path $filePath -Value $updatedContent
}

Export-ModuleMember -Function 'Gitler-AddDirectory'
Export-ModuleMember -Function 'Gitler-RemoveDirectory'
