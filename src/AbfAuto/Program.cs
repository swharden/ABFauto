﻿namespace AbfAuto;

/// <summary>
/// Code in this file runs when the application is called from the command line.
/// It analyzes a file (ABF or TIF) given as a command line argument.
/// If a folder is given, it will analyze all files in that folder.
/// </summary>
public static class Program
{
    public static void Main(string[] args)
    {
        if (System.Diagnostics.Debugger.IsAttached)
        {
            string testFilePath = @"X:\Data\zProjects\Oxytocin Biosensor\experiments\ChR2 stimulation\2024-08-15 ephys\2024_08_15_0002.abf";
            args = [testFilePath];
        }

        if (args.Length != 1)
            throw new ArgumentException("Expected a single argument (path to an ABF file)");

        string path = Path.GetFullPath(args[0]);
        if (!File.Exists(path))
            throw new FileNotFoundException(path);

        if (path.EndsWith(".abf", StringComparison.InvariantCultureIgnoreCase))
        {
            string[] savedFiles = Analyze.AbfFile(path);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(string.Join("\n", savedFiles));
        }
        else if (path.EndsWith(".tif", StringComparison.InvariantCultureIgnoreCase))
        {
            string saved = TifFile.AutoAnalyze(path);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(saved);
        }
        else
        {
            throw new ArgumentException($"unsupported file type: {Path.GetFileName(path)}");
        }

        Console.ForegroundColor = ConsoleColor.Gray;
    }
}