using CdApp.Data;
using System.ComponentModel.DataAnnotations;

namespace CdApp.Models
{
    // Klass som hanterar användare
    public class User
    {
        // Properties med anpassade fältetiketter och felmeddelanden
        // ID
        public int UserId { get; set; }

        // Användarnamn
        [Display(Name = "Användarnamn *")]
        [Required(ErrorMessage = "Du måste ange ditt användarnamn")]
        public string? UserName { get; set; }

        // Lösenord
        [Display(Name = "Lösenord *")]
        [Required(ErrorMessage = "Du måste ange ditt lösenord")]
        public string? Password { get; set; }

        // Konstruktor
        public User() {}
    }
}
