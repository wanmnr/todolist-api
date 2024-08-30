using System.ComponentModel.DataAnnotations;

namespace todolist_api.Models
{
    [MetadataType(typeof(TodoMetaData))]
    public partial class Todo
    {
        // Partial class for Todo
        // Core properties are defined in Todo.cs, but this class is linked to metadata.
    }

    // This class holds the data annotations for the Todo class
    public class TodoMetaData
    {
        public TodoMetaData()
        {
            Title = string.Empty;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [Required]
        public bool Complete { get; set; }
    }
}
