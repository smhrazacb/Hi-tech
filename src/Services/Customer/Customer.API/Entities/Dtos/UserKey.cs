namespace Customer.API.Entities.Dtos
{
    public class UserKey
    {
        public string Id { get; set; }
        public int AddressId{ get; set; }
        public int? GeoDataId { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
