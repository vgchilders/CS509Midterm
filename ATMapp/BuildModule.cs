using Ninject;

public class BuildModule : Ninject.Modules.NinjectModule
{
    
    public override void Load()
    {
        // Bind IDatabase to Database
        Bind<IDatabase>().To<Database>().InSingletonScope();

        // Bind IUserAccount to UserAccount
        Bind<IUserAccount>().To<UserAccount>()
            .WithConstructorArgument("database", ctx => ctx.Kernel.Get<IDatabase>());
            //.WithConstructorArgument("ID", ""); // ID will be set at runtime

        // Bind IAdminAccount to AdminAccount
        Bind<IAdminAccount>().To<AdminAccount>()
            .WithConstructorArgument("database", ctx => ctx.Kernel.Get<IDatabase>());
    }
}