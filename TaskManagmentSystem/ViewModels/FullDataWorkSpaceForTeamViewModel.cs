using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagmentSystem.ViewModels
{
    public class FullDataWorkSpaceForTeamViewModel
    {
        public List<WorkSpaceForTeamViewModel> Workspaces { get; set; }
        public string Name { get; set; } = null!;

        [HiddenInput]
        public string AdminId { get; set; } = null!;
        
        [HiddenInput]
        public int TeamId { get; set; }

        [HiddenInput]
        public string UserId { get; set; }
    }
}
