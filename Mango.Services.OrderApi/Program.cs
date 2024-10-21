using AutoMapper;
using Mango.ServiceBus;
using Mango.Services.OrderApi;
using Mango.Services.OrderApi.Extensions;
using Mango.Services.OrderApi.Services;
using Mango.Services.OrderApi.Services.IService;
using Mango.Services.OrderApi.Utility;
using Mango.Services.ShoppingCart.Data;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(option =>
{
	option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IProductService, Mango.Services.OrderApi.Services.ProductService>();
builder.Services.AddScoped<IMessageBus, MessageBus>();
builder.Services.AddScoped<AuthenticationHttpClientHandler>();

builder.Services.AddHttpClient("Product", u => u.BaseAddress =
	new Uri(builder.Configuration["ServicesUrl:ProductAPI"])).AddHttpMessageHandler<AuthenticationHttpClientHandler>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.AddSwaggerConfiguration();
builder.AddAuthConfiguration();


var app = builder.Build();

app.UseSwagger();
if (app.Environment.IsDevelopment())
{

    app.UseSwaggerUI();
}
else
{
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ORDER API");
        c.RoutePrefix = string.Empty;
    });
}
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<String>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.Run();


void ApplyMigration()
{
	using (var scope = app.Services.CreateScope())
	{
		var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

		if (_db.Database.GetPendingMigrations().Count() > 0)
		{
			_db.Database.Migrate();
		}
	}
}