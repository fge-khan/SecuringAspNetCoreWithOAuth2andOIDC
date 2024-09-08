using ImageGallery.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace ImageGallery.API.Authorization
{
    public class MustOwnImageHandler : AuthorizationHandler<MustOwnImageRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGalleryRepository _galleryRepository;

        public MustOwnImageHandler(IHttpContextAccessor httpContextAccessor, IGalleryRepository galleryRepository)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _galleryRepository = galleryRepository ?? throw new ArgumentNullException(nameof(galleryRepository));
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustOwnImageRequirement requirement)
        {
            var imageId = _httpContextAccessor.HttpContext?.GetRouteValue("id")?.ToString();
            
            if (!Guid.TryParse(imageId, out var imageIdAsGuid))
            {
                context.Fail();
                return;
            }

            //Get the sub claim
            var ownerId = context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (ownerId == null)
            {
                context.Fail();
                return;
            }

            if(!await _galleryRepository.IsImageOwnerAsync(imageIdAsGuid, ownerId))
            {
                context.Fail();
                return;
            }

            //Everithing is ok
            context.Succeed(requirement);
        }
    }
}
