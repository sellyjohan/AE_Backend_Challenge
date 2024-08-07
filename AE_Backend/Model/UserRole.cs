using System.Text.Json.Serialization;

namespace AE_Backend.Model
{
    public class UserRole
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int RowStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        public Role Role { get; set; }
    }

    public class UserRoleCreateParam
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string CreatedBy { get; set; }
    }

    public class UserRoleUpdateParam
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int RowStatus { get; set; }
        public string ModifiedBy { get; set; }
    }
}
