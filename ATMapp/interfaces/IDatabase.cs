public interface IDatabase{
    int ValidateLogin(string login, int pin);
    void Withdraw(int accountId, int cashAmount);
    void Deposit(int accountId, int cashAmount);
    int GetBalance(int accountId);
    void AddAccount(string login, int pin, string holder, int startBalance, int status);
    void DeleteAccount(int accountId);
    void UpdateAccount(int accountId, string newLogin, int newPin, string newHolder, int newStatus);
    void PrintAccount(int accountId);
}