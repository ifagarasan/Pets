using CardMaster.Model.Deck;

namespace CardMaster.Deck.Repository
{
    public interface ICardRepository
    {
        int Add(Card card);
        Card ById(int id);
    }
}