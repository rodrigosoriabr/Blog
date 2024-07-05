namespace Blog.Models
{
    public class Category
    {
        public Category()
        {
            Id = 0;
            Posts = new List<Post>();
        }
        
        public int Id { get; private set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public IList<Post>? Posts { get; private set; } 
    }
}