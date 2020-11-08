using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BlackJack;
using BlackJackBLL;
using BlackJackBLL.Services;

namespace BlackJack
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameControl = new GameControl();

            gameControl.PersonPlay();
        }
    }
}
