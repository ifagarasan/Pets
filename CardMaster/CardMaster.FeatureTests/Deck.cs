using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardMaster.Model.Deck;
using CardMaster.Infrastructure.Repository;

namespace CardMaster.FeatureTests
{
    [TestClass]
    public class Deck
    {
        private const int CardId = 1;

        [TestMethod]
        public void StoresCards()
        {
            var deck = new Model.Deck.Deck(new CardRepository());
            var card = new Card("Hello, cards!", "This is my awesome content!");

            deck.Add(card);

            Assert.AreEqual(card, deck.ById(CardId));
        }
    }
}
