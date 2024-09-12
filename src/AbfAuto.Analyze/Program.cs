﻿if (System.Diagnostics.Debugger.IsAttached)
    args = [@"X:\Data\Alchem\Donepezil\BLA\07-28-2021\2021_07_28_DIC1_0000.tif"];

if (args.Length != 1)
    throw new ArgumentException("Expected a single argument (path to an ABF file)");

string path = Path.GetFullPath(args[0]);
if (!File.Exists(path))
    throw new FileNotFoundException(path);

if (path.EndsWith(".abf", StringComparison.InvariantCultureIgnoreCase))
{
    string[] savedFiles = AbfAuto.Core.Analyze.AnalyzeAbfFile(path);
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine(string.Join("\n", savedFiles));
}
else if (path.EndsWith(".tif", StringComparison.InvariantCultureIgnoreCase))
{
    string saved = AbfAuto.Core.TifFile.AutoAnalyze(path);
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine(saved);
}
else
{
    throw new ArgumentException($"unsupported file type: {Path.GetFileName(path)}");
}

Console.ForegroundColor = ConsoleColor.Gray;