using System;
using System.ComponentModel.DataAnnotations;

namespace DBCU_WebApp.Models
{
    public class OperationPlanWeek
    {
        [Key]
        public int OperationPlanWeekID { get; set; }
        public int Week { get; set; }
        public int Year { get; set; }

        [Display(Name = "From Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FromDate { set; get; }


        [Display(Name = "To Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime ToDate { set; get; }
    }
}
