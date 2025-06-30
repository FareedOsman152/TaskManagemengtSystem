using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Srvices;
using TaskManagmentSystem.Srvices.Interfaces;

namespace TaskManagmentSystem.Filters
{
    public class TeamPermissionsFilter : IAsyncAuthorizationFilter
    {
        private readonly ITeamAppUserService _teamAppUserService;
        private readonly TeamPermissions _permissionCheck;

        public TeamPermissionsFilter(ITeamAppUserService teamAppUserService, TeamPermissions permissionCheck)
        {
            _teamAppUserService = teamAppUserService;
            _permissionCheck = permissionCheck;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Console.WriteLine("TeamPermissionsFilter executing...");
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!context.RouteData.Values.TryGetValue("id", out var teamIdObj) ||
                !int.TryParse(teamIdObj?.ToString(), out var teamId))
            {
                context.Result = new BadRequestObjectResult("Team ID is required");
                return;
            }
            var isHasPermissionsResult = await _teamAppUserService.IsHasPermissionAsync(userId!, teamId,_permissionCheck);
            if(!isHasPermissionsResult.Succeeded || !isHasPermissionsResult.Data)
                context.Result = new ForbidResult();
        }
    }
}
