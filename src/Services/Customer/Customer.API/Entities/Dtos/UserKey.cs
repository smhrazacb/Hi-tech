namespace Customer.API.Entities.Dtos
{
    public class UserKey
    {
        public Guid Id { get; set; }
        public int AddressId{ get; set; }
        public int ContactId { get; set; }
        public int? GeoDataId { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
