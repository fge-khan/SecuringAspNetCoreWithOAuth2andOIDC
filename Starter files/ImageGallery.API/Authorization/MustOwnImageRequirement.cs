using Microsoft.AspNetCore.Authorization;

namespace ImageGallery.API.Authorization
{
    public class MustOwnImageRequirement : IAuthorizationRequirement
    {
        // her can you pass whatever additional parameters needed for the requirement as they would be accessible from the handler.
        public MustOwnImageRequirement()
        {

        }
    }
}
