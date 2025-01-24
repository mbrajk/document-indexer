namespace DocumentIndexer;

//files need an id, ideally a hash so path doesn't matter if we want to correlate files that have moved

public record Occurence(int Row, int Col);