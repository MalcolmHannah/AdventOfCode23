#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>
#include <ctype.h>

#define INT_FROM_DIGIT_CHAR(x) (int)((x) - '0')
#define NOT_FOUND (-42)


typedef struct stats_s
{
    int lineCount;
    int linesWithDigits;
    int linesWithoutDigits;
    int total;
}
stats_t;


static int wordToDigit(const char *text)
{
    typedef struct digit_s
    {
        const char *text;
        size_t length;
    }
    digit_t;

    static digit_t digitMap[10] =
    {
        {"", 0}, // Not used, but lets each index match its digit.
        {"one", 3}, 
        {"two", 3},
        {"three", 5},
        {"four", 4},
        {"five", 4},
        {"six", 3},
        {"seven", 5},
        {"eight", 5},
        {"nine", 4}
    };

    // Start at 1, as "zero" is not allowed.
    for (int i = 1; i < 10; i++)
    {
        if (0 == strncmp(text, digitMap[i].text, digitMap[i].length))
        {
            return i;
        }
    }

    return NOT_FOUND;
}


/// @brief Read file into memory.
/// @param name Full path and file name.
/// @return Address of buffer holding file content.  Caller must free.
static uint8_t *readEntireFile(const char *name, size_t *bufferByteCount)
{
    int error = 0;
    uint8_t *entireFile = NULL;

    FILE *f = fopen(name, "rb");
    if (f == NULL)
    {
        printf("Can't open %s\n", name);
        goto exit;
    }

    size_t numBytes = 0;
    error = fseek(f, 0l, SEEK_END);
    if (error == 0)
    {
        numBytes = (size_t)ftell(f);
        // Rewind back to start.
        error = fseek(f, 0l, SEEK_SET);
    }

    if (error)
    {
        printf("Could not get size of %s", name);
        goto exit;
    }

    // Allocate one extra byte so we can treat buffer like a terminated string.
    *bufferByteCount = numBytes + 1;
    entireFile = malloc(*bufferByteCount);
    if (entireFile == NULL)
    {
        printf("Failed to allocate %u bytes\n", (unsigned int)*bufferByteCount);
        goto exit;
    }

    size_t bytesRead = fread(entireFile, sizeof(uint8_t), numBytes, f);
    if (bytesRead != numBytes)
    {
        printf("Error.  Read %u bytes instead of %u bytes",
            (unsigned int)bytesRead,
            (unsigned int)numBytes);
        free(entireFile);
        entireFile = NULL;
        goto exit;
    }

    // Terminate the string.
    entireFile[numBytes] = 0;

exit:
    if (f)
    {
        fclose(f);
    }
    return entireFile;
}


/// @brief Extract digits per line and sum over all lines.
///
/// On each line, the first and final digit are treated respectively as the 
/// tens and units of a two-digit number.  E.g., abc1def2ghi3jk gives 1 and 3,
/// which gives 13.  If there is only one digit, it is both the first and 
/// final digit.
///
/// @param content Entire content of file, as single string.
/// @param stats On return, holds count of lines etc.
static void findDigits(const char *content, stats_t *stats)
{
    const char *cursor;
    int firstDigit = NOT_FOUND;
    int finalDigit = NOT_FOUND;
    int lineValue = 0;

    stats->linesWithDigits = 0;
    stats->linesWithoutDigits = 0;
    stats->lineCount = 0;
    stats->total = 0;

    for (cursor = content; /* for ever */; cursor++)
    {
        char currentChar = *cursor;

        if (currentChar == '\0')
        {
            // End of content.
            break;
        }

        if (currentChar == '\n')
        {
            stats->lineCount++;
            if (firstDigit == NOT_FOUND)
            {
                stats->linesWithoutDigits++;
            }
            else
            {
                stats->linesWithDigits++;
                lineValue = (firstDigit * 10) + finalDigit;
                stats->total += lineValue;
            }
            // Reset, ready for next line
            firstDigit = finalDigit = NOT_FOUND;
            continue;
        }

        int thisDigit = NOT_FOUND;

        if (isdigit(currentChar))
        {
            thisDigit = INT_FROM_DIGIT_CHAR(currentChar);
        }
        else
        {
            thisDigit = wordToDigit(cursor);
        }

        if (thisDigit != NOT_FOUND)
        {
            if (firstDigit == NOT_FOUND)
            {
                firstDigit = thisDigit;
            }
            finalDigit = thisDigit;
        }
    }
}


int main(void)
{
    size_t bufferByteCount = 0;
    uint8_t *entireFile = readEntireFile("../input.txt", &bufferByteCount);
    if (entireFile == NULL)
    {
        return 10;
    }

    stats_t stats;

    findDigits((char *)entireFile, &stats);

    printf(
        "%d lines (%d with digits, %d without)\nTotal: %d\n",
        stats.lineCount,
        stats.linesWithDigits,
        stats.linesWithoutDigits,
        stats.total);
    
    free(entireFile);
    return 0;
}
