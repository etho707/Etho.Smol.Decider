namespace DeciderApp.Classes
{
    public class Decision
    {
        public string? Title { get; set; }
        public int Rating { get; set; } = 0;
    }

    public class DecisionPair
    {
        public Decision? Decision1 { get; set; }
        public Decision? Decision2 { get; set; }
    }
}
