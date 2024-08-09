namespace AE_Backend.Model
{
    public class Port
    {
        public int PortId { get; set; }
        public string? PortName { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public int RowStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        //public ICollection<Ship> Ships { get; set; }
    }

    public class PortInfo
    {
        public Port? Port { get; set; }
        public decimal DistanceKm { get; set; }
        public TimeSpan EstimatedArrivalTime { get; set; }
    }

}
