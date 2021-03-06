namespace DeciderApp.Classes
{
    public class DeciderService
    {
        private List<Decision> _allDecisions;
        private List<DecisionPair> _currentPairs;
        private bool _loaded;
        private int _fileCounter;
        private string _fileName;

        public DecisionPair ActualPair { get; set; }
        public int ActualScoreLevel { get; set; } = 0;
        public int TotalPairs { get; set; } = 0;

        public void Load(string filename)
        {
            if (_loaded == false)
            {
                _fileName = filename;
                var lines = File.ReadAllLines(_fileName);
                _allDecisions = new List<Decision>();
                foreach (var line in lines)
                {
                    _allDecisions.Add(new Decision() { Title = line, Score = 0 });
                }
                //_allDecisions = EvenDecisions(_allDecisions);
                _currentPairs = GetHighestPairs(_allDecisions);
                var pairNum = 0;
                foreach (var pair in _currentPairs)
                {
                    pairNum++;
                    pair.PairNum = pairNum;
                }
                TotalPairs = _currentPairs.Count;
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
            {
                _currentPairs = GetHighestPairs(_allDecisions);
                var pairNum = 0;
                foreach (var pair in _currentPairs)
                {
                    pairNum++;
                    pair.PairNum = pairNum;
                }
                TotalPairs = _currentPairs.Count;
                ActualPair = _currentPairs.First(x => x.Checked == false);
                File.WriteAllLines($"{_fileName}.{_fileCounter}.txt", ToTextLines());
            }
            else
                ActualPair = _currentPairs.First(x => x.Checked == false);
            return ActualPair;
        }

        public void LoadSave(string fileName)
        {
            _fileName = fileName;

            var lines = File.ReadAllLines(_fileName);
            _allDecisions = new List<Decision>();
            foreach (var line in lines)
            {
                var score = line[0] - '0';
                var newLine = line.Substring(4);
                _allDecisions.Add(new Decision() { Title = newLine, Score = score });
            }
            //_allDecisions = EvenDecisions(_allDecisions);
            _currentPairs = GetHighestPairs(_allDecisions);
            var pairNum = 0;
            foreach (var pair in _currentPairs)
            {
                pairNum++;
                pair.PairNum = pairNum;
            }
            TotalPairs = _currentPairs.Count;
            ActualPair = _currentPairs.First(x => x.Checked == false);
            _loaded = true;
        }
    }
}
