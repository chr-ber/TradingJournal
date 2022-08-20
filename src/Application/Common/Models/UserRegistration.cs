namespace TradingJournal.Application.Common.Models;

public class UserRegistration
{
   [Required]
   [StringLength(16, ErrorMessage = "Your username is too long (32 characters max).")]
   public string DisplayName { get; set; }

   [Required]
   [EmailAddress(ErrorMessage = "The email address is invalid.")]
   public string Email { get; set; }

   [Required]
   [StringLength(128, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 128 characters.")]
   public string Password { get; set; }

   [Required]
   [Compare(nameof(Password))]
   public string ConfirmPassword { get; set; }

   [Required]
   [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the Terms of Service.")]
   public bool TermsOfServiceAccepted { get; set; }
}
