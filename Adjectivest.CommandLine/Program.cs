using System;
using Adjectivest;
using Microsoft.Extensions.CommandLineUtils;

namespace Adjectivest.CommandLine
{
    class Program
    {
        const string applicationName = "adjectivest";
        const string applicationDescription = "Inflection of English adjectives to comparative and superlative forms.";
        const string helpOptions = "-?|-h|--help";

        static void Main(string[] args)
        {
            var app = GetApplication();

            
        
        }

        static CommandLineApplication GetApplication()
        {
            var app = new CommandLineApplication(false);
            app.Name = applicationName;
            app.Description = applicationDescription;
            app.HelpOption(helpOptions);

            return app;
        }

    }
}
