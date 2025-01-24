namespace DocumentIndexer;

public class Indexer
{
    private Dictionary<string, Dictionary<FileId, List<Occurence>>> _words = new();
    
    // would want to monitor for new files, but another class would do this
    // would also want to monitor for file changes, potentially rescan the entire file if a change is detected, 
    // otherwise results would become stale
    // OCR could be done for documents that are not text
    // Dates and nonstandard text would be searchable but only in the exact format presented
    // e.g. 12/12/2024 cannot be searched by "Dec 12 2024", but this could be handled by another class that 
    // manages text input from the user and converts it to common patterns. All explicit text searches are O(1) so 
    // we could make any number of text transforms before doing a lookup
    public Indexer()
    {
        
    }

    public async Task<Dictionary<FileId, List<Occurence>>> GetFilesContainingWordAsync(string word)
    {
        if (!_words.ContainsKey(word))
        {
            return new();
        }
        
        return _words[word];
    }
    
    public async Task<FileId> ProcessFileAsync(string filePath)
    {
        Console.WriteLine($">>>  {filePath}");
        var fileId = new FileId(filePath);
        
        if (_words.ContainsKey(filePath))
        {
            Console.WriteLine($"!Err: {filePath}\n file at this path already exists");
            //could re-process because contents could have changed
            return fileId;
        }

        int uniqueWordsAdded = 0;
        //hyphenated words will break with read all lines but this is just a poc anyway
        var fileLines = await File.ReadAllLinesAsync(filePath);
        foreach (var (i, line) in fileLines.Select((s, i) => ( i, s )))
        {
            foreach (var word in line.Split(' '))
            {
                if (!_words.ContainsKey(word))
                {
                    uniqueWordsAdded++;
                    _words.Add(word, new Dictionary<FileId, List<Occurence>>());
                }
                
                if (!_words[word].ContainsKey(fileId))
                {
                    _words[word].Add(fileId, new List<Occurence>());    
                }
               
                //i dont actually know the column so just use a random number for poc 
                _words[word][fileId].Add(new Occurence(i, new Random().Next(20_000)));
                
            }
        }
        Console.WriteLine($"Done processing file. Added {uniqueWordsAdded} unique words to index.");

        return fileId;
    }
}