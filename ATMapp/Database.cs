using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using System;

class Con
{
    // Replace with real MySQL connection string
    public const string str = "server=localhost;database=atmapp;user=root;password=vgchilders";
}

public class AccountData
{
    public int AccountID { get; set; } // Primary Key
    public string Login { get; set; }
    public int Pin { get; set; }
    public string Holder { get; set; }
    public int Balance { get; set; } // proj doesnt call for cents
    public int Status { get; set; }
}

public class MyContext : DbContext
{
    public DbSet<AccountData> Account { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(Con.str);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountData>()
            .HasKey(a => a.AccountID); // Set primary key
    }
}

// Main program to test database connection
class Database : IDatabase
{
    
    public Database(){
        using (var db = new MyContext())
        {
            db.Database.EnsureCreated(); // Creates the database if it doesn’t exist
            //Console.WriteLine("Database connected");
        }
    }

    //Checks given login information, returns accountID num if found otherwise returns 0
    public int ValidateLogin(string login, int pin){
        using(var db = new MyContext()){
            var account = db.Account.FirstOrDefault(a => a.Login == login);
            if (account != null)
            {
                if(account.Pin == pin){
                    Console.WriteLine($"Welcome {account.Holder}");
                    return account.AccountID;
                }else{
                    Console.WriteLine($"Pin {pin} is incorrect");
                    return 0;
                }
            }else{
                Console.WriteLine($"Login {login} not found");
                return 0;
            }

        }
    }

    //----- USER DATABASE FUNCTIONALITY ----
    //Withdraw given amount into account of given ID
    public void Withdraw(int accountId, int cashAmount){
        using(var db = new MyContext()){
            var account = db.Account.Find(accountId);
            if (account != null)
            {
                account.Balance = account.Balance - cashAmount;
                db.SaveChanges();

                DateTime today = DateTime.Now;
                string formattedDate = today.ToString("MM/dd/yyyy");

                Console.WriteLine("Cash Succesfully Withdrawn");
                Console.WriteLine($"Account #{accountId}");
                Console.WriteLine("Date: " + formattedDate);
                Console.WriteLine($"Withdrawn: {cashAmount}");
                Console.WriteLine($"Balance: {account.Balance}");
            }else{
                Console.WriteLine($"Account {accountId} not found");
            }
        }
    }

    //Deposit given amount into account of given ID
    public void Deposit(int accountId, int cashAmount){
        using(var db = new MyContext()){
            var account = db.Account.Find(accountId);
            if (account != null)
            {
                account.Balance = account.Balance + cashAmount;
                db.SaveChanges();

                DateTime today = DateTime.Now;
                string formattedDate = today.ToString("MM/dd/yyyy");

                Console.WriteLine("Cash Deposited Succesfully");
                Console.WriteLine($"Account #{accountId}");
                Console.WriteLine("Date: " + formattedDate);
                Console.WriteLine($"Deposited: {cashAmount}");
                Console.WriteLine($"Balance: {account.Balance}");
            }else{
                Console.WriteLine($"Account {accountId} not found");
            }
        }
    }

    //Grab balance of account with given ID
    public int GetBalance(int accountId){
        using(var db = new MyContext()){
            var account = db.Account.Find(accountId);
            if (account != null)
            {
                DateTime today = DateTime.Now;
                string formattedDate = today.ToString("MM/dd/yyyy");

                Console.WriteLine($"Account #{accountId}");
                Console.WriteLine("Date: " + formattedDate);
                Console.WriteLine($"Balance: {account.Balance}");
                return account.Balance;
            }else{
                Console.WriteLine($"Account {accountId} not found");
                return -1;
            }
        }
    }

    //----- ADMIN DATABASE FUNCTIONALITY -----
    //Adds account with given info
    public void AddAccount(string login, int pin, string holder, int startBalance, int status){
        using (var db = new MyContext())
        {
            // Creating a new account
            var newAccount = new AccountData
            {
                Login = login,
                Pin = pin,
                Holder = holder,
                Balance = startBalance,
                Status = status
            };

            // Add and save to the database
            db.Account.Add(newAccount);
            db.SaveChanges();

            Console.WriteLine("New account added");
        }
    }

    //delete account with given id
    public void DeleteAccount(int accountId)
    {
        using (var db = new MyContext())
        {
            var account = db.Account.Find(accountId);

            if (account != null)
            {
                db.Account.Remove(account);
                db.SaveChanges();
                Console.WriteLine($"Account {accountId} deleted");
            }
            else
            {
                Console.WriteLine($"Account {accountId} not found");
            }
        }
    }

    //update account with given id
    public void UpdateAccount(int accountId, string newLogin, int newPin, string newHolder, int newStatus)
    {
        using (var db = new MyContext())
        {
            var account = db.Account.Find(accountId);

            if (account != null)
            {
                account.Login = newLogin;
                account.Pin = newPin;
                account.Holder = newHolder;
                account.Status = newStatus;

                db.SaveChanges();
                Console.WriteLine($"Account {accountId} updated");
            }
            else
            {
                Console.WriteLine($"Account {accountId} not found");
            }
        }
    }

    //Prints out account details of given ID to console
    public void PrintAccount(int accountId)
    {
        using (var db = new MyContext())
        {
            var account = db.Account.Find(accountId);

            if (account != null)
            {
                Console.WriteLine("The account information is: ");
                Console.WriteLine($"Account # {accountId}");
                Console.WriteLine($"Holder: {account.Holder}");
                Console.WriteLine($"Balance: {account.Balance}");
                if(account.Status == 0){
                    Console.WriteLine($"Status: Disabled");
                }else{
                    Console.WriteLine($"Status: Enabled");
                }
                Console.WriteLine($"Login: {account.Login}");
                Console.WriteLine($"Pin Code: {account.Pin}");
            }
            else
            {
                Console.WriteLine($"Account {accountId} not found");
            }
        }
    }
}