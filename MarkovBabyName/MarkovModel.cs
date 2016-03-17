using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovBabyName
{
    public class MarkovModel
    {
        public double[,,] MarkovArray { get; set; }
        public List<string> OriginalNames { get; set; }
        public List<string> Names { get; set; }
        public int ALength = 27;
        public int minLength { get; set; }
        public int maxLength { get; set; }
        public int numberOfNamesRequested { get; set; }
        public int genderSelection { get; set;  }
        public MarkovModel(int min, int max, int numberRequested, int gender)
        {
            MarkovArray = new double[ALength, ALength, ALength];
            OriginalNames = new List<string>();
            Names = new List<string>();
            minLength = min;
            maxLength = max;
            numberOfNamesRequested = numberRequested;
            genderSelection = gender;
        }


        //Count letter distribution
        public void CountDistribution()
        {
            //System.Console.WriteLine("----------Counting Distribution----------");
            CharIndexConverter cic = new CharIndexConverter();

            foreach (var name in Names)
            {
                for (var i = 0; i < name.Length - 2; i++)
                {
                    var firstLetter = cic.IndexFromChar(name[i]);
                    var secondLetter = cic.IndexFromChar(name[i + 1]);
                    var thirdLetter = cic.IndexFromChar(name[i + 2]);

                    MarkovArray[firstLetter, secondLetter, thirdLetter] =
                        MarkovArray[firstLetter, secondLetter, thirdLetter] + 1;
                }
            }
            //System.Console.WriteLine("----------End Counting Distribution----------");
        }

        public void Normalize()
        {
            //System.Console.WriteLine("----------Normalizing----------");
            //Normalize array by counting up the row, then dividing each value by rowCount
            for (var i = 0; i < ALength; i++)
            {
                for (var j = 0; j < ALength; j++)
                {
                    var rowCount = 0.0;
                    for (var k = 0; k < ALength; k++)
                    {
                        rowCount += MarkovArray[i, j, k];
                    }

                    if (rowCount > 0)
                    {
                        for (var l = 0; l < ALength; l++)
                        {
                            MarkovArray[i, j, l] = MarkovArray[i, j, l] / rowCount;
                        }
                    }
                }
            }
            //System.Console.WriteLine("----------End Normalizing----------");
        }

        public char GenerateLetter(char firstLetter, char secondLetter)
        {

            CharIndexConverter cic = new CharIndexConverter();
            var firstIndex = cic.IndexFromChar(firstLetter);
            var secondIndex = cic.IndexFromChar(secondLetter);
            //Generate Random letter sequence
            Random rnd = new Random();
            int randNumber = rnd.Next(0, 10000);
            //Divide by 10k to normalize between 0-1, some percents might be low.
            double valueToCompare = randNumber / 10000.0;
            //compare it to values in MarkovArray
            for (var k = 0; k < ALength; k++)
            {
                valueToCompare = valueToCompare - MarkovArray[firstIndex, secondIndex, k];
                //return if value in MarkovArray > randomly generated
                if (valueToCompare <= 0)
                {
                    //this will ensure that a random number is generated and
                    //maintain the frequency of the letter.
                    return cic.CharFromIndex(k);
                }
            }

            return '_'; // bad random number
        }

        public string GenerateName()
        {
            StringBuilder name = new StringBuilder();
            //we use __ to find initial letter.
            name.Append("__");
            var firstLetter = name[0];
            var secondLetter = name[1];
            char letter = GenerateLetter(firstLetter, secondLetter);
            while (letter != '_')
            {
                name.Append(letter);
                letter = GenerateLetter(name[name.Length - 2], name[name.Length - 1]);
            }

            //Capitalize and return name
            name[2] = char.ToUpper(name[2]);

            return name.ToString().Substring(2); ;
        }

        public bool NameExists(string nameToSearch)
        {
            return OriginalNames.Any(name => name == nameToSearch);
        }

        public bool CorrectLength(string name)
        {
            if (name.Length > minLength && name.Length < maxLength)
                return true;
            else
                return false;
        }
    }
}
