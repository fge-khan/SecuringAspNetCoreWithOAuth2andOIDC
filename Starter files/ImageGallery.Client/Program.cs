using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(configure =>
        configure.JsonSerializerOptions.PropertyNamingPolicy = null);

JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

// create an HttpClient used for accessing the API
builder.Services.AddHttpClient("APIClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ImageGalleryAPIRoot"]!);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
  {
      options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      options.Authority = "https://localhost:5001";
      options.ClientId = "imagegalleryclient";
      options.ClientSecret = "secret";
      options.ResponseType = "code";

      // set by default
      //options.Scope.Add("openid");
      //options.Scope.Add("profile");

      // set by default
      //options.CallbackPath = new PathString("signin-oidc");

      //options.SignedOutCallbackPath: default = host:port/signout-callback-oidc.
      //must match with the post logout redirect URI at IDP client config if you want to automatically return to the application afer logging out of the IDP
      //To change , set SignedOutCallbackPath
      //eg: SignedOutCallback = "pathaftersignout"
      options.SaveTokens = true;
      options.GetClaimsFromUserInfoEndpoint = true;
      options.ClaimActions.Remove("aud"); // This ensure that the audience claim will be returned - what is removed here is the filter that remove the audience claim
      options.ClaimActions.DeleteClaim("idp"); // This remove the idp claim from the list of claims
      options.Scope.Add("roles");
      options.ClaimActions.MapJsonKey("role", "role");
      options.TokenValidationParameters = new()
      {
          NameClaimType = "given_name",
          RoleClaimType = "role"
      };
  });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Gallery}/{action=Index}/{id?}");

app.Run();
