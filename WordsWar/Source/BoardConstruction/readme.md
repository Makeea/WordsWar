# Board Construction

### Vision
Board Construction is an offline component that will generate boards, find all the possible words in the board and publish that to the cloud.

The clients will just consume boards created by the construction tool, by separating board construction and consumption, we are able to:
- Make the game client and server more lightweight by focusing only on playing
- Allow players to play specific boards that their friends played on
- Generate Leaderboards
- Tweak the parameters that make a "good board"

### Goals
The Board Construction will do the following:
- Generate random boards.
- Find all possible words on the board based on a specific dictionary.
- Publish the board to the cloud if it achieves specific criteria.

### Components
To build a random board we will use:
- `GameBoard` - the game board itself
- `WordDictionary` - the dictionary of all the possible words.
- `DictionaryLookupInfo`- a support structure to find words in the board
- `WordFinder` - find all words in `WordDictionary`, contained in a `GameBoard`
- `Alphabet` - contains all the letters used in an alphabet to build the board
- `BoardRandomizer` - generates a random board using a specific alphabet
- `BoardEvaluator` - validates if a board with its words has enough quality
- `BoardPublisher` - takes a board and publishes it to the cloud
- `BoardConstructor` - coordinates all the other components.

For now this will all be packaged in the `BoardConstruction.dll` in the [`BoardConstruction`](/WordsWar/Source/BoardConstruction/BoardConstruction) folder 

This is the high level  the `BoardConstructor` uses to create a board with a solution:
```csharp
BoardSolution BuildBoardAndSolve(string alphabetId, string dictionaryId)
{
    // Load the Alphabet
    Alphabet alphabet = Alphabets.Load(alphabetId);

    // Load the Dictionary
    Dictionary dict = Dictionaries.Load(dictionaryId);

    // Build a RandomBoard based on the Alphabet
    GameBoard board = BoardBuilder.GenerateRandom(4, alphabet);

    // Solve the Board using the Dictionary
    List<string> wordsfound = WordFinder.FindWords(board, dictionary);

    return new BoardSolution(board, dictionary, wordsfound);
}

...

BoardSolution sol = BuildBoardAndSolve("en-us", "5kWords");
if (BoardEvaluator.EvaluateBoard(sol)) 
{
    BoardPublisher.Publish(sol);
}

```

##### Notes -
Input and boards are normalized to be lowercase in input and the words found will be returned in lower case.

### Board Construction Runner
This is a command line exe that will call the constructor to generate boards and publish them to the cloud
```sh
BoardConstructionRunner.exe blah blah blah
```
### Unit Test
There is a unit test class for each of the pieces above in the ['BoardConstruction.UnitTest'](/WordsWar/Source/BoardConstruction/BoardConstruction.UnitTest) folder

### Sample Generated Board
Here is what the result looks like - with 300 words found.
```json
{
  "AlphabetId": "en-us",
  "DictionaryId": "english-words",
  "SolvedBoard": {
    "Size": 4,
    "Board": [
      ["n","a","t","e"],
      ["i","r","r","a"],
      ["n","w","w","s"],
      ["h","n","o","d"]
    ]
  },
  "SolutionWords": [
    "aer",
    "aet",
    "ain",
    "air",
    "airn",
    "airt",
    "ani",
    "ara",
    "arain",
    "arar",
    "are",
    "area",
    "areas",
    "areason",
    "aret",
    "arn",
    "arni",
    "arr",
    "arras",
    "arret",
    "arri",
    "ars",
    "arson",
    "art",
    "artar",
    "artarin",
    "arte",
    "arter",
    "atar",
    "ate",
    "atria",
    "awd",
    "awn",
    "don",
    "dos",
    "dosa",
    "dow",
    "down",
    "dows",
    "dsr",
    "ear",
    "ears",
    "eat",
    "era",
    "eras",
    "erat",
    "err",
    "errata",
    "erratas",
    "ers",
    "eta",
    "etas",
    "iare",
    "inn",
    "ira",
    "iran",
    "irate",
    "irater",
    "irrate",
    "nain",
    "nar",
    "nare",
    "narr",
    "narra",
    "narras",
    "narrate",
    "narw",
    "nat",
    "natr",
    "niata",
    "nod",
    "nods",
    "nos",
    "now",
    "nows",
    "ods",
    "osar",
    "owd",
    "own",
    "rain",
    "ran",
    "rani",
    "rara",
    "rare",
    "ras",
    "rason",
    "rat",
    "rata",
    "ratan",
    "rate",
    "rater",
    "raters",
    "raw",
    "raws",
    "rea",
    "reason",
    "reata",
    "ret",
    "retain",
    "retan",
    "retar",
    "retrain",
    "ria",
    "riata",
    "riatas",
    "rin",
    "rte",
    "rwd",
    "sae",
    "saeta",
    "sar",
    "saran",
    "sare",
    "sarra",
    "sart",
    "sartain",
    "sat",
    "sata",
    "satai",
    "satan",
    "sate",
    "saw",
    "sawn",
    "sod",
    "son",
    "sow",
    "sowar",
    "sown",
    "swa",
    "sware",
    "swart",
    "swat",
    "swow",
    "tae",
    "tai",
    "tain",
    "tairn",
    "tan",
    "tar",
    "tara",
    "tare",
    "tarea",
    "tari",
    "tarin",
    "tarn",
    "tarr",
    "tarras",
    "tarre",
    "tarri",
    "tars",
    "tas",
    "taw",
    "tawn",
    "taws",
    "tea",
    "tear",
    "tears",
    "teas",
    "ter",
    "tera",
    "terai",
    "teras",
    "terr",
    "terra",
    "terrain",
    "terran",
    "tra",
    "train",
    "treas",
    "treason",
    "tri",
    "trin",
    "trina",
    "trs",
    "wae",
    "waer",
    "war",
    "ware",
    "warran",
    "warrin",
    "wars",
    "wart",
    "was",
    "wat",
    "water",
    "waters",
    "win",
    "winare",
    "winnow",
    "winnows",
    "wir",
    "wirr",
    "wirra",
    "wod",
    "won",
    "wos",
    "wow",
    "wows",
    "wran",
    "wraw",
    "wreat"
  ]
}

```