namespace AE_Backend.Model
{
    public class Ship
    {
        public int ShipId { get; set; }
        public string? ShipName { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal Velocity { get; set; }
        public int RowStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        public ICollection<UserShip>? UserShips { get; set; }

    }

    public class ShipCreateParam
    {
        public string? ShipName { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal Velocity { get; set; }
        public string? CreatedBy { get; set; }

    }

    public class ShipUpdateParam
    {
        public int ShipId { get; set; }
        public string? ShipName { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal Velocity { get; set; }
        public int RowStatus { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
