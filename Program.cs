using System.Collections;
using System.IO;

#region Get Paths From User
static string GetPathFromUser(string pathType)
{
    string path;
    while (true)
    {
        Console.Write($"Please enter the {pathType} path: ");
        path = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine("Path cannot be empty. Try again.");
            continue;
        }

        try
        {
            // Normalize the path
            path = Path.GetFullPath(path);

            // Check if path is valid by trying to get full path, 
            // and optionally check if it's accessible or creatable
            if (!Directory.Exists(path))
            {
                Console.WriteLine("Directory does not exist. Creating it...");
                Directory.CreateDirectory(path);
            }

            break; // Valid path entered, exit loop
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Invalid path: {ex.Message}. Try again.");
        }

    }

    return path;
}

string sourcePath = GetPathFromUser("source");
string replicaPath = GetPathFromUser("replica");
string logfilePath = GetPathFromUser("logfile");
#endregion

#region Get Synchronization Frequency From The User
string frequencyPreference = "";
string[] frequencyOptions = { "minutely, hourly", "daily", "weekly" };

Console.Write("Please enter the frequency which you prefer the synchronization be(hourly, daily, weekly)): ");
while (true)
{
    if (frequencyOptions.Contains(frequencyPreference.ToLower()))
    {
        break;
    }

    else
    {
        Console.WriteLine("You have entered invalid frequency. Try again.");
    }
}

switch (frequencyPreference)
{
    case "minutely":
        break;

    case "hourly":
        break;

    case "daily":
        break;

    case "weekly":
        break;

    default:

        break;
}
#endregion