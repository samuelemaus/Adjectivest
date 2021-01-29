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

            float inMemElapsed = 0;
            for (int i = 0; i < Adjectives.Count; i++)
            {
                stopWatch.Start();
                RunTest(i, true);
                stopWatch.Stop();
                inMemElapsed += stopWatch.ElapsedMilliseconds;
                Console.WriteLine(stopWatch.ElapsedMilliseconds + " ms elapsed." );
                Console.WriteLine();
                stopWatch.Reset();
            }

            float inMemAvg = inMemElapsed / Adjectives.Count;

            //float fileReadElapsed = 0;
            ////FileRead
            //for (int i = 0; i < Adjectives.Count; i++)
            //{
            //    stopWatch.Start();
            //    RunTest(i, false);
            //    stopWatch.Stop();               
            //    fileReadElapsed += stopWatch.ElapsedMilliseconds;
            //    Console.WriteLine(stopWatch.ElapsedMilliseconds);
            //    stopWatch.Reset();
            //}

            //float fileReadAvg = fileReadElapsed / Adjectives.Count;

            Console.WriteLine("Total: " + inMemElapsed + " Avg:" + inMemAvg);

            Console.WriteLine("Press R to run again");
            Console.WriteLine("________________");
            bool gotoStart = Console.ReadKey().KeyChar == 'r';
            if (gotoStart)
            {
                goto START;
            }

        }

        static void RunTest(int i, bool loadIntoMemory)
        {
            Console.WriteLine("Before: " + GC.GetTotalMemory(true) / 1000 + " KB.");

            AdjectiveInflector inflector = new AdjectiveInflector(loadIntoMemory);

            string adj = Adjectives[i];

            string inflection = inflector.GetAdjectiveInflections(adj);

            Console.WriteLine(inflection);
            Console.WriteLine("During: " + GC.GetTotalMemory(false) / 1000 + " KB.");
            //inflector.Dispose();
            Console.WriteLine("After: " + GC.GetTotalMemory(true) / 1000 + " KB.");
        }

        static List<string> Adjectives = new List<string>()
        {
            "sad","gigantic","elegant","red","pissed","unbelievable", "exciting", "crungus","woogle","round","fantastic","large","big","bad","good","zany","zesty", "aggravating"
        };
    }
}
