namespace ImageUploadDemo.Models
{
    public class ZipEntity
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
        public double Rate { get; set; }
        public double Before { get; set; }
        public double After { get; set; }
    }
}