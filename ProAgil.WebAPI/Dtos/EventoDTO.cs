using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class EventoDTO
    {
        public int Id {get; set;}    
        
        [Required(ErrorMessage="{0} obrigatório")]
        [StringLength(100,MinimumLength=3,ErrorMessage="{0} deve ter entre 3 e 100 Caracteres")]
        public string Local {get; set;}    
        public string DataEvento {get; set;}
        
        [Required(ErrorMessage="{0} obrigatório")]    
        public string Tema {get; set;}    
        
        [Range(2,120000,ErrorMessage="{0} de pessoas deve ser entre 2 e 120000")]
        public int QtdPessoas {get; set;}    
        public string ImagemURL {get; set;}
        public string Telefone {get; set;}
        
        [EmailAddress]
        public string Email {get; set;}
        public List<LoteDTO> Lotes {get; set;}
        public List<RedeSocialDTO> RedesSociais { get; set; }
        public List<PalestranteDTO> Palestrantes { get; set; }
    }
}