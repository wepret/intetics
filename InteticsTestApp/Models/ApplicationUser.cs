using Microsoft.AspNet.Identity.EntityFramework;

public class ApplicationUser : IdentityUser
{
    public string ProfileName { get; set; }

    public ApplicationUser()
    {
    }
}