using System;
using System.Collections.Generic;
using System.Text;

namespace Adjectivest.Phonemes
{
    public sealed class ConsonantPhoneme : Phoneme
    {
        public ConsonantPhoneme(string symbol, ConsonantFormation formation, bool doubleOnShort, string example)
        {
            Symbol = symbol;
            Formation = formation;
            DoubleOnShortVowel = doubleOnShort;
            PronunciationExample = example;

        }

        public ConsonantFormation Formation { get; private set; }
        public bool DoubleOnShortVowel { get; private set; }

        public override string ToString()
        {
            return base.ToString() + " | " + Formation + " | DoubleOnShort: " + DoubleOnShortVowel;
        }

    }
}
