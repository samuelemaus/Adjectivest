using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Adjectivest.Phonemes
{
    public class PhonemeCollection : IDisposable
    {
        public PhonemeCollection()
        {
            Initialize();
        }

        private const string vowelsFileName = "vowels.txt";
        private const string consonantsFileName = "consonants.txt";
        private const string resourcesFolder = "AdjectivestResources";
        private const string phoneticVowels = "aeiou";
        private const char commaDelimiter = ',';

        private List<VowelPhoneme> Vowels = new List<VowelPhoneme>();
        private List<ConsonantPhoneme> ConsonantPhonemes = new List<ConsonantPhoneme>();
        private Dictionary<string, Phoneme> phonemeIndex = new Dictionary<string, Phoneme>();
        private bool disposedValue;

        public Phoneme GetPhoneme(string symbol)
        {
            return phonemeIndex[symbol];
        }

        private void Initialize()
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                Vowels = null;
                ConsonantPhonemes = null;
                phonemeIndex = null;
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~PhonemeCollection()
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
