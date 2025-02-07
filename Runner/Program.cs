using DocumentIndexer;

var indexer = new Indexer();
var files = Directory.EnumerateFiles("/Volumes/heap/documents/Obsidian Vault/0.1 Daily notes");

Console.WriteLine("Processing files...");
foreach (var file in files)
{
    await indexer.ProcessFileAsync(file);
}

Console.Write("Enter search term: ");
while (true)
{
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }
    var containingFiles = await indexer.GetFilesContainingWordAsync(input);

    if (containingFiles.Any())
    {
        foreach (var file in containingFiles.Keys)
        {
            Console.WriteLine($"Found in {file.Path} at: ");
            foreach (var occurence in containingFiles[file])
            {
                Console.WriteLine($"{{{occurence.Row}}}:{{{occurence.Col}}}");
            } 
        }
    }
    else
    {
        Console.WriteLine($"{input} was not found in any files");
    }
     
    Console.Write("Enter another search term: ");
}