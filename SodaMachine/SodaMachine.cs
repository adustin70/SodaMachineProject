using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachine
{
    class SodaMachine
    {
        //Member Variables (Has A)
        private List<Coin> _register;
        private List<Can> _inventory;

        //Constructor (Spawner)
        public SodaMachine()
        {
            _register = new List<Coin>();
            _inventory = new List<Can>();
            FillInventory();
            FillRegister();
        }

        //Member Methods (Can Do)

        //A method to fill the sodamachines register with coin objects.
        public void FillRegister()
        {
            for (int i = 0; i < 50; i++)
            {
                Penny penny = new Penny();
                _register.Add(penny);
            }
            for (int i = 0; i < 20; i++)
            {
                Nickel nickel = new Nickel();
                Quarter quarter = new Quarter();
                _register.Add(nickel);
                _register.Add(quarter);
            }
            for (int i = 0; i < 10; i++)
            {
                Dime dime = new Dime();
                _register.Add(dime);
            } 
        }
        //A method to fill the sodamachines inventory with soda can objects.
        public void FillInventory()
        {
            RootBeer rootBeer = new RootBeer();
            Cola cola = new Cola();
            OrangeSoda orangeSoda = new OrangeSoda();

            for (int i = 0; i < 20; i++)
            {
                _inventory.Add(rootBeer);
                _inventory.Add(cola);
                _inventory.Add(orangeSoda);
            }            
        }
        //Method to be called to start a transaction.
        //Takes in a customer which can be passed freely to which ever method needs it.
        public void BeginTransaction(Customer customer)
        {
            bool willProceed = UserInterface.DisplayWelcomeInstructions(_inventory);
            if (willProceed)
            {
                Transaction(customer);
            }
        }
        
        //This is the main transaction logic think of it like "runGame".  This is where the user will be prompted for the desired soda.
        //grab the desired soda from the inventory.
        //get payment from the user.
        //pass payment to the calculate transaction method to finish up the transaction based on the results.
        private void Transaction(Customer customer)
        {
            string userSodaChoice = UserInterface.SodaSelection(_inventory);
            CalculateTransaction(customer.GatherCoinsFromWallet(GetSodaFromInventory(userSodaChoice)), GetSodaFromInventory(userSodaChoice), customer);
            
        }
        //Gets a soda from the inventory based on the name of the soda.
        private Can GetSodaFromInventory(string nameOfSoda)
        {
            foreach (Can can in _inventory)
            {
                if (can.Name == nameOfSoda)
                {
                    return can;                    
                }
            }
            return null;
        }

        //This is the main method for calculating the result of the transaction.
        //It takes in the payment from the customer, the soda object they selected, and the customer who is purchasing the soda.
        //This is the method that will determine the following:
        //If the payment is greater than the price of the soda, and if the sodamachine has enough change to return: Dispense soda, and change to the customer.
        //If the payment is greater than the cost of the soda, but the machine does not have ample change: Dispense payment back to the customer.
        //If the payment is exact to the cost of the soda:  Dispense soda.
        //If the payment does not meet the cost of the soda: dispense payment back to the customer.
        private void CalculateTransaction(List<Coin> payment, Can chosenSoda, Customer customer)
        {
            UserInterface.DisplayCost(chosenSoda);
            double change = DetermineChange(TotalCoinValue(payment), chosenSoda.Price);
            GatherChange(change);
            if (TotalCoinValue(payment) > chosenSoda.Price)
            {

                if (GatherChange(change) != null)
                {
                    GetSodaFromInventory(chosenSoda.Name);
                    customer.AddCanToBackpack(chosenSoda);
                    customer.AddCoinsIntoWallet(GatherChange(change));
                    UserInterface.EndMessage(chosenSoda.Name, change);
                }
                else
                {
                    Console.WriteLine("Not enough change in register returning your payment");
                    customer.AddCoinsIntoWallet(payment);
                }

            }
            else if (TotalCoinValue(payment) == chosenSoda.Price)
            {
                GetSodaFromInventory(chosenSoda.Name);
                customer.AddCanToBackpack(chosenSoda);
                UserInterface.EndMessage(chosenSoda.Name, change);
            }
            else if (TotalCoinValue(payment) < chosenSoda.Price)
            {
                Console.WriteLine("You have entered insufficent change now returning your payment");
                customer.AddCoinsIntoWallet(payment);
            }
        }
        //Takes in the value of the amount of change needed.
        //Attempts to gather all the required coins from the sodamachine's register to make change.
        //Returns the list of coins as change to despense.
        //If the change cannot be made, return null.
        private List<Coin> GatherChange(double changeValue)
        {
            List<Coin> totalChangeDue = new List<Coin>();
            Coin coinFound = null;
            while (changeValue > 0)
            {
                if (changeValue >= .25)
                {
                    if (RegisterHasCoin("Quarter"))
                    {
                        coinFound = GetCoinFromRegister("Quarter");
                        totalChangeDue.Add(coinFound);
                        changeValue -= coinFound.Value;
                    }
                }
                else if (changeValue >= .10)
                {
                    if (RegisterHasCoin("Dime"))
                    {
                        coinFound = GetCoinFromRegister("Dime");
                        totalChangeDue.Add(coinFound);
                        changeValue -= coinFound.Value;
                    }
                }
                else if (changeValue >= .05)
                {
                    if (RegisterHasCoin("Nickle"))
                    {
                        coinFound = GetCoinFromRegister("Nickle");
                        totalChangeDue.Add(coinFound);
                        changeValue -= coinFound.Value;
                    }
                }
                else if (changeValue >= .01)
                {
                    if (RegisterHasCoin("Penny"))
                    {
                        coinFound = GetCoinFromRegister("Penny");
                        totalChangeDue.Add(coinFound);
                        changeValue -= coinFound.Value;
                    }
                }
                return totalChangeDue;
            }
            return null;
        }
        //Reusable method to check if the register has a coin of that name.
        //If it does have one, return true.  Else, false.
        private bool RegisterHasCoin(string name)
        {
            bool coinFound = false;

            foreach(Coin coin in _register)
            {
                if(coin.Name == name)
                {
                    coinFound = true;
                    break;
                }
            }
            return coinFound;
   
        }
        //Reusable method to return a coin from the register.
        //Returns null if no coin can be found of that name.
        private Coin GetCoinFromRegister(string name)
        {
            foreach (Coin coin in _register)
            {
                if (coin.Name == name)
                {
                    return coin;
                }
            }
            return null;
        }
        //Takes in the total payment amount and the price of can to return the change amount.
        private double DetermineChange(double totalPayment, double canPrice)
        {
            double changeDue = totalPayment - canPrice;
            Math.Round(changeDue);
            return changeDue;
        }
        //Takes in a list of coins to return the total value of the coins as a double.
        private double TotalCoinValue(List<Coin> payment)
        {
            double coinTotal = 0;
            foreach (Coin coin in payment)
            {
                coinTotal += coin.Value;
            }
            return coinTotal;
            
        }
        //Puts a list of coins into the soda machines register.
        private void DepositCoinsIntoRegister(List<Coin> coins)
        {
            _register.AddRange(coins);
        }
    }
}
