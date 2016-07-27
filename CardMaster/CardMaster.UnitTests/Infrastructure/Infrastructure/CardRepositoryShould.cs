using CardMaster.Deck.Repository;
using CardMaster.Infrastructure.Repository;
using CardMaster.Model.Deck;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace CardMaster.UnitTests.Infrastructure.Infrastructure
{
    [TestClass]
    public class CardRepositoryShould
    {
        [TestMethod]
        public void StoresCard()
        {
            var card = new Card("My card", "My content");
            var repository = new CardRepository();

            repository.Add(card);

            Assert.AreEqual(card, repository.ById(1));
        }
    }
}
