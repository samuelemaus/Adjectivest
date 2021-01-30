using Adjectivest.Phonemes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Adjectivest.Core.AdjectiveDictionary
{
    public class BasicResourceDictionary : ResourceDictionary
    {
        public BasicResourceDictionary()
        {
            dictionaryLetterIndices = GetDictionaryLetterIndices();
            adjectivesListLetterIndices = GetAdjectivesListLetterIndices();
            PhonemeCollection = new PhonemeCollection();
        }

        public override bool AdjectivesListContainsWord(string word)
        {
            char first = word[0];
            int startIndex = adjectivesListLetterIndices[first];
            int maxIndex = adjectivesListLetterIndices[(char)(first + 1)];

            using (StreamReader streamReader = new StreamReader(adjectivesListPath))
            {
                for (int i = 0; i < maxIndex; i++)
                {
                    string adj = streamReader.ReadLine();

                    if (adj.Equals(word))
                    {
                        return true;
                    }
                }
            }

            return false;

        }

        public override bool DictContainsWord(string word)
        {
            char upperFirst = Char.ToUpper(word[0]);
            int maxIndex = (upperFirst != 'Z') ? dictionaryLetterIndices[(char)(upperFirst + 1)] : 133904;

            return GetDictLine(word, 0, maxIndex) != null;
        }

        protected override string GetDictLine(string word, int startIndex, int maxIndex)
        {
            using (StreamReader streamReader = new StreamReader(dictionaryPath))
            {
                for (int i = 0; i < maxIndex; i++)
                {
                    string line = streamReader.ReadLine();
                    if (LineIdentifiedAsWord(word, line))
                    {
                        return line;
                    }
                }
                
            }

            return null;
        }

        protected override Dictionary<char, int> GetAdjectivesListLetterIndices()
        {
            const char delimiter = ',';
            var returnDict = new Dictionary<char, int>();

            using (StreamReader streamReader = new StreamReader(adjectivesListPath))
            {
                bool readingIndices = false;

                for (int i = 0; i < 29; i++)
                {
                    string line = streamReader.ReadLine();

                    if (line.StartsWith("a,"))
                    {
                        readingIndices = true;
                    }

                    if (readingIndices)
                    {
                        var split = line.Split(delimiter);
                        char character = Char.Parse(split[0]);
                        int index = Int32.Parse(split[1]);

                        returnDict.Add(character, index);
                    }


                }
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
                GC.Collect();
            }
        }
    }
}
