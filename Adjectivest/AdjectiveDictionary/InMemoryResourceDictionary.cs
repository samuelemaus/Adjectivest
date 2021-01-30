using Adjectivest.Core.AdjectiveDictionary;
using Adjectivest.Phonemes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Adjectivest
{
    public class InMemoryResourceDictionary : ResourceDictionary
    {

        private string[] cmuDictLines;
        private string[] adjectivesLines;

        public InMemoryResourceDictionary()
        {
            cmuDictLines = File.ReadAllLines(dictionaryPath);
            dictionaryLetterIndices = GetDictionaryLetterIndices();
            adjectivesLines = File.ReadAllLines(adjectivesListPath);
            adjectivesListLetterIndices = GetAdjectivesListLetterIndices();
            PhonemeCollection = new PhonemeCollection();
        }

        public override bool AdjectivesListContainsWord(string word)
        {

            char first = word[0];
            int startIndex = adjectivesListLetterIndices[first];
            int maxIndex = adjectivesListLetterIndices[(char)(first + 1)];

            for (int i = startIndex; i <maxIndex; i++)
            {
                if (adjectivesLines[i].Equals(word))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool DictContainsWord(string word)
        {
            char upperFirst = Char.ToUpper(word[0]);
            int startIndex = dictionaryLetterIndices[upperFirst];

            int maxIndex = (upperFirst != 'Z') ? dictionaryLetterIndices[(char)(upperFirst + 1)] : cmuDictLines.Length - 1;
            //int startIndex = 0;
            //int maxIndex = cmuDictLines.Length - 1;

            return GetDictLine(word.ToUpper(), startIndex, maxIndex) != null;
        }

        protected override string GetDictLine(string word, int startIndex, int maxIndex)
        {
            for (int i = startIndex; i < maxIndex; i++)
            {
                string line = cmuDictLines[i];
                if (LineIdentifiedAsWord(word,line))
                {
                    return line;
                }
            }

            return null;
        }

        protected override Dictionary<char, int> GetAdjectivesListLetterIndices()
        {
            var returnDict = new Dictionary<char, int>();

            const int indicesStartIndex = 5;
            const char delimiter = ',';

            for (int i = indicesStartIndex; i < (indicesStartIndex + 25); i++)
            {
                var split = adjectivesLines[i].Split(delimiter);
                char character = Char.Parse(split[0]);
                int index = Int32.Parse(split[1]);

                returnDict.Add(character, index);
            }

            return returnDict;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    PhonemeCollection.Dispose();
                    PhonemeCollection = null;
                }

                dictionaryLetterIndices = null;
                adjectivesListLetterIndices = null;
                disposedValue = true;
                cmuDictLines = null;
                adjectivesLines = null;
            }
        }
    }
}
