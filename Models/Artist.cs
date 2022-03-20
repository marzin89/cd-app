namespace CdApp.Models
{
    // Klass som hanterar artister
    public class Artist
    {
        // Properties
        // ID
        public int ArtistId { get; set; }

        // Namn
        public string? Name { get; set; }

        // Artistens skivor
        public List<Cd> Albums = new List<Cd>();

        // Konstruktor
        public Artist() {}
    }
}
