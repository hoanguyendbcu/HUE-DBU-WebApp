using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBCU_WebApp.Models
{
    public class Gazetteer
    {
        [Key]
        [StringLength(50)]
        public string Gazetteer_guid { set; get; }


        [Display(Name = "gazetteer localid")]
        [StringLength(50)]
        public string Gazetteer_localid { set; get; }

        [Display(Name = "Parentgazetteer_guid")]
        [StringLength(50)]
        public string Parentgazetteer_guid { set; get; }

        [Display(Name = "Gazetteer Name")]
        [StringLength(50)]
        public string Gazetteername { set; get; }

        [Display(Name = "Gazetteer Name Eng")]
        [StringLength(50)]
        public string Gazetteername_eng { set; get; }
        public float? Long { set; get; }
        public float? Lat { set; get; }



        [Display(Name = "Parentgazetteer")]
        [ForeignKey("Parentgazetteer_guid")]
        public Gazetteer ParentGazetteer { set; get; }
    }
}
