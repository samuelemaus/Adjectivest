using Adjectivest.Phonemes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Adjectivest
{
    public static class CMUDictionary
    {
        private const string resourcesFolder = "Resources";
        private const string cmuDictFileName = "cmudict-0.7b.txt";
        private const string dictLetterIndicesFileName = "dictLetterIndices.txt";

        private static string[] lines;
        private static Dictionary<char, int> dictionaryLetterIndices = new Dictionary<char, int>();
        
        public static void InitializeDictionary()
        {

            string fullPath = Path.Combine(resourcesFolder, cmuDictFileName);


            lines = File.ReadAllLines(fullPath);

            dictionaryLetterIndices = GetDictionaryLetterIndices();
        }

        

        public static List<Phoneme> GetPhonemesFromWord(string word)
        {
            const char spaceSeparator = ' ';
            List<Phoneme> phonemes = new List<Phoneme>();
            
            char upperFirst = Char.ToUpper(word[0]);
            string wordUpper = word.ToUpper();
            
            int startIndex = dictionaryLetterIndices[upperFirst];
            
            int max = dictionaryLetterIndices[(char)(upperFirst + 1)];

            bool wordFound = false;

            for (int i = startIndex; i < max; i++)
            {
                string line = lines[i];
                if (line.StartsWith(wordUpper) && line.Split(spaceSeparator)[0] == wordUpper)
                {
                    wordFound = true;
                    string phonemeSubString = line.Substring(word.Length).Trim();
                    string[] phonemesRaw = phonemeSubString.Split(spaceSeparator);

                    for (int j = 0; j < phonemesRaw.Length; j++)
                    {
                        string phoneme = Regex.Replace(phonemesRaw[j], @"[\d-]", string.Empty);
                        phonemes.Add(PhonemeCollection.GetPhoneme(phoneme));
                    }
                }
            }

            if (!wordFound)
            {
                return null;
            }

            return phonemes;
        }


        private static Dictionary<char,int> GetDictionaryLetterIndices()
        {
            const char delimiter = ',';
            var keyValuePairs = new Dictionary<char,int>();

            string[] lines = File.ReadAllLines(Path.Combine(resourcesFolder, dictLetterIndicesFileName));

            for (int i = 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(delimiter);
                char letter = values[0][0];
                int index = Int32.Parse(values[1]);

                keyValuePairs.Add(letter, index);
            }

            return keyValuePairs;

        }

    }
}
