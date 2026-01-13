namespace Api.Shared.DTOs.ListingPaymentMethods
{
    public class ListingPaymentMethodsDto
    {
        public int Id { get; set; }              // PaymentMethodID
        public int ListingId { get; set; }
    }

    public class AddListingPaymentMethodsDTO
    {
        public int ListingId { get; set; }
        public int PaymentMethodId { get; set; }
    }

    public class UpdateListingPaymentMethodsDTO
    {
        public int Id { get; set; }              // PaymentMethodID
        public int ListingId { get; set; }
    }
}
