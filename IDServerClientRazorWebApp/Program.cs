using System.IdentityModel.Tokens.Jwt;

//JM: [IDSRVR] Added conforming to IDServer tutorial (1)
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//JM: [IDSRVR] Added conforming to IDServer tutorial (2)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:5001";

        options.ClientId = "web";
        options.ClientSecret = "secret";
        options.ResponseType = "code";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");

        options.SaveTokens = true;
    });

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//JM: [IDSRVR] Added conforming to IDServer tutorial (3) (DUH)
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages()
    //JM: [IDSRVR] Added conforming to IDServer tutorial (4)
    .RequireAuthorization();

app.Run();