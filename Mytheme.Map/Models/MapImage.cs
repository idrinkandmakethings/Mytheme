namespace Mytheme.Map.Models
{
    public class MapImage
    {
        public string Url { get; set; }
        public long Height { get; set; }
        public long Width { get; set; }

        public MapImage(string url, long height, long width)
        {
            Url = url;
            Height = height;
            Width = width;
        }
    }
}
