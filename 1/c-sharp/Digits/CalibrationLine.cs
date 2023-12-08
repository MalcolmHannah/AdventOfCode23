/// <summary>
/// Processes a line from a calibration file.
/// </summary>
public class CalibrationLine
{
    /// <summary>
    /// One line from the calibration file.
    /// </summary>
    string _text;

    static readonly IReadOnlyList<string> _digitNames = new List<string> 
    {
        "zero",
        "one",
        "two",
        "three",
        "four",
        "five",
        "six",
        "seven",
        "eight",
        "nine"
    }
    .AsReadOnly();

    public const int NotFound = -1;

    /// <summary>
    /// First digit in the calibration line.
    /// </summary>
    public int First { get; init; } = 0;

    /// <summary>
    /// Zero-based index of first digit, or NotFound.
    /// </summary>
    public int FirstIndex {  get; init; }

    /// <summary>
    /// Final digit in the calibration line.
    /// </summary>
    public int Final { get; init; } = 0;
    
    /// <summary>
    /// Zero-based index of final digit, or NotFound.
    /// </summary>
    public int FinalIndex { get; init; }

    /// <summary>
    /// Result of converting found digits into a two-digit number.
    /// </summary>
    public int Result { get; init; } = NotFound;


    public CalibrationLine(string text)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        _text = text;

        // Ensure first digit we find becomes the current "winner".
        FirstIndex = int.MaxValue;
        // Ditto for backward search.
        FinalIndex = NotFound;

        for (int digit = 1; digit < 10; digit++)
        {
            int index = FindFirst(digit);
            if (index != NotFound && index < FirstIndex)
            {
                First = digit;
                FirstIndex = index;
            }

            index = FindFinal(digit);
            if (index != NotFound && index > FinalIndex)
            {
                Final = digit;
                FinalIndex = index;
            }
        }

        Result = (First * 10) + Final;
    }

    /// <summary>
    /// Find first occurrence of the specified digit, considering
    /// numeric and verbal instances, e.g.both "7" and "seven".
    /// </summary>
    /// <param name="digit">Digit to find.</param>
    /// <returns>
    /// Zero-based index of first occurrence, or <see cref="NotFound"/>
    /// if not found.
    /// </returns>
    private int FindFirst(int digit)
    {
        var textToFind = digit.ToString();
        var firstDigitIndex = _text.IndexOf(textToFind);
        
        textToFind = _digitNames[digit];
        var firstNameIndex = _text.IndexOf(textToFind);

        if (firstDigitIndex == NotFound && firstNameIndex == NotFound)
        {
            return NotFound;
        }

        if (firstDigitIndex == NotFound && firstNameIndex != NotFound)
        {
            return firstNameIndex;
        }

        if (firstDigitIndex != NotFound && firstNameIndex == NotFound)
        {
            return firstDigitIndex;
        }

        return (firstDigitIndex < firstNameIndex) ?
            firstDigitIndex :
            firstNameIndex;
    }

    /// <summary>
    /// Find final occurrence of the specified digit, considering
    /// numeric and verbal instances, e.g.both "6" and "six".
    /// </summary>
    /// <param name="digit">Digit to find.</param>
    /// <returns>
    /// Zero-based index of final occurrence, or <see cref="NotFound"/>
    /// if not found.
    /// </returns>
    private int FindFinal(int digit)
    {
        var textToFind = digit.ToString();
        var finalDigitIndex = _text.LastIndexOf(textToFind);

        textToFind = _digitNames[digit];
        var finalNameIndex = _text.LastIndexOf(textToFind);

        if (finalDigitIndex == NotFound && finalNameIndex == NotFound)
        {
            return NotFound;
        }

        if (finalDigitIndex == NotFound && finalNameIndex != NotFound)
        {
            return finalNameIndex;
        }

        if (finalDigitIndex != NotFound && finalNameIndex == NotFound)
        {
            return finalDigitIndex;
        }

        return (finalDigitIndex > finalNameIndex) ?
            finalDigitIndex :
            finalNameIndex;
    }
}
