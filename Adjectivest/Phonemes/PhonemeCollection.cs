using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Adjectivest.Phonemes
{
    public static class PhonemeCollection
    {

        private const string vowelsFileName = "vowels.txt";
        private const string consonantsFileName = "consonants.txt";
        private const string resourcesFolder = "Resources";
        private const string phoneticVowels = "aeiou";
        private const char commaDelimiter = ',';

        private static List<VowelPhoneme> Vowels = new List<VowelPhoneme>();
        private static List<ConsonantPhoneme> ConsonantPhonemes = new List<ConsonantPhoneme>();
        private static Dictionary<string, Phoneme> phonemeIndex = new Dictionary<string, Phoneme>();

        public static Phoneme GetPhoneme(string symbol)
        {
            return phonemeIndex[symbol];
        }

        public static void Initialize()
        {
            string[] vowels = File.ReadAllLines(Path.Combine(resourcesFolder, vowelsFileName));

            for (int i = 0; i < vowels.Length; i++)
            {
                string[] vowelPhonemeRaw = vowels[i].Split(commaDelimiter);

                string symbol = vowelPhonemeRaw[0];
                string length = vowelPhonemeRaw[1];
                string example = vowelPhonemeRaw[2];

                VowelLength vowelLength = (VowelLength)Enum.Parse(typeof(VowelLength), length);

                VowelPhoneme vowelPhoneme = new VowelPhoneme(symbol, example, vowelLength);
                Vowels.Add(vowelPhoneme);
                phonemeIndex.Add(symbol, vowelPhoneme);

            }


            string[] consonants = File.ReadAllLines(Path.Combine(resourcesFolder, consonantsFileName));

            for (int i = 0; i < consonants.Length; i++)
            {
                string[] consonantsPhonemeRaw = consonants[i].Split(commaDelimiter);

                string symbol = consonantsPhonemeRaw[0];
                string doubleRaw = consonantsPhonemeRaw[1];
                string example = consonantsPhonemeRaw[2];
                string formation = consonantsPhonemeRaw[3];

                bool doubleOnShort = Boolean.Parse(doubleRaw);
                ConsonantFormation consonantFormation = (ConsonantFormation)Enum.Parse(typeof(ConsonantFormation), formation);

                ConsonantPhoneme consonantPhoneme = new ConsonantPhoneme(symbol, consonantFormation, doubleOnShort, example);
                ConsonantPhonemes.Add(consonantPhoneme);
                phonemeIndex.Add(symbol, consonantPhoneme);

            }





        }

    


    }
}
