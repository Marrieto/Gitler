using System.Diagnostics;

namespace Gitler;

public class GitCommitAndPushService : BackgroundService
{
    private readonly TimeSpan _runInterval = TimeSpan.FromMinutes(5); // Run every 5 minutes

    private const string FileName = "listOfDirectories.txt";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Create the file in the current executing directory if it doesn't exist
                CreateFileIfNotExist();

                // Get the list of directories from the file
                var directories = File.ReadAllLines(FileName);

                // Loop through the directories
                foreach (var directory in directories)
                {
                    // Run Git commands
                    RunGitCommand("add .", directory);
                    RunGitCommand($"commit -m \"Auto-commit on {DateTime.Now:yyyy-MM-dd HH:mm:ss}\"", directory);
                    RunGitCommand("push", directory);
                }
            }
            catch (Exception ex)
            {
                // Log exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            await Task.Delay(_runInterval, stoppingToken);
        }
    }

    private static void CreateFileIfNotExist()
    {
        if (!File.Exists(FileName))
        {
            File.WriteAllText(FileName, "");
        }
    }

    private void RunGitCommand(string gitCommand, string gitDirectory)
    {
        using (var process = new Process())
        {
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = gitCommand,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = gitDirectory
            };

            process.Start();

            // Read the output (or errors)
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!string.IsNullOrEmpty(errors))
            {
                throw new InvalidOperationException(errors);
            }

            // Optionally, log the output
            Console.WriteLine(output);
        }
    }
}