using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBCU_WebApp.Models
{
    [Table("OperationPlans")] // Model OperationPlan
    public class OperationPlans
    {
        [Key]
        public int Id { get; set; }

        public int Id1 { get; set; }
        public int Week { get; set; }
        public int Org { get; set; }
        public int Team { get; set; }
        public int Activity { get; set; }

        [Display(Name = "Province")]
        [StringLength(50)]
        public string ProvinceID { get; set; }

        [Display(Name = "District")]
        [StringLength(50)]
        public string DistictID { get; set; }

        [Display(Name = "Commune")]
        [StringLength(50)]
        public string CommuneID { get; set; }

        [Display(Name = "Village")]
        [StringLength(50)]
        public string VillageID { get; set; }

        [StringLength(250)]
        public string TaskID { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int? Areas { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? StartDate { set; get; }

        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? EndDate { set; get; }

        [DataType(DataType.Text)]
        [Display(Name = "Comment")]

        public string Comment { set; get; }


        [Display(Name = "User Created")]
        [StringLength(250)]
        public string UserCreated { set; get; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { set; get; }

        [Display(Name = "Date Updated")]
        public DateTime? DateUpdated { set; get; }


        [ForeignKey("ProvinceID")]
        public Gazetteer Province { set; get; }

        [ForeignKey("DistictID")]
        public Gazetteer District { set; get; }

        [ForeignKey("CommuneID")]
        public Gazetteer Commune { set; get; }

        [ForeignKey("VillageID")]
        public Gazetteer Village { set; get; }

        [ForeignKey("Week")]
        public OperationPlanWeek OperationPlanWeek { set; get; }

        [ForeignKey("Org")]
        public Organization Organization { set; get; }

        [ForeignKey("Team")]
        public Organization Teams { set; get; }

        [ForeignKey("Activity")]
        public ActivityMA ActivityMA { set; get; }
    }
}
