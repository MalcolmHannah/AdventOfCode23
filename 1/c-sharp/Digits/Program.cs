
var assemblyLocation = typeof(CalibrationLine).Assembly.Location;
string? exePath = Path.GetDirectoryName(assemblyLocation);
if (exePath == null)
{
    Console.WriteLine("Can't determine path of current executable.");
    return;
}

string inputFile = "input.txt";
string absoluteInputFile = Path.Combine(exePath, inputFile);
if (!File.Exists(absoluteInputFile))
{
    Console.WriteLine($"Can't find {absoluteInputFile}");
    return;
}
Console.WriteLine($"Processing {absoluteInputFile}");

var f = new CalibrationFile(absoluteInputFile);

Console.WriteLine($"{f.FileLines.Count()} lines, {f.LinesWithoutDigits} without digits.");
Console.WriteLine($"Total: {f.Total}");
