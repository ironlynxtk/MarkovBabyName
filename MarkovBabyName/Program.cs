using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovBabyName
{
    class Program
    {
        public static MarkovModel ProgramSetup()
        {
            Console.WriteLine("Enter the minimum name length");
            var min = Int32.Parse(Console.ReadLine()); //name min length

            Console.WriteLine("Enter the maximum name length");
            var max = Int32.Parse(Console.ReadLine()); //name max length

            Console.WriteLine("How many names do you want?");
            var numberOfNames = Int32.Parse(Console.ReadLine()); //number of names requested

            Console.WriteLine("Boys (1) or Girls (2) Names?");
            var genderSelection = Int32.Parse(Console.ReadLine()); //gender

            MarkovModel markovModel = new MarkovModel(min, max, numberOfNames, genderSelection);
            Console.WriteLine("\n");

            return markovModel;
        }

        static void Main(string[] args)
        {
            var markovModel = ProgramSetup();

            List<string> Names = new List<string>();
            System.IO.StreamReader file;
            string path = string.Empty;

            if (markovModel.genderSelection == 1)
            {
                path = Path.Combine(Environment.CurrentDirectory, "namesBoys.txt");
            }
            else
            {
                path = Path.Combine(Environment.CurrentDirectory, "namesGirls.txt");
            }

            // Read the file and display it line by line.
            file = new System.IO.StreamReader(path);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                markovModel.OriginalNames.Add(line);
                var name = "__" + line + "__";
                markovModel.Names.Add(name);
            }
            file.Close();

            //Count distributions of second order markov chains
            markovModel.CountDistribution();

            //Normalize distribution between 0 -1
            markovModel.Normalize();
            var totalNames = 0;
            while (totalNames != markovModel.numberOfNamesRequested)
            {
                var name = markovModel.GenerateName();
                var PreviouslyGeneratedName = Names.Any(record => record == name);

                //Make sure name doesn't exist in data set, is correct length and
                //Make sure we don't add a previously generated name too.
                if (!markovModel.NameExists(name) &&
                    markovModel.CorrectLength(name) &&
                    !PreviouslyGeneratedName)
                {
                    Names.Add(name);
                    totalNames++;
                }
            }
            Console.WriteLine("\n");
            Console.WriteLine("----------New Name List----------");
            foreach (var name in Names)
            {
                System.Console.WriteLine(name);
            }

            Console.WriteLine("----------Exiting in 10 seconds----------");
            System.Threading.Thread.Sleep(10000);
    }
    }
}
