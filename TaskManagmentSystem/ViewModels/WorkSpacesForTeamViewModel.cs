using Microsoft.AspNetCore.Mvc;
using TaskManagmentSystem.Models;

namespace TaskManagmentSystem.ViewModels
{
    public class WorkSpaceForTeamViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public WorkSpaceColor Color { get; set; } = WorkSpaceColor.None;
    }
}
