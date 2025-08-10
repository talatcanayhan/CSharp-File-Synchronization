using System.Collections;
using System.IO;

#region Get Paths From User
static string GetPathFromUser(string pathType, bool isTextFile)
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
                Console.WriteLine("Directory does not exist. Try again. ");
                // Directory.CreateDirectory(path);
            }
            else if (isTextFile && !path.EndsWith(".txt"))
            {
                Console.WriteLine("Please enter a valid txt file. ");
            }
            else
            {
                break; // Valid path entered, exit loop
            }
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

string frequency;
string[] frequencyTypes = { "second", "minute", "hour", "day", "week" };


while (true)
{
    Console.Write("Please enter the frequency type(second, hour, day, week)): ");
    frequencyPreference = Console.ReadLine();

    if (!frequencyTypes.Contains(frequencyPreference.ToLower()))
    {
        Console.WriteLine("You have entered invalid frequency type. Try again.");
        continue;
    }

    Console.Write("Please enter the frequency (in integer): ");
    frequency = Console.ReadLine();


    if (int.TryParse(frequency, out int number))
    {
        Console.WriteLine($"The replica folder will be synchronized with source folder in every {number} {frequencyPreference}(s)");
        break;
    }
    else
    {
        Console.WriteLine("Please enter a valid integer number");
    }
}

switch (frequencyPreference)
{
    case "second":
        break;

    case "minute":
        break;

    case "hour":
        break;

    case "day":
        break;

    case "week":
        break;

    default:

        break;
}
#endregion