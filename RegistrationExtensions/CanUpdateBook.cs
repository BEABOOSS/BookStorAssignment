using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BookStore.RegistrationExtensions
{
    public class CanUpdateBook : IAuthorizationRequirement
    {
        public CanUpdateBook(byte minPermLvl)
        {
            MinPermLvl = minPermLvl;
        }

        public byte MinPermLvl { get; }
    }

    public class IsManagerHandler :
        AuthorizationHandler<CanUpdateBook>
    {
        private readonly UserManager<UserIdentity> _userManager;
        public IsManagerHandler(
            UserManager<UserIdentity> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CanUpdateBook requirement)
        {
            var appUser = await _userManager.GetUserAsync(context.User);
            if (appUser == null)
            {
                return;
            }
            if (appUser.Permission >= requirement.MinPermLvl)
            {
                context.Succeed(requirement);
            }
        }


    }
}
