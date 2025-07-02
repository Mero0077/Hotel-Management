using Hotel_Management.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Hotel_Management.Models.Enums
{
    //public class CustomAuthorizeFilter:ActionFilterAttribute
    //{
    //    private RoleFeatureService _roleFeatureService;
    //    Features Feature { get; set; }

    //    public CustomAuthorizeFilter(Features feature, RoleFeatureService roleFeatureService)
    //    {
    //            Feature = feature;
    //        _roleFeatureService = roleFeatureService;
    //    }

    //    public override void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        var RoleId = context.HttpContext.User.FindFirst(ClaimTypes.Role);

    //        if (RoleId == null || string.IsNullOrEmpty(RoleId.Value))
    //         {
    //            throw new UnauthorizedAccessException("Not authorized!");
    //        }
    //        var role= (Role) Enum.Parse(typeof(Role), RoleId.Value,true);

    //       if( !_roleFeatureService.CheckFeatureAccess(Feature,role))
    //        {
    //            throw new UnauthorizedAccessException("Not authorized!");
    //        }

    //        base.OnActionExecuting(context);
    //    }
    //}
}
