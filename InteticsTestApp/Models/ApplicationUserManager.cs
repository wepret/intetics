using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Threading.Tasks;

public class ApplicationUserManager : UserManager<ApplicationUser>
{
    public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
    {
    }
    public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
                                            IOwinContext context)
    {
        ApplicationContext db = context.Get<ApplicationContext>();
        ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        return manager;
    }




    //public override async Task<ApplicationUser> FindAsync(string email, string password)
    //{
    //    var user = await FindByIdAsync(email).WithCurrentCulture();
    //    if (user == null)
    //    {
    //        return null;
    //    }
    //    return await CheckPasswordAsync(user, password).WithCurrentCulture() ? user : null;
    //}

}