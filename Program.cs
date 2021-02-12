using System;
using CommandLine;
using System.IO;
using System.Collections.Generic;

namespace Dictionate
{
    class Program
    {
        public class Options
        {
            [Option('s', "source", Required = true, HelpText = "The input filename of the source list of words.")]
            public string Source { get; set; }

            [Option('c', "capitalize", Required = false, HelpText = "Capitalize the words of the source list in the dictionary.")]
            public bool Capitalize { get; set; }

            [Option('a', "all-caps", Required = false, HelpText = "Put the words of the source list in all caps in the dictionary.")]
            public bool AllCaps { get; set; }

            [Option('x', "silent", Required = false, HelpText = "Disable verbose console output.")]
            public bool Silent { get; set; }
        }

        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args).MapResult(options => Run(options),
            _ => 1);
        }

        static int Run(Options options)
        {
            // Create a list for storing the source words
            List<string> source = new List<string>();

            // Trim the supplied source file
            options.Source = options.Source.Trim();

            // Load the source list from the specified file
            if(File.Exists(options.Source)) {
                if(options.Silent != true) Console.WriteLine("Reading source words from '{0}'...", options.Source);

                source.AddRange(File.ReadAllLines(options.Source));

                if(options.Silent != true) Console.WriteLine("Found {0} source words...", source.Count);
            } else {
                Console.WriteLine("File '{0}' is not a valid file/path!", options.Source);
                return -1;
            }

            return 0;
        }
    }
}