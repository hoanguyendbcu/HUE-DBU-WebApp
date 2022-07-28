using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBCU_WebApp.Models
{
    public class NewsBase
    {
        [Key]
        public int NewsId { set; get; }

        [Required(ErrorMessage = "Must be title news")]
        [Display(Name = "Title Vietnamese")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "{0} length {1} to {2}")]
        public string Title { set; get; }

        [Required(ErrorMessage = "Must be title news English")]
        [Display(Name = "Title English")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "{0} length {1} to {2}")]
        public string TitleEn { set; get; }

        [Display(Name = "Description Vietnamese")]
        public string Description { set; get; }

        [Display(Name = "Description English")]
        public string DescriptionEn { set; get; }

        //[Required(ErrorMessage = "Please choose image")]
        [Display(Name = "Picture")]
        public string StrUrlImage { set; get; }

        [Display(Name = "Identity string (url) Vietnamese", Prompt = "Enter or create with Title")]
        [Required(ErrorMessage = "Must be enter string URL TV")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} length {1} to {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Only user character [a-z0-9-]")]
        public string Slug { set; get; }

        [Display(Name = "Identity string (url) English", Prompt = "Enter or create with Title")]
        [Required(ErrorMessage = "Must be enter string URL TA")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} length {1} to {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "COnly user character [a-z0-9-]")]
        public string SlugEn { set; get; }


        [Display(Name = "Content Vietnamese")]
        public string Content { set; get; }

        [Display(Name = "Content English")]
        public string ContentEn { set; get; }

        [Display(Name = "Author")]

        [StringLength(160)]
        public string Author { set; get; }

        [Display(Name = "Published")]

        public bool Published { set; get; }

        public List<NewsCategory> NewsCategories { get; set; }

    }
}