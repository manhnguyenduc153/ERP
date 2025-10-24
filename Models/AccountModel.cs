namespace ERP_API.Models
{
    public class AccountModel
    {
    }

    public class UpdateAccountModel
    {
        public string Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class ChangePasswordModel
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
