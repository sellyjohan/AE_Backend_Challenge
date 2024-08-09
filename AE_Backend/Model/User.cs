using Microsoft.VisualBasic;
using System.Text.Json.Serialization;

namespace AE_Backend.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Fullname { get; set; }
        public DateTime Birthdate { get; set; }
        public int RowStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UserShip> UserShips { get; set; }
    }

    public class UserCreateParam
    {
        public string? Username { get; set; }
        public string? Fullname { get; set; }
        public DateTime Birthdate { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class UserUpdateParam
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Fullname { get; set; }
        public DateTime Birthdate { get; set; }
        public int RowStatus { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
