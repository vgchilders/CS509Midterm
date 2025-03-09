using System;
class AdminAccount : IAdminAccount{
    private readonly IDatabase database;

    public AdminAccount(IDatabase db){
        database = db;
    }

    public void CreateAccount(string login, int pin, string holder, int startBalance, string status){
        int statusInt;
        if(String.Equals(status, "enable")){
            statusInt = 1;
        }else{
            statusInt = 0;
        }

        if(pin > 99999 || pin <= 9999){
            Console.WriteLine("Invalid pin number (5 digit only)");
            return;
        }

        database.AddAccount(login, pin, holder, startBalance, statusInt);
    }

    public void DeleteAccount(int accountId){
        if(accountId == 1){
            Console.WriteLine("Cannot delete Admin account!");
            return;
        }
        database.DeleteAccount(accountId);
    }

    public void UpdateAccount(int accountId, string newLogin, int newPin, string newHolder, string newStatus){
        int statusInt;
        if(String.Equals(newStatus, "enable")){
            statusInt = 1;
        }else{
            statusInt = 0;
        }
        database.UpdateAccount(accountId, newLogin, newPin, newHolder, statusInt);
    }

    public void SearchAccount(int accountId){
        database.PrintAccount(accountId);
    }
}