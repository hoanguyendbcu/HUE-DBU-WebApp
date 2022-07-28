using System.ComponentModel.DataAnnotations.Schema;

namespace DBCU_WebApp.Models
{
    public class NewsCategory
    {

        public int NewsID { set; get; }

        public int CategoryID { set; get; }


        [ForeignKey("PostID")]
        public News News { set; get; }

        [ForeignKey("CategoryID")]
        public Category Category { set; get; }

    }
}