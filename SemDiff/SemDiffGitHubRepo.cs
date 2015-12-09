using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SemDiff
{
    internal class SemDiffGitHubRepo : SemDiffRepo
    {
        public HttpClient Http { get; }
        public string UserName { get; }
        public string RepoName { get; }
        public List<PullRequest> PullRequests { get; } = new List<PullRequest>();

        public SemDiffGitHubRepo(string username, string reponame)
        {
            UserName = username;
            RepoName = reponame;
            Http = new HttpClient
            {
                BaseAddress = new Uri("https://api.github.com/")
            };
            Http.DefaultRequestHeaders.UserAgent.ParseAdd(nameof(SemDiff));
        }

        protected override void Update()
        {
            var tm = Stopwatch.StartNew();
            UpdateAsync().Wait();
            var elapsed = tm.ElapsedMilliseconds;
        }

        private async Task UpdateAsync()
        {
            //TODO: Need to handle multiple pages, the api will only return sets of 30
            //There is also an unauthenticated limit of 60 api request per hour! (that doesn't seem to count downloading of actual files) So we must be careful
            var response = await Http.GetAsync($"/repos/{UserName}/{RepoName}/pulls");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var pulls = JsonConvert.DeserializeObject<List<PullRequest>>(content);
                PullRequests.Clear();
                PullRequests.AddRange(await Task.WhenAll(pulls.Select(async p =>
                {
                    var res = await Http.GetAsync($"/repos/{UserName}/{RepoName}/pulls/{p.Number}/files");
                    if (res.IsSuccessStatusCode)
                    {
                        var con = await res.Content.ReadAsStringAsync();
                        var files = JsonConvert.DeserializeObject<List<Files>>(con);
                        p.Files = await Task.WhenAll(files.Where(f => f.FileName.EndsWith(".cs")).Select(async f =>
                        {
                            var r = await Http.GetAsync(f.Url);
                            if (r.IsSuccessStatusCode)
                            {
                                var file_content = await r.Content.ReadAsStringAsync();
                                f.SyntaxTree = CSharpSyntaxTree.ParseText(file_content);
                                return f;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }));
                        return p;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                })));
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}