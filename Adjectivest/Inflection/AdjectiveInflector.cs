using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Adjectivest.WordProcessor;
using Adjectivest.Phonemes;
using Adjectivest.Core.AdjectiveDictionary;
using System.Diagnostics;

namespace Adjectivest
{
    public enum InflectionType { MoreMost, ErEst }
    public enum AdjectiveForm {None, Base, Comparative, Superlative }

    public class AdjectiveInflector : IDisposable
    {
        private const string irregularAdjectivesFileName = "AdjectivestResources/irregularAdjectives.txt";
        private const char commaDelimiter = ',';
        public const string leSuffix = "le";
        public const string erSuffix = "er";
        public const string ySuffix = "y";
        private const string basicComparative = "er";
        private const string basicSuperlative = "est";
        private const string pastParticipleEnding = "ed";

        private const string multiSyllabicComparative = "more";
        private const string multiSyllabicSuperlative = "most";

        //exception rules
        private const string eSuffix = "e";

        public static string[] irregularAdjectives;
        private bool disposedValue;

        public ResourceDictionary ResourceDictionary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadDictionaryIntoMemory">If set to true, the <see cref="InMemoryResourceDictionary"/> loads its data into memory for greater speed. If false, it seeks the file each use.</param>
        public AdjectiveInflector(bool loadDictionaryIntoMemory)
        {
            Initialize(loadDictionaryIntoMemory);
        }

        public void Initialize(bool loadDictionaryIntoMemory)
        {
            irregularAdjectives = File.ReadAllLines(irregularAdjectivesFileName);
            if (loadDictionaryIntoMemory)
            {
                ResourceDictionary = new InMemoryResourceDictionary();
            }

            else
            {
                ResourceDictionary = new BasicResourceDictionary();
            }
        }

        public string GetAdjectiveInflections(string word, AdjectiveForm adjectiveForm = AdjectiveForm.None)
        {
            string baseForm = "";
            string comparative = "";
            string superlative = "";

           

            if(IsKnownIrregular(word, out adjectiveForm, out baseForm, out comparative, out superlative))
            {
                return Format(baseForm, comparative, superlative);
            }
            
            WordObj wordObj = null;

            var phonemes = ResourceDictionary.GetPhonemesFromWord(word);

            wordObj = new WordObj(word, phonemes);

            WordObj baseWordObj = null;

            if (adjectiveForm == AdjectiveForm.None)
            {
                adjectiveForm = InferAdjectiveFormAndBaseForm(wordObj, out baseForm, out baseWordObj);
            }

            WordObj inflectionBase = (baseWordObj == null) ? wordObj : baseWordObj;

            if(inflectionBase.Phonemes == null)
            {
                return word;
            }

            var inflectionType = GetInflectionType(inflectionBase);

            switch (adjectiveForm)
            {
                case AdjectiveForm.Base:
                    baseForm = word;
                    comparative = GetComparative(inflectionBase, inflectionType);
                    superlative = GetSuperlative(inflectionBase, inflectionType);
                    break;
                case AdjectiveForm.Comparative:
                    comparative = word;
                    superlative = GetSuperlative(inflectionBase, inflectionType);
                    break;

                case AdjectiveForm.Superlative:
                    superlative = word;
                    comparative = GetComparative(inflectionBase, inflectionType);
                    break;

            }
            return Format(baseForm, comparative, superlative);
        }

        private string Format(string word, string comparative, string superlative)
        {
            return $"{word}|{comparative}|{superlative}";
        }

        public bool IsKnownIrregular(string word, out string comparative, out string superlative)
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

        public bool IsKnownIrregular(string word, out AdjectiveForm form, out string baseForm, out string comparative, out string superlative)
        {
            baseForm = null;
            comparative = null;
            superlative = null;

            form = AdjectiveForm.None;

            for (int i = 0; i < irregularAdjectives.Length; i++)
            {
                string irregular = irregularAdjectives[i];
                if (irregular.Contains(word))
                {
                    string[] splitIrregular = irregular.Split(commaDelimiter);
                    baseForm = splitIrregular[0];
                    comparative = splitIrregular[1];
                    superlative = splitIrregular[2];

                    for (int j = 0; j < splitIrregular.Length; j++)
                    {
                        if(splitIrregular[j] == word)
                        {
                            form = j switch
                            {
                                0 => AdjectiveForm.Base,
                                1 => AdjectiveForm.Comparative,
                                2 => AdjectiveForm.Superlative,
                                _ => AdjectiveForm.None
                            };
                        }
                    }

                    return true;
                }

            }

            return false;
        }

        public bool IsKnownIrregular(string word)
        {
            string comparative = null;
            string superlative = null;
            return IsKnownIrregular(word, out comparative, out superlative);
        }

        public InflectionType GetInflectionType(WordObj word)
        {
            string wordText = word.Word;
            int syllableCount = word.SyllableCount;

            if ((syllableCount < 2 || wordText.EndsWith(ySuffix) || wordText.EndsWith(leSuffix)) && !IsPastParticiple(word))
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
                InflectionType.MoreMost => GetMultiSyllabicComparative(word, AdjectiveForm.Comparative),
                InflectionType.ErEst => GetBasicComparative(word, AdjectiveForm.Comparative)
            };
        }

        public string GetSuperlative(WordObj word, InflectionType inflectionType)
        {
            return inflectionType switch
            {
                InflectionType.MoreMost => GetMultiSyllabicComparative(word, AdjectiveForm.Superlative),
                InflectionType.ErEst => GetBasicComparative(word, AdjectiveForm.Superlative)
            };
        }


