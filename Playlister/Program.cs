using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Search;
using System.Collections.Generic;
using YoutubeExplode.Common;

class Program
{
    static async Task Main(string[] args)
    {
        string inputFile = "input.txt";
        string outputFile = "output_urls.txt";
        string searchTerm = " OST music";

        YoutubeClient client = new YoutubeClient();
        var lines = File.ReadAllLines(inputFile);
        var failedQueries = new List<string>();
        int batchSize = 8;

        await ProcessBatch(lines, client, searchTerm, outputFile, failedQueries, batchSize);

        while (failedQueries.Count > 0)
        {
            var remainingFailedQueries = new List<string>(failedQueries);
            failedQueries.Clear();

            await ProcessBatch(remainingFailedQueries.ToArray(), client, searchTerm, outputFile, failedQueries, batchSize);
        }
    }

    static async Task ProcessBatch(string[] lines, YoutubeClient client, string searchTerm, string outputFile, List<string> failedQueries, int batchSize)
    {
        for (int i = 0; i < lines.Length; i += batchSize)
        {
            var batch = lines.Skip(i).Take(batchSize);
            var searchTasks = batch.Select(line => ProcessLine(client, line, searchTerm, outputFile, failedQueries)).ToArray();
            await Task.WhenAll(searchTasks);
        }
    }

    static async Task ProcessLine(YoutubeClient client, string line, string searchTerm, string outputFile, List<string> failedQueries)
    {
        if(line.Contains(searchTerm))
            searchTerm = "";
        string query = line + searchTerm;
        try
        {
            var searchResults = await client.Search.GetPlaylistsAsync(query);
            var firstPlaylist = searchResults.FirstOrDefault();
            if (firstPlaylist != null)
            {
                string playlistUrl = "https://www.youtube.com/playlist?list=" + firstPlaylist.Id;
                Console.WriteLine(playlistUrl + " | " + query);
                await File.AppendAllTextAsync(outputFile, playlistUrl + Environment.NewLine);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing query '{query}': {ex.Message}");
            lock (failedQueries)
            {
                failedQueries.Add(query);
            }
        }
    }
}
