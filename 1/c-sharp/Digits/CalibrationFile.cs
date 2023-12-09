
/// <summary>
/// Represents a file containing lines with encoded calibration values.
/// </summary>
public class CalibrationFile
{
    /// <summary>
    /// All lines in the file.
    /// </summary>
    public readonly IEnumerable<string> FileLines;
    /// <summary>
    /// Sum of all calibration values.
    /// </summary>
    public readonly int Total = 0;
    /// <summary>
    /// Number of lines where no digits were found.
    /// </summary>
    public readonly int LinesWithoutDigits;
    /// <summary>
    /// Calibration value of each individual line.
    /// </summary>
    public readonly List<int> LineResults = new List<int>();

    public CalibrationFile(string absoluteInputFile)
    {
        FileLines = File.ReadLines(absoluteInputFile);

        foreach (string line in FileLines)
        {
            var calLine = new CalibrationLine(line.Trim());

            LineResults.Add(calLine.Result);

            if (calLine.Result > 0 && calLine.Result < 100)
            {
                // It's valid.
                Total += calLine.Result;
            }
            else
            {
                LinesWithoutDigits++;
            }
        }
    }
}
