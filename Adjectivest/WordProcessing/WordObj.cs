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
            SyllableCount = GetSyllableCount(phonemes);
            SetPhonemeTypes(phonemes);
        }

        public string Word { get; private set; }
        public List<Phoneme> Phonemes { get; private set; }
        public int SyllableCount { get; private set; }
        public PhonemeType FirstSyllableType { get; private set; }
        public PhonemeType LastSyllableType { get; private set; }

        private int GetSyllableCount(List<Phoneme> phonemes)
        {
            int value = 0;

            if(phonemes != null)
            {
                foreach (var p in phonemes)
                {
                    if (p is VowelPhoneme)
                    {
                        value++;
                    }
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


        private void SetPhonemeTypes(List<Phoneme> phonemes)
        {
            if(phonemes != null)
            {
                FirstSyllableType = Phonemes[0].PhonemeType;
                var last = GetLastPhoneme();
                LastSyllableType = GetLastPhoneme().PhonemeType;
            }
        }

    }
}
