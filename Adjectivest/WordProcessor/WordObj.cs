using Adjectivest.Phonemes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adjectivest.WordProcessor
{
    public class WordObj
    {
        public WordObj(string wordContent, List<Phoneme> phonemes)
        {
            Word = wordContent;
            Phonemes = phonemes;
            SyllableCount = GetSyllableCount();
            SetPhonemeTypes();
        }

        public string Word { get; private set; }
        public List<Phoneme> Phonemes { get; private set; }
        public int SyllableCount { get; private set; }
        public PhonemeType FirstSyllableType { get; private set; }
        public PhonemeType LastSyllableType { get; private set; }

        private int GetSyllableCount()
        {
            int value = 0;

            foreach(var p in Phonemes)
            {
                if(p is VowelPhoneme)
                {
                    value++;
                }
            }

            return value;
        }

        //Phoneme Helpers

        public Phoneme GetLastPhoneme()
        {
            return Phonemes[Phonemes.Count -1];
        }

        public Phoneme GetPenultimatePhoneme()
        {
            return Phonemes[Phonemes.Count - 2];
        }


        public VowelPhoneme GetFirstVowel()
        {
            for (int i = 0; i < Phonemes.Count; i++)
            {
                if(Phonemes[i] is VowelPhoneme v)
                {
                    return v;
                }
            }

            return null;
        }


        private void SetPhonemeTypes()
        {
            FirstSyllableType = Phonemes[0].PhonemeType;
            LastSyllableType = GetLastPhoneme().PhonemeType;
        }

    }
}
