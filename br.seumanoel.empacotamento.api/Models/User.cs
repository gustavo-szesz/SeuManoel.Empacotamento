using System.ComponentModel.DataAnnotations;

namespace br.seumanoel.empacotamento.api.Models
{
    /// <summary>
    /// Classe responsável por representar um usuário no sistema.
    /// </summary>
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O username é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O username não pode exceder 100 caracteres.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MaxLength(200)]
        public string Password { get; set; }
    }
}
