using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBCU_WebApp.Models
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }

        public string OrganizationCode { get; set; }

        // activity cha (FKey)
        [Display(Name = "List Parent")]
        public int? ParentId { get; set; }

        // 
        [Required(ErrorMessage = "Must be name Organization")]
        [StringLength(250)]
        [Display(Name = "Name Organization - Vietnamese")]
        public string OrganizationName { get; set; }

        // 
        [Required(ErrorMessage = "Must be name Organization English")]
        [StringLength(250)]
        [Display(Name = "Name Organization - English")]
        public string OganizationNameEN { get; set; }

        // Các con
        public ICollection<Organization> OrganizationChildren { get; set; }

        [Display(Name = "Parent")]
        [ForeignKey("ParentId")]
        public Organization ParentOrganization { set; get; }

    }
}
