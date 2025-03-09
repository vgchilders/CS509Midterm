using Ninject;

public class BuildModule : Ninject.Modules.NinjectModule
{
    public override void Load()
    {
        //Bind<IUserAccount>().To<UserAccount>().WithConstructorArgument("initialBalance", 1000m);
        //Bind<IDatabase>().To<Database>();
    }
}