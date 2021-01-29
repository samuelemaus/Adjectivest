# Adjectivest

This is a small library for inflecting Adjectives in English between their comparative, superlative, and base forms.

I was working with some NLP libraries in .NET and couldn't find something like this, so I took it upon myself to put this together.

Essentially, if you're working with NLP and have any kind of need to inflect English adjectives, this should cover your need. 

## How to Use:

Clone the repo wherever you'd like:

```
git clone https://github.com/samuelemaus/Adjectivest
```

### Option A) Include Adjectivest in your Solution

#### Installing
______________

*This is assuming you are using Visual Studio.*

In your Solution, right-click on the Solution in the Solution Explorer.  Highlight **'Add'** and select **'Existing Project'**.  Then select the **Adjectivest.csproj** file in the **Adjectivest** directory of wherever you cloned the repository.

You can then reference the Project however you'd like.  When you build your solution, a folder called **'AdjectivestResources'** should appear in your output folder.

#### Using Adjectivest in Code
______________

Add **'Adjectivest'** to your using statements in whatever class you're using Adjectivest with.

The only class you should need to utilize in a typical scenario is the **AdjectiveInflector** class, and you should only need one instance.

The only parameter in the class's constructor is the 'loadDictionaryIntoMemory' bool.  This determines whether the CMU Dictionary is loaded into memory or accessed on-demand.  The difference may be obvious, but loading it into memory is faster on each individual request at the cost of, well, memory.  Accessing it on each request is a bit slower but more memory-efficient.

`AdjectiveInflector inflector = new AdjectiveInflector(true);`

#### Examples:
______________

Use the **'GetAdjectiveInflections'** method to get a pipe-delimited string of the adjective's base, comparative, and superlative forms (in that order).

*Entering a base, comparative, or superlative form of the same adjective should get the same result.*

```
string adj = "heavy";

string inflection = inflector.GetAdjectiveInflections(adj);

//output: "heavy|heavier|heaviest"
//an input of "heavier" or "heaviest" would have same output.
```


### Option B) Interface with Adjectivest via Command-Line

This is a significantly more niche use-case, but if for whatever reason you need to interface with Adjectivest through a command-line, I've included an executable that will allow you to do so.

