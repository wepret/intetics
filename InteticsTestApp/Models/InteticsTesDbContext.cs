using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

public class ApplicationContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationContext() : base("DB_A2C173_InteticsTestDb") { }

    public static ApplicationContext Create()
    {
        return new ApplicationContext();
    }
}