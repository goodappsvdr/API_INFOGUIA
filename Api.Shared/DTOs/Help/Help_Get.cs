namespace Api.Shared.DTOs.Help
{
    public class Help_Get
    {
        public int HelpId { get; set; }

        public string? Question { get; set; }

        public string? Answer { get; set; }
    }
}
