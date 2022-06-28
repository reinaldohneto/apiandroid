using Microsoft.AspNetCore.Identity;

namespace AppAndroid.Model;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<Localizacao> Localizacoes { get; set; }
}