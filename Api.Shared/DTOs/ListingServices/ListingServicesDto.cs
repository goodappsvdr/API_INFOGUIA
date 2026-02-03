namespace Api.Shared.DTOs.ListingServices
{
    public class ListingServicesDto
    {
        public int ListingId { get; set; }
        public int ServiceId { get; set; }
    }
}

public class AddListingServicesDTO
{
    public int ListingId { get; set; }
    public int ServiceId { get; set; }
}
public class AddListingServicesComleteDTO
{
 
    public int ServiceId { get; set; }
}



public class GetListingServices
{

    public int ServiceId { get; set; }
    public DateTime CreatedAt { get; set; }
}
