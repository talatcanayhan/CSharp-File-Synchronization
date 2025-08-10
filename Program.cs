using System;
using System.IO;
using System.Linq;
using System.Timers;


// Check if the User initiated the program with valid command line arguments.
if (args.Length != 4)
{
    Console.WriteLine("Usage: program <source> <replica> <logfile> <interval_seconds>");
    return;
}

string sourcePath = args[0];
string replicaPath = args[1];
string logfilePath = args[2];

if (!int.TryParse(args[3], out int intervalSeconds) || intervalSeconds <= 0)
{
    Console.WriteLine("Invalid interval. Please provide a positive integer for seconds.");
    return;
}

// Validate source path exists
if (!Directory.Exists(sourcePath))
{
    Console.WriteLine($"Source directory does not exist: {sourcePath}");
    return;
}

// Validate log file path
try
{
    Directory.CreateDirectory(Path.GetDirectoryName(logfilePath));
}
catch (Exception ex)
{
    Console.WriteLine($"Invalid log file path: {ex.Message}");
    return;
}

// --- Setup timer and start synchronization ---
double intervalMilliseconds = intervalSeconds * 1000.0;

System.Timers.Timer syncTimer = new System.Timers.Timer(intervalMilliseconds);
syncTimer.AutoReset = true;

bool isSyncRunning = false;

syncTimer.Elapsed += (sender, e) =>
{
    if (isSyncRunning) return; // Skip if already running

    isSyncRunning = true;
    try
    {
        SyncFolders(sourcePath, replicaPath, logfilePath);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during synchronization: {ex.Message}");
    }
    finally
    {
        isSyncRunning = false;
    }
};

syncTimer.Start();

Console.WriteLine($"Synchronization started. Press Enter to stop and exit.");
Console.ReadLine();

syncTimer.Stop();
syncTimer.Dispose();

#region Synchronizing function
static void SyncFolders(string source, string replica, string logFilePath)
{
    // Ensure replica exists
    Directory.CreateDirectory(replica);

    using (StreamWriter logWriter = new StreamWriter(logFilePath, append: true))
    {
        // Helper for logging to console + file
        void Log(string message)
        {
            Console.WriteLine(message);
            logWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }

        // Copy new and updated files from source to replica
        foreach (var sourceFile in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
        {
            string relativePath = Path.GetRelativePath(source, sourceFile);
            string replicaFile = Path.Combine(replica, relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(replicaFile)!);

            if (!File.Exists(replicaFile) ||
                File.GetLastWriteTimeUtc(sourceFile) > File.GetLastWriteTimeUtc(replicaFile))
            {
                File.Copy(sourceFile, replicaFile, true);
                Log($"Copied: {relativePath}");
            }
        }

        // Remove files that are in replica but not in source
        foreach (var replicaFile in Directory.GetFiles(replica, "*", SearchOption.AllDirectories))
        {
            string relativePath = Path.GetRelativePath(replica, replicaFile);
            string sourceFile = Path.Combine(source, relativePath);

            if (!File.Exists(sourceFile))
            {
                File.Delete(replicaFile);
                Log($"Deleted: {relativePath}");
            }
        }

        // Remove empty directories in replica
        foreach (var dir in Directory.GetDirectories(replica, "*", SearchOption.AllDirectories))
        {
            if (Directory.GetFiles(dir).Length == 0 && Directory.GetDirectories(dir).Length == 0)
            {
                Directory.Delete(dir, true);
                Log($"Removed empty folder: {Path.GetRelativePath(replica, dir)}");
            }
        }
    }
}
#endregion