using CardMaster.Deck.Repository;

namespace CardMaster.Model.Deck
{
    public class Deck
    {
        private readonly ICardRepository _cardRepository;

        public Deck(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public void Add(Card card)
        {
            _cardRepository.Add(card);
        }

        public Card ById(int id)
        {
            return _cardRepository.ById(id);
        }
    }
}