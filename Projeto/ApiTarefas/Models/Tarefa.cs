using System.ComponentModel.DataAnnotations;

namespace TarefasApi.Models; // Namespace para os modelos

public class Tarefa
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O título é obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 100 caracteres")]
    public required string Titulo { get; set; }

    [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres")]
    public string? Descricao { get; set; }

    public bool Concluida { get; set; }

    public DateTime DataCriacao { get; set; }

    /// <summary>
    /// Centraliza a lógica de validação da tarefa.
    /// </summary>
    /// <returns>Uma lista de strings contendo as mensagens de erro.</returns>
    public List<string> Validar()
    {
        var erros = new List<string>();

        if (string.IsNullOrWhiteSpace(Titulo))
        {
            erros.Add("O título é obrigatório.");
        }
        else if (Titulo.Length < 3 || Titulo.Length > 100)
        {
            erros.Add("O título deve ter entre 3 e 100 caracteres.");
        }

        if (!string.IsNullOrEmpty(Descricao) && Descricao.Length > 500)
        {
            erros.Add("A descrição não pode exceder 500 caracteres.");
        }

        return erros;
    }
}