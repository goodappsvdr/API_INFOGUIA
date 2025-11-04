namespace Api.Shared.DTOs.Identity.AspNetUsers
{
    public class User_ChangePassword
    {
        public string CurrenPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
