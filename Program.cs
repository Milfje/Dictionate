using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using ByteSizeLib;
using CommandLine;

namespace Dictionate
{
    class Program
    {
        public class Options
        {
            [Option('s', "source", Required = true, HelpText = "The input path/file of the source list of words.")]
            public string Source { get; set; }

            [Option('o', "output", Default = "output.txt", Required = false, HelpText = "Determines the output path/file of the generated list.")]
            public string Output { get; set; }

            [Option('d', "depth", Default = 2, Required = false, HelpText = "Determines the depth (amount of combinations with other words).")]
            public int Depth { get; set; }

            [Option('c', "capitalize", Required = false, HelpText = "Capitalize the words of the source list in the dictionary.")]
            public bool Capitalize { get; set; }

            [Option('a', "all-caps", Required = false, HelpText = "Put the words of the source list in all caps in the dictionary.")]
            public bool AllCaps { get; set; }

            [Option('r', "repetition", Default = false, Required = false, HelpText = "Determines if repetition is allowed in combinations.")]
            public bool Repetition { get; set; }

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

            // If capitalization is set add captials to the source list
            if(options.Capitalize) {
                List<string> capitals = new List<string>();
                foreach(var word in source) {
                    capitals.Add(word.First().ToString().ToUpper() + word.Substring(1));
                }
                source.AddRange(capitals);

                if(!options.Silent) Console.WriteLine("Source list expanded to {0} words after capitalization added.", source.Count);

                source = source.Distinct().ToList<string>();
            }

            // If all caps is set, add the new words to the source list
            if(options.AllCaps) {
                List<string> allcaps = new List<string>();
                foreach(var word in source) {
                    allcaps.Add(word.ToUpper());
                }
                source.AddRange(allcaps);

                if(!options.Silent) Console.WriteLine("Source list expanded to {0} words after all caps added.", source.Count);

                source = source.Distinct().ToList<string>();
            }

            // Remove any duplicates
            source = source.Distinct().ToList<string>();
            if(!options.Silent) Console.WriteLine("Source list contains {0} words after duplicate removal.", source.Count);


            // Calculate the combinations
            double combinations = 0;
            for(int i = options.Depth; i >= 0; i--) {
                combinations += Enumerable.Range(source.Count - i, i + 1).ToArray<int>().Aggregate(1, (a, b) => a * b);
            }
            if(!options.Silent) Console.WriteLine("Source file will generate {0} combinations{1}", combinations, options.Repetition ? " (excluding repetition!)" : ".");

            // Verify with the user if generation is still desired
            string result = string.Empty;
            do {
                Console.Write("\nContinue generating list? (y/n) ");
                result = Console.ReadLine().Trim();
            } while (result != "y" && result != "n");

            if(result == "n") {
                return 0;
            }

            // Make the combinations
            if(!options.Silent) Console.WriteLine("\nGenerating the list of combinations...");
            List<string> output = new List<string>();
            for(int i = 0; i <= options.Depth; i++){
                foreach(var list in GetCombinations(source, i + 1, options.Repetition)) {
                    output.Add(string.Join("", list.ToArray()));
                }
            }

            // Write the output to the specified output file
            if(!options.Silent) Console.WriteLine("Writing to output file ({0})...", options.Output);

            File.WriteAllLines(options.Output, new List<string>(output));

            return 0;
        }

        static double Factorial(int n)
        {
            int res = 1;

            while (n != 1) {
                res = res * n;
                n = n -1;
            }

            return res;
        }

        // Credits to 'Pengyang' at Stackoverflow
        static IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> list, int depth, bool repetition)
        {
            if(depth == 1 ) return list.Select(t => new T[] { t });

            if(!repetition) {
                return GetCombinations(list, depth - 1, false)
                    .SelectMany(t => list.Where(e => !t.Contains(e)),
                        (t1, t2) => t1.Concat(new T[] { t2 }));
            } else {
                return GetCombinations(list, depth - 1, true)
                    .SelectMany(t => list,
                        (t1, t2) => t1.Concat(new T[] { t2 }));
            }
        }
    }
}