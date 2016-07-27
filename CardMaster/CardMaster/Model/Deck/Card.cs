namespace CardMaster.Model.Deck
{
    public class Card
    {
        public Card(string label, string content)
        {
            Label = label;
            Content = content;
        }

        public string Label { get;  }
        public string Content { get; }
    }
}