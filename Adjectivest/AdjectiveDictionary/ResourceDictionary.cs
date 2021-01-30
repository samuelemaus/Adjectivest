using Adjectivest.Phonemes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Adjectivest.Core.AdjectiveDictionary
{
    public abstract class ResourceDictionary : IDisposable
    {
        protected const string resourcesFolder = "AdjectivestResources";
        protected const string cmuDictFileName = "cmudict-0.7b.txt";
        protected const string dictLetterIndicesFileName = "dictLetterIndices.txt";
        protected const string adjectivesListFileName = "adjectivesList.txt";

        protected Dictionary<char, int> dictionaryLetterIndices = new Dictionary<char, int>();
        protected string dictionaryPath = Path.Combine(resourcesFolder, cmuDictFileName);

        protected Dictionary<char, int> adjectivesListLetterIndices = new Dictionary<char, int>();
        protected string adjectivesListPath = Path.Combine(resourcesFolder, adjectivesListFileName);
        protected bool disposedValue;

        protected PhonemeCollection PhonemeCollection { get; set; }


        public List<Phoneme> GetPhonemesFromWord(string word)
        {
            List<Phoneme> phonemes = new List<Phoneme>();

            char upperFirst = Char.ToUpper(word[0]);
            string wordUpper = word.ToUpper();

            int startIndex = dictionaryLetterIndices[upperFirst];

            int maxIndex = (upperFirst != 'Z') ? dictionaryLetterIndices[(char)(upperFirst + 1)] : 133904;

            string line = GetDictLine(wordUpper, startIndex, maxIndex);
            if(line == null)
            {
                return null;
            }

            phonemes = GetPhonemesFromLine(word, line);
            return phonemes;

        }

        public abstract bool DictContainsWord(string word);
        public abstract bool AdjectivesListContainsWord(string word);

        protected abstract string GetDictLine(string word, int startIndex, int maxIndex);

        protected static Dictionary<char,int> GetDictionaryLetterIndices()
        {
            const char delimiter = ',';
            var keyValuePairs = new Dictionary<char, int>();

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
        protected List<Phoneme> GetPhonemesFromLine(string word, string line)
        {
            const char spaceSeparator = ' ';

            List<Phoneme> phonemes = new List<Phoneme>();

            string phonemeSubString = line.Substring(word.Length).Trim();
            string[] phonemesRaw = phonemeSubString.Split(spaceSeparator);

            for (int j = 0; j < phonemesRaw.Length; j++)
            {
                string phoneme = Regex.Replace(phonemesRaw[j], @"[\d-]", string.Empty);
                phonemes.Add(PhonemeCollection.GetPhoneme(phoneme));
            }

            return phonemes;
        }

        protected bool LineIdentifiedAsWord(string word, string line)
        {
            const char spaceSeparator = ' ';
            return line.StartsWith(word) && line.Split(spaceSeparator)[0] == word;
        }

        protected abstract Dictionary<char, int> GetAdjectivesListLetterIndices();

        protected abstract void Dispose(bool disposing);

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ResourceDictionary()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
