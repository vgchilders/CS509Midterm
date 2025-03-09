using Microsoft.EntityFrameworkCore;

class Con{
    //connection string
    public const string str =  "";
}

class AccountData{
    public int accountID {get;}
    public string login {get; set;}
    public int pin {get; set;}
    public string holder {get; set;}
    public int balance {get; set;}
    public int status {get;}
}

internal class MyContext : DbContext{
    public DbSet<AccountData> account {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(Con.str);
    }
}