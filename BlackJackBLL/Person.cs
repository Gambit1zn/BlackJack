using BlackJackBLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace BlackJackBLL
{
    public class Person
    {
        private int _total = 0;
        private static readonly int _hitLimit = 21;

        public Person()
        {
            Hand = new ObservableCollection<Card>();
            Hand.CollectionChanged += CalculateTotal;
        }


        public bool Stand { get; set; }

        public ObservableCollection<Card> Hand { get; private set; }
        public bool Bust { get; private set; }
        public int Total
        {
            get
            {
                return _total;
            }
            private set
            {
                _total = value;
            }
        } // not accessible (set) outside of class

        public void HitMe()
        {
            if (!Stand || Bust)
            {
                var card = Dealer.DealCard();

                Hand.Add(card);


                if(_total > 21)
                    Bust = true;
            }
        }

        public void Reinitialize()
        {
            _total = 0;
            Stand = false;
            Bust = false;
            Hand.Clear();
        }

        private void CalculateTotal(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                var card = (Card)e.NewItems[0]; // Cast back into card object to retriev value and add to total :D

                _total += card.Value;
            }
        }
    }
}
