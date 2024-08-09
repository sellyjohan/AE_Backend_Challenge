using System.Text.Json.Serialization;

namespace AE_Backend.Model
{
    public class UserShip
    {
        public int UserShipId { get; set; }
        public int UserId { get; set; }
        public int ShipId { get; set; }
        public int RowStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        [JsonIgnore]
        public User? User { get; set; } // Reference to User
        public Ship? Ship { get; set; } // Reference to Ship
    }

    public class UserShipCreateParam
    {
        public int UserId { get; set; }
        public int ShipId { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class UserShipUpdateParam
    {
        public int UserShipId { get; set; }
        public int UserId { get; set; }
        public int ShipId { get; set; }
        public int RowStatus { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
