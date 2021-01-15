using System;
using Adjectivest;
using Adjectivest.Phonemes;
using Adjectivest.Inflection;


namespace AdjectivestTester
{
    class Program
    {
        static void Main(string[] args)
        {
            
            AdjectiveInflector inflector = new AdjectiveInflector();

            START:
            Console.WriteLine("Enter adjective: ");
            string adj = Console.ReadLine().Trim();

            string inflections = inflector.GetAdjectiveInflections(adj);

            Console.WriteLine(inflections);
            Console.WriteLine();
            goto START;

        }
    }
}
