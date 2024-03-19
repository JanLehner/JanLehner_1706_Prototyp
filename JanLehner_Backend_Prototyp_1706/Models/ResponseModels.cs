namespace JanLehner_Backend_Prototyp_1706.Models
{
    public class Box
    {
        public int xmin { get; set; }
        public int ymin { get; set; }
        public int xmax { get; set; }
        public int ymax { get; set; }
    }

    public class Candidate
    {
        public double score { get; set; }
        public string plate { get; set; }
    }

    public class Region
    {
        public string code { get; set; }
        public double score { get; set; }
    }

    public class Result
    {
        public Box box { get; set; }
        public string plate { get; set; }
        public Region region { get; set; }
        public double score { get; set; }
        public List<Candidate> candidates { get; set; }
        public double dscore { get; set; }
        public Vehicle vehicle { get; set; }
    }

    public class Root
    {
        public double processing_time { get; set; }
        public List<Result> results { get; set; }
        public string filename { get; set; }
        public int version { get; set; }
        public object camera_id { get; set; }
        public DateTime timestamp { get; set; }
    }

    public class Vehicle
    {
        public double score { get; set; }
        public string type { get; set; }
        public Box box { get; set; }
    }
}
