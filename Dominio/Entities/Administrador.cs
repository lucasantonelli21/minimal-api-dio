using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace minimals_api.Dominio.Entities
{
    public class Administrador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
         [StringLength(50)]
        public string Password { get; set; }
         [StringLength(10)]
        public string Perfil {get; set; }

        public Administrador(String Email, String Password,String Perfil) 
        {
            this.Email = Email;
            this.Password = Password;
            this.Perfil = Perfil;
        }
    }
}