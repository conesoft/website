using Conesoft.Hosting;

class UserTokenStorage(HostEnvironment environment)
{
    public Conesoft.Files.Directory Directory => environment.Global.Storage / "users";
    public Conesoft.Files.Directory For(string User) => Directory / User / "tokens";
}
