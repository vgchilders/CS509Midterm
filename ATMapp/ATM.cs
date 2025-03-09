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
            var userAccount = kernel.Get<IUserAccount>(new Ninject.Parameters.ConstructorArgument("ID", idNum));
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
                    Console.Write("Enter the account number : ");
                    int depositAmount = 0;
                    if(!Int32.TryParse(Console.ReadLine(), out depositAmount)){
                        Console.WriteLine("Invalid deposit number");
                        break;
                    }
                    userAccount.Deposit(depositAmount);
                    break;
                case "3":
                    userAccount.CheckBalance();
                    break;
                case "4":
                    userAccount.CheckBalance();
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
