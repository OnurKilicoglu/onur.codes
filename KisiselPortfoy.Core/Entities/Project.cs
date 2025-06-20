namespace KisiselPortfoy.Core.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? CodeFileUrl { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
