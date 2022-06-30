using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace AppAndroid.Model;

public class ApplicationUser : IdentityUser
{
    [JsonIgnore]
    public virtual ICollection<Localizacao> Localizacoes { get; set; }
    [JsonIgnore]
    public virtual Grupo Grupo { get; set; }

    public Guid GrupoId { get; set; }
}