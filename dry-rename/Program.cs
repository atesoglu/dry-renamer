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
var nameCollisionTracker = new Dictionary<string, int>();

foreach (var filePath in files)
{
    var fileName = Path.GetFileName(filePath);
    var updatedFileName = CleanFileName(fileName);
    var updatedPath = Path.Combine(targetDirectory, updatedFileName);

    if (fileName.Equals(updatedFileName, StringComparison.InvariantCultureIgnoreCase))
    {
        // No rename needed
        continue;
    }

    if (isDryRun)
    {
        Console.WriteLine($"[DRY RUN] Would rename: {fileName} -> {updatedFileName}");
    }
    else
    {
        try
        {
            var finalFileName = updatedFileName;
            var finalPath = updatedPath;

            while (File.Exists(finalPath))
            {
                // Track the number of duplicates seen for this base file
                if (!nameCollisionTracker.TryAdd(updatedFileName, 1))
                {
                    nameCollisionTracker[updatedFileName]++;
                }

                var count = nameCollisionTracker[updatedFileName];
                var nameWithoutExt = Path.GetFileNameWithoutExtension(updatedFileName);
                var ext = Path.GetExtension(updatedFileName);

                finalFileName = $"DUP-{count}_{nameWithoutExt}{ext}";
                finalPath = Path.Combine(targetDirectory, finalFileName);
            }

            File.Move(filePath, finalPath);

            Console.WriteLine($"✅ Renaming: {fileName} -> {finalFileName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error renaming '{fileName}': {ex.Message}");
        }
    }
}

return;

string CleanFileName(string fileName)
{
    var extension = Path.GetExtension(fileName);
    var filename = Path.GetFileNameWithoutExtension(fileName);

    // Remove 'sanet.st' and similar patterns
    filename = DomainNameOrSimilarPatternsRegex().Replace(filename, "");

    // Replace multiple underscores with a space
    filename = MultipleUnderscoresWithASpaceRegex().Replace(filename, " ");

    // Trim whitespace
    filename = filename.Trim();

    // Clean ending
    filename = CleanStartAndEndRegex().Replace(filename, "");

    // Convert to Title Case
    filename = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(filename.ToLower());

    return filename + extension;
}

internal partial class Program
{
    [GeneratedRegex(@"_+")]
    private static partial Regex MultipleUnderscoresWithASpaceRegex();

    [GeneratedRegex(@"(?i)(sanet\.(st|cd|me)|[\s_]*sanet[\s_]*\.?(st|cd|me))", RegexOptions.IgnoreCase, "en-NL")]
    private static partial Regex DomainNameOrSimilarPatternsRegex();

    [GeneratedRegex(@"^[@#$%^&*.\-]+|[@#$%^&*.\-]+$", RegexOptions.None, "en-NL")]
    private static partial Regex CleanStartAndEndRegex();
}