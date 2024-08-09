namespace AE_Backend.Model
{
    public class Role
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public int RowStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; }
    }

    public class RoleCreateParam
    {
        public string? RoleName { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class RoleUpdateParam
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public int RowStatus { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
