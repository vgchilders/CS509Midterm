public interface IAdminAccount{
    void CreateAccount(string login, int pin, string holder, int startBalance, string status);
    void DeleteAccount(int accountId);
    void UpdateAccount(int accountId, string newLogin, int newPin, string newHolder, string newStatus);
    void SearchAccount(int accountId);
}