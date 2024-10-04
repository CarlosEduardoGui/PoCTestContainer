namespace PoCTestContainer.API.Models;

public class Usuario
{
    public Usuario()
    {
        
    }
    public Usuario(string nome, string sobrenome)
    {
        Nome = nome;
        Sobrenome = sobrenome;
        CriadoEm = DateTime.Now;
    }

    public int Id { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }

    public void MudarNomeESobrenome(string nome, string sobreNome)
    {
        Nome = nome;
        Sobrenome = sobreNome;
        AtualizadoEm = DateTime.Now;
    }
}
