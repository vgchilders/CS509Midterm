class UserAccount : IUserAccount{
    private readonly IDatabase database;
    private readonly int ID;

    public UserAccount(IDatabase db, int idNum){
        database = db;
        ID = idNum;
    }

    public void Withdraw(int amount){
        int currentBalance = database.GetBalance(ID);
        if(currentBalance >= amount){ //make sure we have enough money
            database.Withdraw(ID, amount);
        }else{
            Console.WriteLine("Not enough funds in account");
        }
    }

    public void Deposit(int amount){
        database.Deposit(ID, amount);
    }

    public void CheckBalance(){
        database.GetBalance(ID);
    }

}