using System;
using CardMaster.Model.Deck;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardMaster.Infrastructure.Repository;

namespace CardMaster.IntegrationTests
{
    [TestClass]
    public class MongoCardRepositoryShould
    {
        [TestMethod]
        public void StoresCard()
        {
            var card = new Card("Hello, Cards!", "This is my card, saved in Mongo");
            var repository = new MongoCardRepository();

            var id = repository.Add(card);

            Assert.AreEqual(card, repository.ById(id));
        }
    }
}
