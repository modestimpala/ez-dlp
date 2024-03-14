
# README for Youtube Playlist Fetcher

## Overview

This console application leverages the YoutubeExplode library to automate the process of fetching YouTube playlist URLs based on search terms. Users can input a list of search terms, and the application will append " OST music" to each term, search for relevant YouTube playlists, and output the URLs to a specified file.

## Prerequisites

-   .NET runtime to execute the application.
-   The YoutubeExplode library, which can be added via NuGet.

## Input

-   The application reads from an `input.txt` file containing search terms, one per line.
-   Users can modify the `searchTerm` variable within the code to change the default search term appended to each query.

## Output

-   Playlist URLs are written to `output_urls.txt`.
-   The output includes the playlist URL followed by the search query that produced it.

## Functionality

-   The application processes search queries in batches to optimize performance, with a default batch size of 8.
-   If a query fails (e.g., due to network issues), the application will retry it in subsequent batches.
-   Search results prioritize playlists; the first result's URL is captured.

## Usage

1.  Ensure `input.txt` contains the desired search terms, each on a new line.
2.  Run the program. The console will display live updates as playlists are found and errors are encountered.
3.  After execution, `output_urls.txt` will contain the found playlist URLs.

## Customization

-   Change `inputFile` and `outputFile` variables to use different file paths.
-   Modify `searchTerm` to alter the appended string for each search query.
-   Adjust `batchSize` to increase or decrease the number of concurrent search queries.

## Error Handling

-   The program logs errors to the console and retries failed queries.
-   If persistent errors occur, check the network connection or the validity of the search terms.

## Libraries Used

-   YoutubeExplode is utilized for interfacing with YouTube's search functionality.
