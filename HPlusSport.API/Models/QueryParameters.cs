namespace HPlusSport.API.Models
{
    public class QueryParameters
    {
        const int _maxSize = 100;
        // size of my page size
        private int _size = 50;

        public int Page { get; set; } = 1; // getters and setters

        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = Math.Min(_maxSize, value);
            }
        }
    }
}
