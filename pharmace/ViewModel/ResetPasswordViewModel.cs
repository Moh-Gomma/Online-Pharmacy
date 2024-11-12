using System.ComponentModel.DataAnnotations;

public class ResetPasswordViewModel
{
    public string Token { get; set; }

    [Required(ErrorMessage = "يرجى إدخال كلمة المرور الجديدة.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "كلمة المرور يجب أن تكون على الأقل 8 أحرف.")]
    [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}", ErrorMessage = "كلمة المرور يجب أن تتضمن حرفاً كبيراً، حرفاً صغيراً ورقماً.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "يرجى تأكيد كلمة المرور.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "كلمات المرور غير متطابقة.")]
    public string ConfirmPassword { get; set; }
}