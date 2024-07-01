using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

// Editor prefix is a convention to use when the model are create and edit with same props;
// Add the required "annotation" to force the prop be required; 

public class EditorCategoryViewModel
{
    [Required]
    [StringLength(40, MinimumLength = 3)]
    public required string Name { get; set; }

    [Required] public required string Slug { get; set; }
}