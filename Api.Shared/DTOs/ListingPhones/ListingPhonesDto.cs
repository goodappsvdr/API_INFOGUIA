namespace Api.Shared.DTOs.ListingPhones
{
    public class ListingPhonesDto
    {
        public int Id { get; set; }              // ListingPhoneID
        public int ListingId { get; set; }
        public string PhoneType { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }


    public class AddListingPhonesDTO
    {
        public int ListingId { get; set; }
        public string PhoneType { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int SortOrder { get; set; }
    }
    public class AddListingPhonesCompleteDTO
    {
   
        public string PhoneType { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int SortOrder { get; set; }
    }


    public class UpdateListingPhonesDTO
    {
        public int Id { get; set; }              // ListingPhoneID
        public string PhoneType { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
    public class GetListingPhones
    {
        public int ListingPhoneID { get; set; }
        public string PhoneType { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }

}

