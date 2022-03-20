using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CdApp.Models
{
    // Klass som hanterar skivor
    public class Cd
    {
        // Properties med anpassade fältetiketter och felmeddelanden
        // Skivans ID
        public int CdId { get; set; }

        // Skivans namn
        [Required(ErrorMessage = "Du måste ange skivans namn")]
        [Display(Name = "Namn *")]
        public string? Name { get; set; }

        // Artistens ID
        [ForeignKey("Artist")]
        public int ArtistId { get; set; }

        // Artistens namn
        [Required(ErrorMessage = "Du måste ange en artist")]
        [Display(Name = "Artist *")]
        public string Artist { get; set; }

        // Året då skivan kom ut
        [Required(ErrorMessage = "Du måste ange ett år")]
        [Display(Name = "År *")]
        public string? Year { get; set; }

        // Skivans längd
        [Display(Name = "Längd")]
        public string? Length { get; set; }

        // Skivbolag
        [Display(Name = "Skivbolag")]
        public string? Label { get; set; }

        // Genre
        [Required(ErrorMessage = "Du måste ange en genre")]
        [Display(Name = "Genre *")]
        public string? Genre { get; set; }

        // Utlånad eller inte
        public bool IsAvailable { get; set; } = true;

        // Utlånad av
        public int BorrowedBy { get; set; }

        // Datum för utlåning
        public DateTime Borrowed { get; set; }

        // Datum då skivan lämnas tillbaka
        public DateTime? BackInStock { get; set; }

        // Konstruktor
        public Cd() {}
    }
}
