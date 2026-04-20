using System.ComponentModel.DataAnnotations;

namespace Lab07.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email obligatoriu")]
    [EmailAddress(ErrorMessage = "Format email invalid")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola obligatorie")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}

public class RegisterViewModel
{
    [Required(ErrorMessage = "Email obligatoriu")]
    [EmailAddress(ErrorMessage = "Format email invalid")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola obligatorie")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmarea parolei este obligatorie")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Parolele nu coincid!")]
    public string ConfirmPassword { get; set; } = string.Empty;
}