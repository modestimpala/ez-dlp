
# README for ezdlp

## Overview

`ezdlp` is a console application designed to facilitate downloading videos or playlists from video-sharing websites using `yt-dlp`. It provides an easy-to-use interface for single and bulk downloads, with various configuration options such as format selection, output template customization, and download limit settings.

## Prerequisites

-   .NET runtime to run the application.
-   `yt-dlp` installed on your system.
-   `ffmpeg` for processing video and audio formats.

## Configuration (`ezdlp.ini`)

Before using `ezdlp`, configure it by editing the `ezdlp.ini` file. The following settings can be customized:

-   `ytDlpPath`: The file path to the `yt-dlp` executable.
-   `outputTemplate`: The output template for downloaded files.
-   `ffmpegPath`: The file path to the `ffmpeg` executable.
-   `playlistTemplate`: Whether to use a different template for playlist downloads.
-   `playlistOutputTemplate`: The output template for downloaded playlist items.
-   `bulkSubdir`: Whether to download bulk items into a separate subdirectory.
-   `bulkSubdirName`: The name of the subdirectory for bulk downloads.
-   `playlistDownloadLimit`: The maximum number of items to download from a playlist.
-   `embedThumbnail`: Whether to embed a thumbnail in the downloaded file.

## Usage

### Running the Application

To start `ezdlp`, execute the compiled binary in a terminal or command prompt.

### Single Download

1.  Run `ezdlp`.
2.  When prompted, enter `n` for bulk download.
3.  Enter the URL of the video or playlist to download.
4.  If applicable, specify the format (e.g., mp4, mp3).

### Bulk Download

1.  Run `ezdlp`.
2.  When prompted, enter `y` for bulk download.
3.  Provide the path to a text file containing URLs, one per line. Use `#` to comment out lines.
4.  Choose the global format or specify formats per line in the text file (e.g., URL|format).

### Download Formats

-   For video, the default format is `mp4`. You can specify other formats like `mp3` or `wav` for audio extraction.
-   In bulk download mode, you can set a global format or specify a format for each URL in the list by appending `|format` to the URL.

### Advanced Options

-   Specify a download limit for playlists.
-   Choose to embed thumbnails in the downloaded files.
