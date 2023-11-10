using Microsoft.AspNetCore.Mvc.Rendering;
using News.Areas.Admin.Models;

namespace News.Areas.Admin.ViewModels
{
    public class NewsViewModels
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        //public string ImageName { get; set; }
        public long CountView { get; set; }
        public DateTime CreationDate { get; set; }
        public long CategoryId { get; set; }
        public string Category { get; set; }
        public string SelectedCategory { get; set; }
        public List<SelectListItem> NewsCategory { get; set; }
    }
}
