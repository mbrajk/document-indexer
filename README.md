# Document Indexer

## Overview

This is a proof of concept to better understand how text processing is handled.
This tool provides a basic implementation of a document indexing system that allows users to index the contents of files and perform quick lookups for specific words. I am attempting to design it with performance in mind, generally providing between `O(1)` or `O(logn)` lookups with fallbacks to slower searching when needed. I look forward to finding out why document searching is hard and why my features are untenable.

We accomplish this by initially processing all of the files that will be indexed and storing references efficiently to allow for quick retrieval. Ultimately the idea is for the system to provide **tiered search results**, meaning it delivers the fastest results (e.g., exact matches) immediately and continues to execute slower searches (e.g., advanced patterns or normalized data) in the background, ensuring a responsive user experience.

---

## Current Features

- **Word Indexing**: Maps words to the files and positions where they appear.

- **File Processing**: Reads files line by line, tokenizes words, and indexes their positions.
- **Scalability**: Attempting to handle large datasets efficiently with `O(1)` explicit text lookups and fallbacks to slower search algorithms.

---

## Considerations And Potential Improvements

- **Date searching**: Standardize dates to allow O(1) date lookup regardless of format provided by user
- **Begin perf monitoring**: Will not know which improvements are meaningful if we metrics around memory and speed are not tracked. This should be one of the first things to consider for any feature.
- **Prefix searching**: Trie data structure to do prefix searches
- **Infix and Postfix Search**: Plus multi-word search
- **JSON format**: Is there another way that json files should be handled such that we can query it like can be done with `jq`
- **OCR**: When processing non-text files, we can OCR files to turn them into text documents.
- **Real-Time Monitoring**: There is no real benefit to a file search tool that can't search newly added files
- **Store processed files**: Currently the application must process every file on startup, as there is no persistent storage mechanism
- **Efficient memory mechanism**: Right now the application is using objects and lists to store information about files. If we can keep the index in memory without massive overhead that would be ideal. The current format of storage is likely to grow quickly.
- **Github issues page will be updated with further improvements**
---

## Design Concepts

### 1. **Indexing Data Structure**
Currently a dictionary is the core data structure. It maps:
- **Word (string)** → A dictionary of files where the word occurs.
  - **FileId** → A list of occurrences in the file.

Sample Structure:
```json
{
    "example": {
        "file1.txt": ["Occurrence":{"Line": 1, "Column": 5}, "Occurrence":{"Line": 2, "Column": 15}]
    },
    "car": {
        "file3.txt": ["Occurrence":{"Line": 2, "Column": 5}]
    }
}
