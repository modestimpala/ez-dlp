using System.Diagnostics;

class ezdlp
{
    private static string ytDlpPath;
    private static string outputTemplate;
    private static string ffmpegPath;
    private static bool playlistTemplate;
    private static string playlistOutputTemplate;
    private static bool bulkSubdir;
    private static string bulkSubdirName;
    private static int playlistDownloadLimit;
    private static bool embedThumbnail;

    private static void Main(string[] args)
    {
        LoadConfiguration();

        Console.WriteLine("Bulk download? (y/N)");
        bool bulk = Console.ReadLine()?.Equals("y", StringComparison.OrdinalIgnoreCase) ?? false;

        if (!bulk ||  args.Contains("-s"))
        {
            ProcessSingleDownload();
        }
        else
        {
            ProcessBulkDownload();
        }
    }

    private static void LoadConfiguration()
    {
        var iniData = IniFileParser.Parse("ezdlp.ini");
        ytDlpPath = iniData["ytdlp"]["Path"];
        outputTemplate = iniData["ytdlp"]["OutputTemplate"];
        ffmpegPath = iniData["ffmpeg"]["Path"];
        playlistTemplate = iniData["playlist"]["PlaylistTemplate"].Equals("true", StringComparison.OrdinalIgnoreCase);
        playlistOutputTemplate = iniData["playlist"]["PlaylistOutputTemplate"];
        bulkSubdir = iniData["bulk"]["BulkSubdir"].Equals("true", StringComparison.OrdinalIgnoreCase);
        bulkSubdirName = iniData["bulk"]["BulkSubdirName"];
        playlistDownloadLimit = int.Parse(iniData["playlist"]["DownloadLimit"]);
        embedThumbnail = iniData["ytdlp"]["EmbedThumbnail"].Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    private static void ProcessSingleDownload()
    {
        Console.WriteLine("Enter URL:");
        string downloadUrl;
        downloadUrl = Console.ReadLine();
        if(downloadUrl == null) return;
        bool isPlaylist = downloadUrl.Contains("list=");
        string format = PromptForFormat();

        if (isPlaylist && playlistTemplate)
            outputTemplate = playlistOutputTemplate;

        string dlpArgs = GenerateDownloadArguments(downloadUrl, format, isPlaylist);
        StartDownloadProcess(dlpArgs);
    }

    private static void ProcessBulkDownload()
    {
        Console.WriteLine("Enter bulk list path: (txt)");
        string bulkListPath = Console.ReadLine();

        string format = "mp4";
        bool globalFormat = ShouldUseGlobalFormat();

        if (globalFormat)
            format = PromptForFormat();

        if (bulkSubdir)
        {
            outputTemplate = bulkSubdirName + "\\" + outputTemplate;
            playlistOutputTemplate = bulkSubdirName + "\\" + playlistOutputTemplate;
        }

        foreach (string downloadUrl in File.ReadAllLines(bulkListPath ?? ""))
        {
            if (ShouldSkipUrl(downloadUrl)) continue;

            if (!globalFormat)
                format = ExtractFormatFromUrl(downloadUrl);

            Console.WriteLine($"Downloading {downloadUrl}...");
            string dlpArgs = GenerateDownloadArguments(downloadUrl, format, downloadUrl.Contains("list="));
            StartDownloadProcess(dlpArgs);
        }
    }

    private static string PromptForFormat()
    {
        Console.WriteLine("Enter format: (default: mp4)");
        string format = Console.ReadLine() ?? "mp4";
        return format.Equals("") ? "mp4" : format;
    }

    private static bool ShouldUseGlobalFormat()
    {
        Console.WriteLine("Global format or per line? (G/l)");
        string input = Console.ReadLine();
        return string.IsNullOrEmpty(input) || input.Equals("g", StringComparison.OrdinalIgnoreCase);
    }

    private static bool ShouldSkipUrl(string url)
    {
        return url.StartsWith('#') || string.IsNullOrWhiteSpace(url);
    }

    private static string ExtractFormatFromUrl(string url)
    {
        var ytUrlSplit = url.Split('|');
        return ytUrlSplit[^1].Trim();
    }

    private static string GenerateDownloadArguments(string url, string format, bool isPlaylist)
    {
        string dlpArgs = "";
        if (isPlaylist && playlistTemplate)
            outputTemplate = playlistOutputTemplate;

        if (format.Equals("mp3", StringComparison.OrdinalIgnoreCase) || format.Equals("wav", StringComparison.OrdinalIgnoreCase))
        {
            dlpArgs = $"-x --audio-format \"{format}\" --add-metadata --ffmpeg-location \"{ffmpegPath}\" --output \"{outputTemplate}\"";
        }
        else
        {
            dlpArgs = $"-f bestvideo+bestaudio --merge-output-format \"{format}\" --add-metadata --ffmpeg-location \"{ffmpegPath}\" --output \"{outputTemplate}\"";
        }

        if (isPlaylist && playlistDownloadLimit > 0)
            dlpArgs += $" -I :{playlistDownloadLimit}";

        if (embedThumbnail)
            dlpArgs += " --embed-thumbnail";

        return dlpArgs + $" \"{url}\"";
    }

    private static void StartDownloadProcess(string arguments)
    {
        string ytDlpExecutable = string.IsNullOrEmpty(ytDlpPath) ? "yt-dlp.exe" : $"{ytDlpPath}\\yt-dlp.exe";
        Process.Start(ytDlpExecutable, arguments).WaitForExit();
    }
}