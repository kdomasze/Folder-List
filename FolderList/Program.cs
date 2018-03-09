using System;
using System.IO;
using System.Linq;

namespace ListFilesInFolder
{
    /// <summary>
    /// Simple commandline program to list all files and folders in a directory and output them to a file.
    /// </summary>
    class Program
    {
        #region Flags
        private static bool _fullPath = false;
        private static bool _extension = false;
        private static bool _print = false;
        private static bool _write = true;
        private static bool _files = true;
        private static bool _folders = true;
        private static string _path = Directory.GetCurrentDirectory();
        private static string _output = "FolderList.txt";
        #endregion

        static void Main(string[] args)
        {
            #region Initialize
            ParseArgs(args);

            if (!Directory.Exists(_path))
            {
                Console.WriteLine($"The path \"{_path}\" was not found.");
                return;
            }

            var files = Directory.GetFiles(_path).ToList();
            var folders = Directory.GetDirectories(_path).ToList();

            StreamWriter writer = null;

            if(_write)
                writer = new StreamWriter(Path.Combine(_path, _output));
            #endregion

            #region Print Files
            if (_files)
            {
                PrintLine(writer, "Files:");
                foreach (var file in files)
                {
                    var output = file;

                    if (!_fullPath)
                    {
                        if (_extension)
                            output = Path.GetFileName(output);
                        else
                            output = Path.GetFileNameWithoutExtension(output);
                    }

                    PrintLine(writer, output);
                }
                PrintLine(writer, "");
            }
            #endregion

            #region Print Folders
            if (_folders)
            {
                PrintLine(writer, "Folders:");
                foreach (var folder in folders)
                {
                    var output = folder;

                    if (!_fullPath)
                        output = new DirectoryInfo(output).Name;

                    PrintLine(writer, output);
                }
            }
            #endregion

            #region Terminate
            if (_write)
                writer.Close();
            #endregion
        }

        /// <summary>
        /// Handles printing text to the console and file
        /// </summary>
        /// <param name="writer">Streamwriter that handles printing to file</param>
        /// <param name="line">String to be printed</param>
        private static void PrintLine(StreamWriter writer, string line)
        {
            if(_write)
                writer.WriteLine(line);

            if (_print)
                Console.WriteLine(line);
        }

        /// <summary>
        /// Parses the commandline arguments.
        /// </summary>
        /// <param name="args">The list of commandline arguments</param>
        private static void ParseArgs(string[] args)
        {
            if (args.Length != 0 && !args[0].Contains('-')) _path = args[0];

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "--nofolders":
                        _folders = false;
                        break;
                    case "--nofiles":
                        _files = false;
                        break;
                    case "-fp":
                    case "--fullpath":
                        _fullPath = true;
                        break;
                    case "-e":
                    case "--extension":
                        _extension = true;
                        break;
                    case "-nw":
                    case "--nowrite":
                        _write = false;
                        _print = true;
                        break;
                    case "-o":
                    case "--output":
                        _output = args[i + 1];
                        break;
                    case "-p":
                    case "--print":
                        _print = true;
                        break;
                    case "-h":
                    case "--help":
                        PrintHelp();
                        return;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Prints the help text to console.
        /// </summary>
        private static void PrintHelp()
        {
            Console.WriteLine("Folder List 1.0, a simple file and folder lister.");
            Console.WriteLine("Usage: folderlist [OPTION]...");
            Console.WriteLine("       folderlist [PATH] [OPTION]...");
            Console.WriteLine("If no path is specified, the current working directory will be listed.");
            Console.WriteLine("Options:");
            Console.WriteLine("\t--noFiles\t\tskips printing the files.");
            Console.WriteLine("\t--noFolders\t\tskips printing the folders.");
            Console.WriteLine("\t-fp, --fullPath\t\tprints the full path and extension of all files and folders.");
            Console.WriteLine("\t-e, --extension\t\tprints the extension of all files if fullpath isn't specified.");
            Console.WriteLine("\t-nw, --noWrite\t\tskips writing to file. Will force printing.");
            Console.WriteLine("\t-o, --output\t\tspecifies a custom filename for the output list (default: FolderList.txt).");
            Console.WriteLine("\t-p, --print\t\tprints to console the output along with writing the file.");
            Console.WriteLine("\t-h, --help\t\tdisplayed this help page.");
        }
    }
}
