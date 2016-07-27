using CardMaster.Deck.Repository;
using CardMaster.Model.Deck;

namespace CardMaster.Infrastructure.Repository
{
    public class MongoCardRepository: ICardRepository
    {
        public MongoCardRepository()
        {
        }

        public int Add(Card card)
        {
            throw new System.NotImplementedException();
        }

        public Card ById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}