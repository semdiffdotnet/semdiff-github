using System;

namespace SemDiffAnalyzer
{
    public abstract class SemDiffRepo
    {
        public TimeSpan UpdateTimeLimit { get; } = TimeSpan.FromHours(0.5);
        private DateTime? LastUpdate { get; set; }

        public void TriggerUpdate()
        {
            if (!LastUpdate.HasValue || DateTime.Now > LastUpdate + UpdateTimeLimit)
            {
                Update();
                LastUpdate = DateTime.Now;
            }
        }

        protected abstract void Update();
    }
}