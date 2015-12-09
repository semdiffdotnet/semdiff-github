using Microsoft.CodeAnalysis;
using Newtonsoft.Json;

namespace SemDiffAnalyzer
{
    public class Files
    {
        public string FileName { get; set; }

        [JsonProperty("raw_url")]
        public string Url { get; set; }

        public SyntaxTree SyntaxTree { get; set; }
    }
}