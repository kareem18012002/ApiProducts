using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NoRiskNoFun;
using NoRiskNoFun.Authorization;
using NoRiskNoFun.Data;
using NoRiskNoFun.Filters;
using NoRiskNoFun.Middleware;
using NoRiskNoFun.services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging(
    cfg =>     {
 
        cfg.AddDebug();
    }   
    );
builder.Configuration.AddJsonFile("config.json");
builder.Services.Configure<AttachmentOptions>(builder.Configuration.GetSection("Attachments"));

/*
var attachmentOptions = builder.Configuration.GetSection("Attachments").Get<AttachmentOptions>();
builder.Services.AddSingleton(attachmentOptions);
var attachmentOptions = new AttachmentOptions();
builder.Configuration.GetSection("Attachments").Bind(attachmentOptions);
builder.Services.AddSingleton(attachmentOptions);

*/

// Add services to the container.
var jwtOption = builder.Configuration.GetSection("Jwt").Get<JwtOptionscs>();
builder.Services.AddSingleton(jwtOption);
builder.Services.AddControllers(options => {

    options.Filters.Add<LogActivityFilter>();
    options.Filters.Add<LogSensitiveActivityAttribute>();
    options.Filters.Add<PermissionBesedAuthorizationFilter>();

});



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
//builder.Services.AddTransient<WeatherForecastServices>();// لو فى اكتر من request يعمل لكل واحدة واحد جديد لكلكلاس  //هيعمل instance جديد لكل request
//builder.Services.AddScoped<IWeatherForecastServices , WeatherForecastServices>(); // هيعمل instance واحد لكل request بس لو فى request جديد هيعمل instance جديد
//.Services.AddSingleton<WeatherForecastServices>(); // هيعمل instance واحد للكلاس ده طول ما الابلكيشن شغال

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
       builder.Configuration["ConnectionStrings:DefaultConnection"]
       
       
    )
);
builder.Services.AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults .AuthenticationScheme, options =>
    {
     options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOption.Issuer,
            ValidAudience = jwtOption.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SigningKey)),
            ClockSkew = TimeSpan.Zero
        };
    } 
    );
//.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);


var app = builder.Build();


// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<ProfilingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Run the app
app.Run();
