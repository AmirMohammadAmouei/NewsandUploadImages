namespace News.Areas.Admin.ViewModels
{
    public class UserFilterViewModels
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string PersonalId { get; set; }
    }
}
