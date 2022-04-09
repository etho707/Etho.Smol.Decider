namespace DeciderApp.Classes
{
    public class DecisionPair
    {
        public Decision? Decision1 { get; set; }
        public Decision? Decision2 { get; set; }
        public bool Checked { get; set; } = false;
    }
}
