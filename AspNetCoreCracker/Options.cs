using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace AspNetCoreCracker
{
    internal class Options
    {
        [Option('h', "hashes", Required = true,
            HelpText = "The path to a file with a list of hashes to crack.")]
        public string HashFile { get; set; }

        [Option('p', "passwords", Required = true,
            HelpText = "The path to a file with a list of passwords to hash.")]
        public string PasswordFile { get; set; }


        [Option('t', "thread", Required = false,
            HelpText = "The count of threads", Default = "50")]
        public string ThreadCount { get; set; }

        [Usage(ApplicationAlias = "AspNetCoreCracker")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Attempt to crack a list of hashes",
                    new Options {HashFile = "hashes.txt", PasswordFile = "passwords.txt"});
            }
        }
    }
}