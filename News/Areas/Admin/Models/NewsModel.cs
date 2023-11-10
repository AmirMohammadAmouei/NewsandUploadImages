using Microsoft.AspNetCore.Mvc.Rendering;
using News.Areas.Admin.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace News.Models
{
    public class NewsModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public long? CountView { get; set; }
        public DateTime CreationDate { get; set; }
        public long CategoryId { get; set; }
        public NewsCategory Category { get; set; }
        [NotMapped]
        public List<SelectListItem> NewsCategory { get; set; }

    }
}
