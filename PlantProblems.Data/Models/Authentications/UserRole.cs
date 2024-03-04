namespace Plant_Problems.Data.Models.Authentications
{
    public class UserRole
    {
        [Required(ErrorMessage = "Username is required..!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Role is required..!")]
        [EnumDataType(typeof(Role))]
        public Role Role { get; set; }
    }
}
