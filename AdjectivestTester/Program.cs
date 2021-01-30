using System;
using System.Collections.Generic;
using System.Diagnostics;
using Adjectivest;
using Adjectivest.Phonemes;


namespace AdjectivestTester
{
    class Program
    {
        static void Main(string[] args)
        {
            START:
            Console.WriteLine("________________");
            Stopwatch stopWatch = new Stopwatch();
            AdjectiveInflector inflector = new AdjectiveInflector(false);

            float totalElapsed = 0;
            for (int i = 0; i < Adjectives.Count; i++)
            {
                stopWatch.Start();
                RunTest(i, inflector);
                stopWatch.Stop();
                totalElapsed += stopWatch.ElapsedMilliseconds;
                stopWatch.Reset();
            }

            float avg = totalElapsed / Adjectives.Count;

            Console.WriteLine("Total: " + totalElapsed + " Avg:" + avg);

            Console.WriteLine("Press R to run again");
            Console.WriteLine("________________");
            bool gotoStart = Console.ReadKey().KeyChar == 'r';
            if (gotoStart)
            {
                goto START;
            }

        }

        static void RunTest(int i, AdjectiveInflector inflector)
        { 
            string adj = Adjectives[i];

            string inflection = inflector.GetAdjectiveInflections(adj);

                        
        }

        static List<string> Adjectives = new List<string>()
        {
            "sad","gigantic","elegant","red","pissed","unbelievable", "exciting", "better", "coolest", "crungus","woogle","round","fantastic","large","big","bad","good","zany","zesty", "aggravating"
        };
    }
}
