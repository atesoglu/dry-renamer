using System.Globalization;
using System.Text.RegularExpressions;

if (args.Length == 0)
{
    Console.WriteLine("Usage: dry-rename <folderPath> [--dry-run | -d | -preview]");
    return;
}

var targetDirectory = args[0];
var isDryRun = args.Contains("--dry-run") || args.Contains("-d") || args.Contains("-preview");

if (!Directory.Exists(targetDirectory))
{
    Console.WriteLine($"Directory does not exist: {targetDirectory}");
    return;
}

var files = Directory.GetFiles(targetDirectory);

foreach (var filePath in files)
{
    var fileName = Path.GetFileName(filePath);
    var newFileName = CleanFileName(fileName);

    if (fileName != newFileName)
    {
        var newPath = Path.Combine(targetDirectory, newFileName);

        if (isDryRun)
        {
            Console.WriteLine($"[DRY RUN] Would rename: {fileName} -> {newFileName}");
        }
        else
        {
            Console.WriteLine($"Renaming: {fileName} -> {newFileName}");
            try
            {
                if (File.Exists(newPath))
                {
                    File.Delete(newPath);
                }

                File.Move(filePath, newPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error renaming '{fileName}': {ex.Message}");
            }
        }
    }
}

string CleanFileName(string fileName)
{
    var extension = Path.GetExtension(fileName);
    var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

    // Remove 'sanet.st' and similar patterns
    nameWithoutExtension = DomainNameOrSimilarPatternsRegex().Replace(nameWithoutExtension, "");

    // Replace multiple underscores with a space
    nameWithoutExtension = MultipleUnderscoresWithASpaceRegex().Replace(nameWithoutExtension, " ");

    // Trim whitespace
    nameWithoutExtension = nameWithoutExtension.Trim();

    // Clean ending
    nameWithoutExtension = CleanEndingRegex().Replace(nameWithoutExtension, "");

    // Convert to Title Case
    nameWithoutExtension = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nameWithoutExtension.ToLower());

    return nameWithoutExtension + extension;
}

internal partial class Program
{
    [GeneratedRegex(@"_+")]
    private static partial Regex MultipleUnderscoresWithASpaceRegex();
    
    [GeneratedRegex(@"(?i)(sanet\.st|[\s_]*sanet[\s_]*\.?st)", RegexOptions.IgnoreCase, "en-NL")]
    private static partial Regex DomainNameOrSimilarPatternsRegex();
    
    [GeneratedRegex("^[@#$%^&*.-]+|[@#$%^&*.-]+$")]
    private static partial Regex CleanEndingRegex();
}