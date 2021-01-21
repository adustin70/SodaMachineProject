using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachine
{
    class Customer
    {
        //Member Variables (Has A)
        public Wallet Wallet;
        public Backpack Backpack;

        //Constructor (Spawner)
        public Customer()
        {
            Wallet = new Wallet();
            Backpack = new Backpack();
        }
        //Member Methods (Can Do)

        //This method will be the main logic for a customer to retrieve coins from their wallet.
        //Takes in the selected can for price reference
        //Will need to get user input for coins they would like to add.
        //When all is said and done this method will return a list of coin objects that the customer will use as payment for their soda.
        public List<Coin> GatherCoinsFromWallet(Can selectedCan)
        {
            
            List<Coin> userPayment = new List<Coin>();
            string resultOfCoinSelection = "";

            while (resultOfCoinSelection != "Done")
            {
                resultOfCoinSelection = UserInterface.CoinSelection(selectedCan, userPayment);
                Coin coinToChangePlaces = GetCoinFromWallet(resultOfCoinSelection);
                if(coinToChangePlaces == null)
                {
                    break;
                }
                Wallet.Coins.Remove(coinToChangePlaces);
                userPayment.Add(coinToChangePlaces);
            }
            return userPayment;

            //need a way to get user input to add coins
            //needs to return list of coins to be used for payment
            //What can I use to capture coins selected by user then return that as a list of coins to be used for payment
            
        }
        //Returns a coin object from the wallet based on the name passed into it.
        //Returns null if no coin can be found
        public Coin GetCoinFromWallet(string coinName)
        {
            foreach (Coin coin in Wallet.Coins)
            {
                if (coin.Name == coinName)
                {
                    return coin;
                }
            }
            return null;
        }
        //Takes in a list of coin objects to add into the customers wallet.
        public void AddCoinsIntoWallet(List<Coin> coinsToAdd)
        {
            Wallet.Coins.AddRange(coinsToAdd);
        }
        //Takes in a can object to add to the customers backpack.
        public void AddCanToBackpack(Can purchasedCan)
        {
            Backpack.cans.Add(purchasedCan);
        }
    }
}
