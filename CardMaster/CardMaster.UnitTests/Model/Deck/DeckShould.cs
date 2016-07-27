using CardMaster.Deck.Repository;
using CardMaster.Model.Deck;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace CardMaster.UnitTests.Model.Deck
{
    [TestClass]
    public class DeckShould
    {
        [TestMethod]
        public void StoresCard()
        {
            var card = new Card("Test card", "Test content");
            var cardRepository = Substitute.For<ICardRepository>();
            var deck = new CardMaster.Model.Deck.Deck(cardRepository);

            deck.Add(card);

            cardRepository.Received().Add(card);
        }
    }
}
