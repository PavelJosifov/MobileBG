namespace MobileBG.Web.ViewModels.Blogs;

public class CreateBlogViewModel
{
    [Display(Name = "Title")]
    [Required]
    [MinLength(3, ErrorMessage = "Enter a Title with minimum length of 3 symbols")]
    public string Title { get; set; }

    [Required]
    [MinLength(50, ErrorMessage = "Enter a Content with minimum length of 50 symbols")]
    public string Content { get; set; }

    [Display(Name = "Select main image")]
    [AllowedExtensions(".png", ".jpg", ".jpeg")]
    [Required(ErrorMessage = "Image is required")]
    public IFormFile Image { get; set; }
}
