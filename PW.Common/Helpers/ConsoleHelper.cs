using PW.Extensions;
using System;
using static System.Console;

namespace PW.Helpers;

/// <summary>
/// Helper methods for use with <see cref="Console"/>
/// </summary>
public static class ConsoleHelper
{
  
  /// <summary>
  /// Displays 'Push a key to exit.' then and waits for a key before continuing.
  /// </summary>  
  public static void PromptPushKeyToExit() => PromptForKey("Push a key to exit.");

  /// <summary>
  /// Displays <paramref name="message"/> and 'Push a key to exit.' then and waits for a key before continuing.
  /// </summary>  
  public static void PromptPushKeyToExit(string message) => PromptForKey(message + "\nPush a key to exit.");


  /// <summary>
  /// Displays prompt message and returns response key as <see cref="char"/>.
  /// </summary>
  public static char PromptForKey(string prompt)
  {
    Write(prompt);
    return ReadKey(true).KeyChar;
  }

  /// <summary>
  /// Displays prompt message and returns response text as <see cref="string"/>.
  /// </summary>
  public static string? PromptForInput(string prompt!!)
  {
    Write(prompt.EnsureEndsWithSpace());
    return ReadLine();
  }

  /// <summary>
  /// Allows user to enter password without displaying it to console.
  /// </summary>
  /// <param name="prompt">Text prompt to display, or null for no prompt.</param>
  /// <param name="showStars">Show * for each character.</param>
  /// <returns></returns>
  public static string ReadPassword(string? prompt = "Enter password: ", bool showStars = true)
  {
    if (prompt != null) Write(prompt.EnsureEndsWithSpace());
    return ReadLineHidden(showStars);
  }

  /// <summary>
  /// Reads a line and either shows nothing or * chars in the console.
  /// </summary>
  /// <param name="showStars">Show * for each char or nothing.</param>
  /// <returns></returns>
  public static string ReadLineHidden(bool showStars)
  {
    string? input = string.Empty;
    while (true)
    {
      var key = ReadKey(true);
      if (key.Key != ConsoleKey.Enter)
      {
        if (key.Key != ConsoleKey.Backspace)
        {
          input += key.KeyChar;
          if (showStars) Write('*');
        }
        else
        {
          input = input.RemoveLastCharacter();
          DeleteLastChar();
        }
      }
      else break;
    };
    WriteLine();
    return input ?? string.Empty;
  }

  /// <summary>
  /// Deletes the last character written by <see cref="Write(char)"/>
  /// </summary>
  public static void DeleteLastChar() => Write("\b \b");


  /// <summary>
  /// Writes a string to the console followed by a line of hyphens of the same length.
  /// </summary>    
  public static void WriteLineUnderlined(string value)
  {
    Console.WriteLine(value);
    Console.WriteLine(new string('-', value.Length));
  }

  /// <summary>
  /// Writes the specified number of new lines to the console.
  /// </summary>
  public static void WriteNewLines(int count)
  {
    if (count > 0) Write(new string('\n', count));
  }

  /// <summary>
  /// Writes a string to the console, optional preceded and/or followed by a number of blank lines.
  /// </summary>
  public static void WriteParagraph(string text, int linesAfter = 1, int linesBefore = 0)
  {
    WriteNewLines(linesBefore);
    Console.WriteLine(text);
    WriteNewLines(linesAfter);
  }
}

