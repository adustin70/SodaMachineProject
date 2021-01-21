using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachine
{
    class Wallet
    {
        //Member Variables (Has A)
        public List<Coin> Coins;
        //Constructor (Spawner)
        public Wallet()
        {
            Coins = new List<Coin>();
            FillRegister();
        }
        //Member Methods (Can Do)
        //Fills wallet with starting money
        private void FillRegister()
        {
            Penny penny = new Penny();
            Nickel nickel = new Nickel();
            Dime dime = new Dime();
            Quarter quarter = new Quarter();

            for (int i = 0; i < 8; i++)
            {
                Coins.Add(quarter);
            }
            for (int i = 0; i < 15; i++)
            {
                Coins.Add(dime);
            }
            for (int i = 0; i < 20; i++)
            {
                Coins.Add(nickel);
            }
            for (int i = 0; i < 50; i++)
            {
                Coins.Add(penny);
            }
        }
    }
}
