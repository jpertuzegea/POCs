using System.ComponentModel.DataAnnotations;

namespace POC_BorradoMasivo.Entity
{
    public class Departament
    {
        [Key]
        public int? DepartamentId { get; set; }
        public string Name { get; set; }
        public byte State { get; set; }
    }
}
