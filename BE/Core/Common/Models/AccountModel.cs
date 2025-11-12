namespace ERP_API.Models
{
    public class AccountModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; }
    }

    public class UpdateAccountModel
    {
        public string Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class AccountSearchModel : BaseSearch
    {

    }

    public class ChangePasswordModel
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
