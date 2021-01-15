using System;
using System.Collections.Generic;
using System.Text;

namespace Adjectivest.Phonemes
{
    public sealed class VowelPhoneme : Phoneme
    {
        public VowelPhoneme(string symbol, string example, VowelLength vowelLength)
        {
            this.Symbol = symbol;
            this.PronunciationExample = example;
            this.Length = vowelLength;
        }
        public VowelLength Length { get; private set; }

    }
}
