using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBCU_WebApp.Models
{
    [Table("News")]
    public class News : NewsBase
    {

        [Required]
        [Display(Name = "Author")]
        public string AuthorId { set; get; }

        [ForeignKey("AuthorId")]


        [Display(Name = "Date Created")]
        public DateTime DateCreated { set; get; }

        [Display(Name = "Date Updated")]
        public DateTime DateUpdated { set; get; }
    }
}