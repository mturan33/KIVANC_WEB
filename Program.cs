// Gerekli using ifadeleri
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KIVANC_WEB.Data; // Projenizin adýnýn "KIVANC_WEB" olduðunu varsayýyoruz
using KIVANC_WEB.Services;

var builder = WebApplication.CreateBuilder(args);

// WeatherService ve HttpClient'ý projenin servislerine ekliyoruz.
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddScoped<WeatherService>();

// Add services to the container.

// 1. ADIM: SQLite için veritabaný baðlantý dizesini alýyoruz.
// builder.Configuration.GetConnectionString("DefaultConnection") ifadesi appsettings.json dosyasýndan ilgili satýrý okur.
// "?? ..." kýsmý, eðer appsettings.json içinde bu satýr bulunamazsa hata fýrlatmasýný saðlar.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 2. ADIM: DbContext'i servislere ekliyoruz.
// Burasý en önemli deðiþiklik. UseSqlServer yerine UseSqlite kullanýyoruz.
// Bu tek satýrlýk deðiþiklik, projenizin artýk bir SQLite veritabanýyla konuþmasýný saðlýyor.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Bu servis, veritabaný hatalarýný geliþtirme ortamýnda daha anlaþýlýr bir sayfada gösterir.
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Projenizde kullanýcý giriþi (login, register) sistemini (ASP.NET Core Identity) yapýlandýrýr.
// Veritabaný olarak ApplicationDbContext'i kullanacaðýný belirtir.
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Razor Pages servislerini uygulamaya ekler.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Geliþtirme ortamýnda, veritabaný migration'larý ile ilgili olasý hatalarý
    // gösteren bir ara katman (middleware) ekler.
    app.UseMigrationsEndPoint();
}
else
{
    // Canlý (Production) ortamýnda bir hata olursa, kullanýcýyý genel bir hata sayfasýna yönlendirir.
    app.UseExceptionHandler("/Error");
    // Tarayýcýlara sitenin sadece HTTPS üzerinden ziyaret edilmesi gerektiðini bildiren güvenlik katmaný.
    app.UseHsts();
}

// HTTP isteklerini otomatik olarak HTTPS'e yönlendirir.
app.UseHttpsRedirection();

// wwwroot klasöründeki statik dosyalarýn (css, js, resimler vb.) kullanýlmasýný saðlar.
app.UseStaticFiles();

// Gelen isteðin hangi Razor Page'e veya Controller'a gideceðini belirleyen yönlendirme mekanizmasýný aktif eder.
app.UseRouting();

// Kimlik doðrulama ve yetkilendirme (Authentication & Authorization) ara katmanlarýný aktif eder.
// ÖNEMLÝ: UseRouting'den sonra ve MapRazorPages'den önce olmalýdýr.
app.UseAuthorization();

// Razor Pages için yönlendirme kurallarýný eþleþtirir.
app.MapRazorPages();

// Uygulamayý çalýþtýrýr.
app.Run();