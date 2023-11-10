using News.Models;

namespace News.Areas.Admin.Models
{
    public class NewsCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public IEnumerable<NewsModel> News { get; set; }

    }
}
