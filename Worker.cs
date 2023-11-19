using System.Diagnostics;

namespace Gitler;

public class GitCommitAndPushService : BackgroundService
{
    // private readonly TimeSpan _runInterval = TimeSpan.FromDays(1); // Run once a day
    private readonly TimeSpan _runInterval = TimeSpan.FromMinutes(5); // Run every 5 minutes
    private readonly string _repoPath = @"C:\path\to\your\repo"; // Your repository path

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Run Git commands
                RunGitCommand("add .");
                RunGitCommand($"commit -m \"Auto-commit on {DateTime.Now:yyyy-MM-dd HH:mm:ss}\"");
                RunGitCommand("push");
            }
            catch (Exception ex)
            {
                // Log exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            await Task.Delay(_runInterval, stoppingToken); // Wait for 24 hours
        }
    }

    private void RunGitCommand(string gitCommand)
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
                WorkingDirectory = _repoPath
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
