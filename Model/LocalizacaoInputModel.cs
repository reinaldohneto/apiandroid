namespace AppAndroid.Model;

public class LocalizacaoInputModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string Base64 { get; set; }
}