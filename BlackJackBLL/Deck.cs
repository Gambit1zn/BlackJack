using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJackBLL
{
    internal class Deck
    {
        private static readonly IEnumerable<string> _suites = new List<string>() //These 2 lists will and never should change, soooooo static readonly it is :D
        {
            "Clubs",
            "Diamonds",
            "Hearts",
            "Spades"
        };

        private static readonly IEnumerable<string> _values = new List<string>()
        {
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "Jack",
            "Queen",
            "King",
            "Ace"
        };

        private Random _randomNumberGen;

        public Deck()
        {
            _randomNumberGen = new Random();
            DeckOfCards = _suites.SelectMany(suit => _values.Select(name => new Card { Name = name, Suit = suit })).ToList(); // As soon as a instance is created, generate deck :P
            UsedPile = new List<Card>();
        }

        public List<Card> DeckOfCards { get; private set; } // Had to change from IEnumerable to List cause you cannot remove from an IEnumerable :( 
                                                            // il never forget this now as I probably lost an hour and some hairs figuring why im getting the same card for 2 players! 

        private List<Card> UsedPile { get; set; } // I dont wana remove cards from the deck, after game i wana collect cards again and add it to the deck to create more randomness and realism


        public void Shuffle(int amountOfShuffles)
        {


            for (int i = 0; i < amountOfShuffles; i++)
            {
                var shuffleddeck = new List<Card>();

                var top = DeckOfCards.Take(26).GetEnumerator();
                var bottom = DeckOfCards.Skip(26).Take(26).GetEnumerator(); //Split the deck into 2 piles, going to imitate the riffle shuffle

                while (top.MoveNext() && bottom.MoveNext())
                {
                    shuffleddeck.Add(top.Current);
                    shuffleddeck.Add(bottom.Current);
                }

                DeckOfCards = shuffleddeck;
            }
        }

        public Card SelectTop()
        {
            var card = DeckOfCards.FirstOrDefault();

            UsedPile.Add(card); //Add to used pile before removing :)

            DeckOfCards.Remove(DeckOfCards.FirstOrDefault());

            return card;
        }

        public void CollectCards()
        {
            if (UsedPile.Count > 0)
            {
                foreach (var card in UsedPile) // Add used cards back to deck and then clear used pile :P
                {
                    DeckOfCards.Add(card);
                }

                UsedPile.Clear();
            }
        }
    }
}
