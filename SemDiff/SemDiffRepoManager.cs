using System;

namespace SemDiffAnalyzer
{
    public class SemDiffRepoManager
    {
        public TimeSpan UpdateTimeLimit { get; } = TimeSpan.FromHours(0.5);
        private DateTime LastUpdate { get; set; }

        public static SemDiffRepoManager GetRepo()
        {
            var rm = new SemDiffRepoManager();
            rm.Initialize();
            return rm;
        }

        public void Initialize()
        {
            Update();
        }

        public void TriggerUpdate()
        {
            var timeToCheckNext = LastUpdate + UpdateTimeLimit;
            if (DateTime.Now > timeToCheckNext)
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