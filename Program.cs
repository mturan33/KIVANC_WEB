// Gerekli using ifadeleri
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KIVANC_WEB.Data; // Projenizin ad�n�n "KIVANC_WEB" oldu�unu varsay�yoruz
using KIVANC_WEB.Services;

var builder = WebApplication.CreateBuilder(args);

// WeatherService ve HttpClient'� projenin servislerine ekliyoruz.
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddScoped<WeatherService>();

// Add services to the container.

// 1. ADIM: SQLite i�in veritaban� ba�lant� dizesini al�yoruz.
// builder.Configuration.GetConnectionString("DefaultConnection") ifadesi appsettings.json dosyas�ndan ilgili sat�r� okur.
// "?? ..." k�sm�, e�er appsettings.json i�inde bu sat�r bulunamazsa hata f�rlatmas�n� sa�lar.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 2. ADIM: DbContext'i servislere ekliyoruz.
// Buras� en �nemli de�i�iklik. UseSqlServer yerine UseSqlite kullan�yoruz.
// Bu tek sat�rl�k de�i�iklik, projenizin art�k bir SQLite veritaban�yla konu�mas�n� sa�l�yor.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Bu servis, veritaban� hatalar�n� geli�tirme ortam�nda daha anla��l�r bir sayfada g�sterir.
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Projenizde kullan�c� giri�i (login, register) sistemini (ASP.NET Core Identity) yap�land�r�r.
// Veritaban� olarak ApplicationDbContext'i kullanaca��n� belirtir.
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Razor Pages servislerini uygulamaya ekler.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Geli�tirme ortam�nda, veritaban� migration'lar� ile ilgili olas� hatalar�
    // g�steren bir ara katman (middleware) ekler.
    app.UseMigrationsEndPoint();
}
else
{
    // Canl� (Production) ortam�nda bir hata olursa, kullan�c�y� genel bir hata sayfas�na y�nlendirir.
    app.UseExceptionHandler("/Error");
    // Taray�c�lara sitenin sadece HTTPS �zerinden ziyaret edilmesi gerekti�ini bildiren g�venlik katman�.
    app.UseHsts();
}

// HTTP isteklerini otomatik olarak HTTPS'e y�nlendirir.
app.UseHttpsRedirection();

// wwwroot klas�r�ndeki statik dosyalar�n (css, js, resimler vb.) kullan�lmas�n� sa�lar.
app.UseStaticFiles();

// Gelen iste�in hangi Razor Page'e veya Controller'a gidece�ini belirleyen y�nlendirme mekanizmas�n� aktif eder.
app.UseRouting();

// Kimlik do�rulama ve yetkilendirme (Authentication & Authorization) ara katmanlar�n� aktif eder.
// �NEML�: UseRouting'den sonra ve MapRazorPages'den �nce olmal�d�r.
app.UseAuthorization();

// Razor Pages i�in y�nlendirme kurallar�n� e�le�tirir.
app.MapRazorPages();

// Uygulamay� �al��t�r�r.
app.Run();