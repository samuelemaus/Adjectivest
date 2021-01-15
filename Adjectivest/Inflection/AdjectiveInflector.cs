using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Adjectivest.WordProcessor;
using Adjectivest.Phonemes;

namespace Adjectivest.Inflection
{
    public enum InflectionType { MoreMost, ErEst }
    public enum ComparisonType {None, Comparative, Superlative }

    public class AdjectiveInflector
    {
        private const string irregularAdjectivesFileName = "Resources/irregularAdjectives.txt";
        private const char commaDelimiter = ',';
        public const string leSuffix = "le";
        public const string erSuffix = "er";
        public const string ySuffix = "y";
        private const string basicComparative = "er";
        private const string basicSuperlative = "est";

        private const string multiSyllabicComparative = "more ";
        private const string multiSyllabicSuperlative = "most ";

        //exception rules
        private const string eSuffix = "e";


        public static string[] irregularAdjectives;

        public AdjectiveInflector()
        {
            Initialize();
        }

        public void Initialize()
        {
            irregularAdjectives = File.ReadAllLines(irregularAdjectivesFileName);
            PhonemeCollection.Initialize();
            CMUDictionary.InitializeDictionary();
        }

        public string GetAdjectiveInflections(string word)
        {
            string comparative = "";
            string superlative = "";

            if(IsKnownIrregular(word, out comparative, out superlative))
            {
                return Format(word, comparative, superlative);
            }
            
            WordObj wordObj = null;

            var phonemes = CMUDictionary.GetPhonemesFromWord(word);

            if(phonemes != null)
            {
                wordObj = new WordObj(word, phonemes);
                var inflectionType = GetInflectionType(wordObj);

                comparative = GetComparative(wordObj, inflectionType);
                superlative = GetSuperlative(wordObj, inflectionType);

                
            }

            return Format(word, comparative, superlative);
        }

        private string Format(string word, string comparative, string superlative)
        {
            return $"{word}|{comparative}|{superlative}";
        }

        public static bool IsKnownIrregular(string word, out string comparative, out string superlative)
        {
            comparative = null;
            superlative = null;

            for (int i = 0; i < irregularAdjectives.Length; i++)
            {
                string irregular = irregularAdjectives[i];
                if (irregular.StartsWith(word))
                {
                    string[] splitIrregular = irregular.Split(commaDelimiter);
                    comparative = splitIrregular[1];
                    superlative = splitIrregular[2];
                    return true;
                }

            }

            return false;
        }

        public InflectionType GetInflectionType(WordObj word)
        {
            string wordText = word.Word;
            int syllableCount = word.SyllableCount;

            if (syllableCount < 2 || wordText.EndsWith(ySuffix) || wordText.EndsWith(leSuffix))
            {
                return InflectionType.ErEst;
            }

            else
            {
                return InflectionType.MoreMost;
            }

        }

        public string GetComparative(WordObj word, InflectionType inflectionType)
        {
            return inflectionType switch
            {
                InflectionType.MoreMost => GetMultiSyllabicComparative(word, ComparisonType.Comparative),
                InflectionType.ErEst => GetBasicComparative(word, ComparisonType.Comparative)
            };
        }

        public string GetSuperlative(WordObj word, InflectionType inflectionType)
        {
            return inflectionType switch
            {
                InflectionType.MoreMost => GetMultiSyllabicComparative(word, ComparisonType.Superlative),
                InflectionType.ErEst => GetBasicComparative(word, ComparisonType.Superlative)
            };
        }


        private string GetBasicComparative(WordObj wordObj, ComparisonType comparisonType)
        {
            string finalValue = "";
            string word = wordObj.Word;
            string suffix = comparisonType switch
            {
                ComparisonType.Comparative => basicComparative,
                ComparisonType.Superlative => basicSuperlative,
            };


            if (word.EndsWith(eSuffix))
            {
                suffix = suffix.Substring(1);
                finalValue = word + suffix;
                return finalValue;
            }

            if (word.EndsWith(AdjectiveInflector.ySuffix) && wordObj.SyllableCount > 1)
            {
                finalValue = word.Substring(0, word.Length - 1);
                finalValue += "i" + suffix;
                return finalValue;
            }

            //Should be only vowel by this point?
            VowelPhoneme firstVowel = wordObj.GetFirstVowel();


            bool endsInConsonant = wordObj.LastSyllableType == PhonemeType.Consonant;


            if (endsInConsonant)
            {
                ConsonantPhoneme lastConsonant = (ConsonantPhoneme)wordObj.GetLastPhoneme();

                switch (firstVowel.Length)
                {
                    case VowelLength.Short:

                        bool endsInNonDigraphicDualConsonant = wordObj.GetPenultimatePhoneme() is ConsonantPhoneme cp;

                        if (lastConsonant.DoubleOnShortVowel && !endsInNonDigraphicDualConsonant)
                        {
                            char last = word[word.Length - 1];
                            finalValue += word + last;
                            finalValue += suffix;
                        }

                        else
                        {
                            finalValue = word + suffix;
                        }

                        return finalValue;


                    case VowelLength.Long:
                        return finalValue + suffix;


                }
            }

            else
            {
                if (wordObj.SyllableCount == 1)
                {
                    return finalValue + suffix;
                }
            }




            return finalValue;
        }

        private string GetMultiSyllabicComparative(WordObj wordObj, ComparisonType comparisonType)
        {
            string finalValue = wordObj.Word;

            string prefix = comparisonType switch
            {
                ComparisonType.Comparative => multiSyllabicComparative,
                ComparisonType.Superlative => multiSyllabicSuperlative,
                _ => throw new NotImplementedException()
            };

            finalValue = prefix + finalValue;

            return finalValue;
        }

    }
}
