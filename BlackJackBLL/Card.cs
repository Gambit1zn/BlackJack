using System;

namespace BlackJackBLL
{
    public class Card
    {
        private string _name;

        public string Suit { get; set; }
        public string Name {
            get {

                return _name;
            } 
            set {
                _name = value;

                switch (_name)
                {
                    case "Jack":
                        Value = 10;
                        break;
                    case "Queen":
                        Value = 10;
                        break;
                    case "King":
                        Value = 10;
                        break;
                    case "Ace":
                        Value = 1;
                        break;
                    default:
                        Value = Convert.ToInt32(Name);
                        break;
                }
            } 
        }
        public int Value { get; private set; } // Should not be set outside of this class, our constructor will will take care of that :D
    }
}
