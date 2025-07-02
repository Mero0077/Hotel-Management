using AutoMapper.Features;
using Hotel_Management.Models.Enums;
using Hotel_Management.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Hotel_Management.Filters
{
    public class CustomAuthorizeFilter : ActionFilterAttribute
    {
        RoleFeatureService _roleFeatureService;
        Features _feature;
        public CustomAuthorizeFilter(RoleFeatureService roleFeatureService, Features feature)
        {
            _roleFeatureService = roleFeatureService;
            _feature = feature;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var roleID = context.HttpContext.User.FindFirst(ClaimTypes.Role);
            if (roleID is null || string.IsNullOrEmpty(roleID.Value))
            {
                throw new UnauthorizedAccessException("You are not authorized to access this resource.");
            }
            var role = (Role)Enum.Parse(typeof(Role), roleID.Value, true);

            if (!_roleFeatureService.CheckFeatureAccess(_feature, role))
            {
                throw new UnauthorizedAccessException("You do not have permission to access this feature ya gayyyyy.");
            }

            base.OnActionExecuting(context);

        }
    }
}
