namespace SemDiffAnalyzer
{
    internal class PullRequest
    {
        public string Url { get; set; }
        public int Number { get; set; }
        public Files[] Files { get; internal set; }
    }
}