using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SemDiffAnalyzer
{
    public class SemDiffRepoManager
    {
        public TimeSpan UpdateTimeLimit { get; } = TimeSpan.FromHours(0.5);
        private DateTime? LastUpdate { get; set; }
        private static ConcurrentDictionary<string, SemDiffRepoManager> ManagerLookup = new ConcurrentDictionary<string, SemDiffRepoManager>();

        //given a the path of a file in the repo, get a repo manager
        public static SemDiffRepoManager GetRepoFor(string filePath)
        {
            var rm = ManagerLookup.GetOrAdd(Path.GetDirectoryName(filePath), AddLookupEntry);

            if (rm != null)
                rm.TriggerUpdate();
            return rm;
        }

        private static Regex _locateGitHubUrl = new Regex(@"https:\/\/github\.com\/(.*)\/(.*)\.git");

        private static SemDiffRepoManager AddLookupEntry(string filePath)
        {
            var gitdir = Path.Combine(filePath, ".git");
            if (Directory.Exists(gitdir) && File.Exists(Path.Combine(gitdir, "config")))
            {
                //We have reached the root of the repo!
                var config = File.ReadAllText(Path.Combine(gitdir, "config"));
                var match = _locateGitHubUrl.Match(config);
                if (!match.Success)
                    throw new NotImplementedException();

                var url = match.Value;
                var user = match.Groups[1].Value;
                var repo = match.Groups[2].Value;

                //We need to actually add it with the base path so that other directories can walk up and find it
                return ManagerLookup.GetOrAdd(filePath, s =>
                {
                    var rm = new SemDiffRepoManager();
                    return rm;
                });
            }
            else
            {
                //Go up a directory and check it out
                var parent = Path.GetDirectoryName(filePath);
                if (parent == null)
                {
                    //This file is not in a git repo! (GetDirectoryName returns null when given the root directory)
                    return null; //This is much more common than you might think, because offten random files are compiled, this will allow us to exclude them
                }
                return ManagerLookup.GetOrAdd(parent, AddLookupEntry);
            }
        }

        public void TriggerUpdate()
        {
            if (!LastUpdate.HasValue || DateTime.Now > LastUpdate + UpdateTimeLimit)
            {
                Update();
            }
        }
        private void Update()
        {
            //After success
            LastUpdate = DateTime.Now;
        }
    }
}