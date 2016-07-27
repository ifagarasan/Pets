using System.Collections.Generic;
using CardMaster.Deck.Repository;
using CardMaster.Model.Deck;

namespace CardMaster.Infrastructure.Repository
{
    public class CardRepository : ICardRepository
    {
        private readonly List<Card> _cards;

        public CardRepository()
        {
            _cards = new List<Card>();
        }

        public int Add(Card card)
        {
            _cards.Add(card);
            return _cards.Count;
        }

        public Card ById(int id) => _cards[id-1];
    }
}