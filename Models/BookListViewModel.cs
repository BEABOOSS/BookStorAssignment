namespace BookStore.Models
{
    public class BookListViewModel
    {
        public IEnumerable<Book>? Books { get; set; }
        public bool CanEditBook { get; set; }
    }
}
