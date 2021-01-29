using Adjectivest.WordProcessor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Adjectivest
{
    public class UnidentifiedWordBuilder
    {
        private const string vowels = "aeiou";


        public WordObj BuildWordObject(string input)
        {
            return null;
        }


        public static Dictionary<char,int> GetDictionaryIndices()
        {
            var returnDict = new Dictionary<char, int>();
            var lines = File.ReadAllLines("AdjectivestResources/adjectivesList.txt");

            returnDict.Add('a', 0);

            char next = 'b';

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                char lineStart = line[0];

                if(lineStart == next)
                {
                    returnDict.Add(next, i);
                    next = (char)(next + 1);
                }

            }

            string format = "";

            foreach(var entry in returnDict)
            {
                format += entry.Key + "," + entry.Value + "\n";
            }

            return returnDict;

        }

    }
}
