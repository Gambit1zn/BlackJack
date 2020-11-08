using BlackJackBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace BlackJackBLL
{
    public class Dealer
    {
        private int _total = 0;
        private static Deck _deck;
        private static readonly int _hitLimit = 17;

        public Dealer()
        {
            Hand = new ObservableCollection<Card>();
            Hand.CollectionChanged += CalculateTotal;
        }

        static Dealer()
        {
            _deck = new Deck();
            _deck.Shuffle(5);
        }


        public bool Stand { get; private set; }
        public bool Bust { get; private set; }
        public int Total {
            get 
            {
                return _total;
            }
            private set
            {
                _total = value;
            } 
        } // This must not be set outside of this class :)
        public ObservableCollection<Card> Hand { get; set; }

        public static Card DealCard()
        {
            var card = _deck.SelectTop();

            return card;
        }

        public void HitMe()
        {
            if (!Stand && _total < 17)
            {
                var card = DealCard(); //Dealer deals card to himself

                Hand.Add(DealCard());

                if (_total >= _hitLimit && _total <= 21)
                    Stand = true; // Auto stand if dealer gets desired score within turn:P
                else if (_total > 21)
                    Bust = true;
            }
            else
            {
                Stand = true;
            }
        }

        public void CollectCards()
        {
            _deck.CollectCards(); //Collect cards from players hands and add it to the deck
        }

        public void Reinitialize()
        {
            _total = 0;
            Stand = false;
            Bust = false;
            Hand.Clear();
            _deck.Shuffle(2);
        }

        private void CalculateTotal(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset) // Dont do this when hand being reset to avoid null ref exception :)
            {
                var card = (Card)e.NewItems[0];

                _total += card.Value;
            }
        }
    }
}
