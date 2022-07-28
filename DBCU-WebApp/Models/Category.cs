using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBCU_WebApp.Models
{

    [Table("Category")] // Model  tương ứng với bảng Category
    public class Category
    {

        [Key]
        public int Id { get; set; }

        // Category cha (FKey)
        [Display(Name = "List parent")]
        public int? ParentId { get; set; }

        // Tiều đề Category
        [Required(ErrorMessage = "Must be name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [Display(Name = "Name")]
        public string Title { get; set; }

        // Nội dung, thông tin chi tiết về Category
        [DataType(DataType.Text)]
        [Display(Name = "Content")]
        public string Content { set; get; }

        //chuỗi Url
        [Required(ErrorMessage = "Must be create url")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} length {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Only user charater [a-z0-9-]")]
        [Display(Name = "Url views")]
        public string Slug { set; get; }

        // Các Category con
        public ICollection<Category> CategoryChildren { get; set; }

        [Display(Name = "Parent")]
        [ForeignKey("ParentId")]
        public Category ParentCategory { set; get; }

        public List<NewsCategory> NewsCategories { get; set; }

        public List<Category> ListParents()
        {
            List<Category> li = new List<Category>();
            var parent = this.ParentCategory;
            while (parent != null)
            {
                li.Add(parent);
                parent = parent.ParentCategory;
            }

            li.Reverse();
            return li;
        }


        public static Category Find(ICollection<Category> lis, int CategoryId)
        {
            foreach (var c in lis)
            {
                if (c.Id == CategoryId) return c;
                if (c.CategoryChildren != null)
                {
                    var c_in_child = Find(c.CategoryChildren, CategoryId);

                    if (c_in_child != null)
                        return c_in_child;
                }
            }
            return null;
        }

        public List<int> ChildCategoryIDs(ICollection<Category> childcates = null, List<int> lists = null)
        {
            if (lists == null)
                lists = new List<int>();


            if (childcates == null)
                childcates = CategoryChildren;

            if (childcates == null)
                return lists;

            foreach (var item in childcates)
            {
                lists.Add(item.Id);
                ChildCategoryIDs(item.CategoryChildren, lists);
            }

            return lists;
        }

    }
}