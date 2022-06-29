using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace AppAndroid.Model;

public class Localizacao
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string Base64 { get; set; }
    public Guid UserId { get; set; }
    [JsonIgnore]
    public virtual ApplicationUser Usuario { get; }

    public void AtualizarLocalizacao(Localizacao localizacao)
    {
        Titulo = localizacao.Titulo;
        Latitude = localizacao.Latitude;
        Longitude = localizacao.Longitude;
        Base64 = localizacao.Base64;
        Descricao = localizacao.Descricao;
    }

    public void AtribuirUsuarioId(Guid usuarioId)
    {
        UserId = usuarioId;
    }

    public Localizacao()
    {
        Id = Guid.NewGuid();
    }
}