using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagmentSystem.Models
{
    public class TaskEdiotor
    {
        [ForeignKey("Editor")]
        public string EditorId { get; set; } = null!;
        public AppUser Editor { get; set; } = null!;

        [ForeignKey("TaskEdited")]
        public int TaskEditedId { get; set; }
        public UserTask TaskEdited { get; set; } = null!;
    }
}
