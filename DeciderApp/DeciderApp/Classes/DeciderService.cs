namespace DeciderApp.Classes
{
    public class DeciderService
    {
        private List<Decision> _allDecisions;
        private List<DecisionPair> _currentPairs;
        private bool _loaded;

        public DecisionPair ActualPair { get; set; }

        public void Load(string filename)
        {
            if (_loaded == false)
            {
                var lines = File.ReadAllLines(filename);
                _allDecisions = new List<Decision>();
                foreach (var line in lines)
                {
                    _allDecisions.Add(new Decision() { Title = line, Score = 0 });
                }
                //_allDecisions = EvenDecisions(_allDecisions);
                _currentPairs = GetHighestPairs(_allDecisions);
                ActualPair = _currentPairs.First(x => x.Checked == false);
                _loaded = true;
            }
        }

        private List<Decision> EvenDecisions(List<Decision> decisions)
        {

            if (decisions.Count % 2 != 0)
                decisions.Add(new Decision() { Title = "empty-decision", Score = decisions.First().Score });
            return decisions;
        }

        private List<DecisionPair> GetHighestPairs(List<Decision> decisions)
        {
            var highestScore = decisions.Max(x => x.Score);
            var decisionsHighestRank = decisions.Where(x => x.Score == highestScore).ToList();
            var shuffled = decisionsHighestRank.ShuffleMe();
            var even = EvenDecisions(shuffled);
            var pairs = new List<DecisionPair>();
            for (int i = 0; i < even.Count; i++)
            {
                if (i % 2 == 0)
                    pairs.Add(new DecisionPair() { Decision1 = even[i], Decision2 = even[i + 1] });
            }
            return pairs;
        }

        public List<string> ToTextLines()
        {
            var sorted = _allDecisions.OrderByDescending(x => x.Score);
            return sorted.Select(x => x.Score + " - " + x.Title).ToList();
        }

        public DecisionPair? GetNextActualPair()
        {
            if (_currentPairs.All(x => x.Checked))
                _currentPairs = GetHighestPairs(_allDecisions);
            ActualPair = _currentPairs.First(x => x.Checked == false);
            return ActualPair;
        }
    }
}