        /// <summary>
        /// Gets the base-form of an adjective.  Input can be any form.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="knownAdjectiveForm">If you already know what form your input adjective is, setting this parameter to the corresponding value will speed up the process and be less error-prone. Leaving it as <see cref="AdjectiveForm.None"/>, the program will attempt to infer its form.</param>
        /// <returns></returns>
        public string GetBaseForm(WordObj wordObj, AdjectiveForm knownAdjectiveForm = AdjectiveForm.None)
        {
            string baseForm = null;

            switch (knownAdjectiveForm)
            {
                case AdjectiveForm.Base:
                    return wordObj.Word;

                case AdjectiveForm.None:

                    WordObj baseFormObj;

                    knownAdjectiveForm = InferAdjectiveFormAndBaseForm(wordObj, out baseForm, out baseFormObj);

                    if(baseForm == null)
                    {
                        return GetBaseForm(wordObj, knownAdjectiveForm);
                    }

                    return baseForm;

                default:

                    if(IsMultiSyllabicComparativeOrSuperlative(wordObj.Word, out knownAdjectiveForm, out baseForm))
                    {
                        return baseForm;
                    }

                    return GetBaseFormFromSimpleComparativeOrSuperlative(wordObj);

            }
        }

        
        private bool IsMultiSyllabicComparativeOrSuperlative(string input, out AdjectiveForm form, out string baseForm)
        {
            const char spaceSeparator = ' ';

            var splitArr = input.Trim().Split(spaceSeparator);
            baseForm = null;

            form = AdjectiveForm.None;

            if(splitArr.Length > 1)
            {

                switch (splitArr[0])
                {
                    case multiSyllabicComparative:
                        form = AdjectiveForm.Comparative;
                        break;
                    case multiSyllabicSuperlative:
                        form = AdjectiveForm.Superlative;
                        break;
                }

                baseForm = splitArr[1];

                return true;
            }

            return false;

        }

        public AdjectiveForm InferAdjectiveFormAndBaseForm(WordObj wordObj, out string baseForm, out WordObj baseFormObj)
        {
            AdjectiveForm likelyForm = AdjectiveForm.None;
            baseFormObj = null;

            bool isMultiSyllabic = IsMultiSyllabicComparativeOrSuperlative(wordObj.Word, out likelyForm, out baseForm);

            if (!isMultiSyllabic)
            {

                string comparative;
                string superlative;

                if (IsKnownIrregular(wordObj.Word, out likelyForm, out baseForm, out comparative, out superlative))
                {
                    return likelyForm;
                }


                if (wordObj.Word.EndsWith(basicComparative))
                {
                    likelyForm = AdjectiveForm.Comparative;
                }

                else if (wordObj.Word.EndsWith(basicSuperlative))
                {
                    likelyForm = AdjectiveForm.Superlative;
                }

                else
                {
                    likelyForm = AdjectiveForm.Base;
                }
            }

            if(likelyForm != AdjectiveForm.Base)
            {
                baseForm = GetBaseForm(wordObj, likelyForm);

                bool baseFormFound = baseForm != null && ResourceDictionary.AdjectivesListContainsWord(baseForm);

                if (!baseFormFound)
                {
                    baseForm = wordObj.Word;
                    likelyForm = AdjectiveForm.Base;
                }

                else
                {
                    baseFormObj = BuildWordObject(baseForm);
                }

            }


            return likelyForm;
        }




        private string GetBasicComparative(WordObj wordObj, AdjectiveForm comparisonType)
        {
            string finalValue = null;
            string word = wordObj.Word;
            string suffix = comparisonType switch
            {
                AdjectiveForm.Comparative => basicComparative,
                AdjectiveForm.Superlative => basicSuperlative,
            };


            if (word.EndsWith(eSuffix))
            {
                suffix = suffix.Substring(1);
                finalValue = word + suffix;
                return finalValue;
            }

            if (word.EndsWith(ySuffix) && wordObj.SyllableCount > 1)
            {
                finalValue = word.Substring(0, word.Length - 1);
                finalValue += "i" + suffix;
                return finalValue;
            }

            finalValue = word;

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
                            finalValue += last;
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

        private string GetMultiSyllabicComparative(WordObj wordObj, AdjectiveForm comparisonType)
        {
            string finalValue = wordObj.Word;

            string prefix = comparisonType switch
            {
                AdjectiveForm.Comparative => multiSyllabicComparative,
                AdjectiveForm.Superlative => multiSyllabicSuperlative,
                _ => throw new NotImplementedException()
            };

            finalValue = prefix + " " + finalValue;

            return finalValue;
        }

        private string GetBaseFormFromSimpleComparativeOrSuperlative(WordObj wordObj)
        {
            string suffix = wordObj.Word.EndsWith(basicComparative) ? basicComparative : basicSuperlative;

            string trimmedWord = wordObj.Word.Substring(0, wordObj.Word.Length - suffix.Length);

            string potentialForm = null;

            if (trimmedWord.EndsWith('i'))
            {
                potentialForm = trimmedWord.Substring(0, trimmedWord.Length - 1) + 'y';
            }

            if(potentialForm != null && ResourceDictionary.DictContainsWord(potentialForm))
            {
                
                return potentialForm;
            }

            return trimmedWord;
        }

        private WordObj BuildWordObject(string word)
        {
            var phonemes = ResourceDictionary.GetPhonemesFromWord(word);
            return new WordObj(word, phonemes);
        }

        private bool IsPastParticiple(WordObj wordObj)
        {
            return wordObj.Word.EndsWith(pastParticipleEnding) && wordObj.Word.Length > 3;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ResourceDictionary.Dispose();
                    ResourceDictionary = null;
                }

                irregularAdjectives = null;
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~AdjectiveInflector()
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
