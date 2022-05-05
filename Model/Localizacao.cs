namespace AppAndroid.Model;

public class Localizacao
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string Base64 { get; set; }

    public void AtualizarLocalizacao(Localizacao localizacao)
    {
        Id = localizacao.Id;
        Titulo = localizacao.Titulo;
        Latitude = localizacao.Latitude;
        Longitude = localizacao.Longitude;
        Base64 = localizacao.Base64;
    }

    public Localizacao()
    {
        
    }
}