namespace JanLehner_Backend_Prototyp_1706.Models
{
    public class CarModel
    {
        public int CarID { get; set; }
        public string NumberPlate { get; set; }
        public bool IsParked { get; set; }
        public DateTime PaidUntil { get; set; }
    }
}
