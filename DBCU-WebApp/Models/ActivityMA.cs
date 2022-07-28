using System.ComponentModel.DataAnnotations;

namespace DBCU_WebApp.Models
{
    public class ActivityMA
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string ActivityCode { get; set; }
        // 
        [Required(ErrorMessage = "Must be name activity")]
        [StringLength(250)]
        [Display(Name = "Name activity")]
        public string ActivityName { get; set; }

        // 
        [Required(ErrorMessage = "Must be name activity english")]
        [StringLength(250)]
        [Display(Name = "Name activity english")]
        public string ActivityNameEN { get; set; }

    }
}
