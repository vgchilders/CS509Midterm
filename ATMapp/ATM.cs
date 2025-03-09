using Ninject;
using Ninject.Activation.Strategies;
using System;

class ATM{
    private readonly IKernel kernel;
    private readonly IDatabase database;
    
    public ATM()
    {
        kernel = new StandardKernel(new BuildModule());
        database = kernel.Get<IDatabase>();
    }

    public void Start(){
        Console.WriteLine("Welcome to the ATM!");
        string login;
        int idNum = 0;
        while(true){ //login loop
            Console.Write("Enter Login: ");
            login = Console.ReadLine();

            int pin = 0;
            Console.Write("Enter PIN: ");
            if(!Int32.TryParse(Console.ReadLine(), out pin)){ //make sure user entered good num
                Console.WriteLine("Invalid PIN (not an int)");
                continue;
            }

            idNum = database.ValidateLogin(login, pin); //grabbing ID
            if (idNum != 0)
            {
                //valid login so exit login loop
                break;
            }
            else
            {
                Console.WriteLine("Invalid credentials");
                continue;
            }
        }
        if(String.Equals(login, "admin123")){ //ONLY 1 ADMIN account and it will be this case
            var adminAccount = kernel.Get<IAdminAccount>();
            AdminSession(adminAccount);
        }else{
            var userAccount = kernel.Get<IUserAccount>(new Ninject.Parameters.ConstructorArgument("idNum", idNum));
            UserSession(userAccount);
        }
        Console.WriteLine("-----Ending Session-----");
        Console.WriteLine("Have a Good Day!");
    }

    public void UserSession(IUserAccount userAccount){
        while (true)
        {
            Console.WriteLine("1----Withdraw Cash\n3----Deposit Cash\n4----Display Balance\n5----Exit");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter the withdrawal amount: ");
                    int withdrawalAmount = 0;
                    if(!Int32.TryParse(Console.ReadLine(), out withdrawalAmount)){
                        Console.WriteLine("Invalid withdrawal number");
                        break;
                    }
                    userAccount.Withdraw(withdrawalAmount);
                    break;
                case "3":
                    Console.Write("Enter the cash amount to deposit: ");
                    int depositAmount = 0;
                    if(!Int32.TryParse(Console.ReadLine(), out depositAmount)){
                        Console.WriteLine("Invalid deposit number");
                        break;
                    }
                    userAccount.Deposit(depositAmount);
                    break;
                case "4":
                    userAccount.CheckBalance();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Selected option is invalid");
                    break;
            }
        }
    }

    public void AdminSession(IAdminAccount adminAccount){
        while (true)
        {
            Console.WriteLine("1----Create New Account\n2----Delete Existing Account\n3----Update Account Information\n4----Search For Account\n6----Exit");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("-----Making New Account-----");
                    Console.Write("Enter the Login: ");
                    string login = Console.ReadLine();

                    int pin = 0;
                    Console.Write("Enter the 5-digit PIN: ");
                    if(!Int32.TryParse(Console.ReadLine(), out pin)){
                        Console.WriteLine("Invalid pin number (non-integer)");
                        break;
                    }

                    Console.Write("Enter the holders name: ");
                    string holder = Console.ReadLine();

                    int startBalance = 0;
                    Console.Write("Enter the starting balance: ");
                    if(!Int32.TryParse(Console.ReadLine(), out startBalance)){
                        Console.WriteLine("Invalid balance number (non-integer)");
                        break;
                    }

                    Console.Write("Enter the account status(enable/disable): ");
                    string status = Console.ReadLine();

                    adminAccount.CreateAccount(login, pin, holder, startBalance, status);
                    break;
                case "2":
                    Console.Write("Enter the account number: ");
                    int accountNum = 0;
                    if(!Int32.TryParse(Console.ReadLine(), out accountNum)){
                        Console.WriteLine("Invalid account number (non-integer)");
                        break;
                    }
                    Console.WriteLine($"Below is the info for account # {accountNum}");
                    adminAccount.SearchAccount(accountNum);

                    Console.Write("To continue deletion, please re-enter the account number: ");
                    if(!Int32.TryParse(Console.ReadLine(), out accountNum)){
                        Console.WriteLine("Cancelling Deletion");
                        break;
                    }
                    adminAccount.DeleteAccount(accountNum);
                    break;
                case "3":
                    Console.WriteLine("-----Updating Account-----");
                    int newAccountNum = 0;
                    Console.Write("Enter the Account Number: ");
                    if(!Int32.TryParse(Console.ReadLine(), out newAccountNum)){
                        Console.WriteLine("Invalid account number (non-integer)");
                        break;
                    }

                    Console.Write("Enter the new holder name: ");
                    string newHolder = Console.ReadLine();

                    Console.Write("Enter the new account status(enable/disable): ");
                    string newStatus = Console.ReadLine();

                    Console.Write("Enter the new login: ");
                    string newLogin = Console.ReadLine();

                    int newPin = 0;
                    Console.Write("Enter the new 5-digit PIN: ");
                    if(!Int32.TryParse(Console.ReadLine(), out pin)){
                        Console.WriteLine("Invalid pin number (non-integer)");
                        break;
                    }

                    adminAccount.UpdateAccount(newAccountNum, newLogin, newPin, newHolder, newStatus);
                    break;
                case "4":
                    int searchAccountID = 0;
                    Console.Write("Enter Account Number: ");
                    if(!Int32.TryParse(Console.ReadLine(), out searchAccountID)){
                        Console.WriteLine("Invalid account number (non-integer)");
                        break;
                    }
                    adminAccount.SearchAccount(searchAccountID);
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Selected option is invalid");
                    break;
            }
        }
    }
}
