namespace AppAndroid.Model;

public class Grupo
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public ICollection<ApplicationUser>? Usuarios { get; set; }

    public Grupo()
    {
        Id = Guid.NewGuid();
    }
}