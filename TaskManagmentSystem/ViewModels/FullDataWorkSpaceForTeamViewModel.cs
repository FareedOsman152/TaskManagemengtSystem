using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagmentSystem.ViewModels
{
    public class FullDataWorkSpaceForTeamViewModel
    {
        public List<WorkSpaceForTeamViewModel> Workspaces { get; set; }
        public List<UserDetailsForTeamViewModel> Users { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }

        [HiddenInput]
        public string AdminId { get; set; } = null!;
        
        [HiddenInput]
        public int TeamId { get; set; }
        public string AdminName { get; set; } = null!;

        [HiddenInput]
        public string UserId { get; set; }
    }
}
