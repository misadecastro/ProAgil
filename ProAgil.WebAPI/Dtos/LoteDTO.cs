using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class LoteDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage="{0} obrigatório")]
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
        [Range(2,120000, ErrorMessage="{0} deve ser entre 2 e 120000")]
        public int Quantidade { get; set; }
    }
}