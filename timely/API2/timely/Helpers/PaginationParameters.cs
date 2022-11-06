namespace timely.Helpers
{
    public class PaginationParameters
    {
        private const int maxItemsPerPage = 20;
        private int itemsPerPage;

        public int Page { get; set; } = 1;
        public int ItemsPerPage
        {
            get => itemsPerPage;
            set => itemsPerPage = value > maxItemsPerPage ? maxItemsPerPage : value;
        }
    }
}
