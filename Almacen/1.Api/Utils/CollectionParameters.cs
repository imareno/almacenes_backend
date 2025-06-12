namespace Api._1.API.Utils
{
    public class CollectionParameters
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool Joined { get; set; }
        public string? Filter { get; set; }
        public string? Sort { get; set; }
    }
}
