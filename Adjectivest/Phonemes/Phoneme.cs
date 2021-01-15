using System;
using System.Collections.Generic;
using System.Text;

namespace Adjectivest.Phonemes
{
    
    public abstract class Phoneme
    {
        public string Symbol { get; protected set; }
        public string PronunciationExample { get; protected set; }
        public PhonemeType PhonemeType { get; protected set; }

        public override string ToString()
        {
            return Symbol + " | " + PronunciationExample;
        }

    }
}
