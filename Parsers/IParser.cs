namespace DLBuddy.Parsers
{
    public interface IParser
    {
        /// <summary>
        /// Tries parsing shared text.
        /// </summary>
        /// <param name="input">Shared text</param>
        /// <param name="result">The parsed url if succeeded, null if failed</param>
        /// <returns>Whether parsing succeeded.</returns>
        bool TryParseShareText(string input, out string result);

        /// <summary>
        /// Tries fetching a video url from a share url.
        /// </summary>
        /// <param name="source">URL to shared post.</param>
        /// <param name="result">Raw video URL fetched from post if succeeded, null if failed.</param>
        /// <returns>Whether fetching MP4 URL succeeded.</returns>
        bool TryFetchVideoUrl(string source, out string[] result);

        /// <summary>
        /// Gets the parser name.
        /// </summary>
        /// <returns>Site name, which should be valid for filenames.</returns>
        string GetParserName();
    }
}