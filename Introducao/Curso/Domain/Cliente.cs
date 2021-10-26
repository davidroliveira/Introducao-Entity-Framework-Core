using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoEFCore.Domain
{
    [Table("Cliente")]//Nome da Tabela no banco
    public class Cliente
    {
        [Key]//Chave
        public int Id { get; set; }
        [Required]//Requerido
        public string Nome { get; set; }
        [Column("Phone")]//Nome da propriedade não é o mesmo nome da coluna no banco
        public string Telefone { get; set; }
        public string CEP { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
    }
}