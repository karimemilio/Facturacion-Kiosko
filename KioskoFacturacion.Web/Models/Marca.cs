using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KioskoFacturacion.Web.Models
{
    public class Marca
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Nombre { get; set; }

        public string Estado { get; set; }
        public Rubro Rubro { get; set; }
        [Required]

        //[ForeignKey("RubroID")]
        public int RubroID { get; set; }

    }
}