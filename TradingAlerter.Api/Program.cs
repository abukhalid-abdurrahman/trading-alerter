using ElectronNET.API;

var builder = WebApplication.CreateBuilder(args);

// Add ElectronNET.
builder.WebHost.UseElectron(args);

// Using developement environment.
// TODO: Remove on release.
builder.WebHost.UseEnvironment("Development");

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Creating ElectronJS Window/App
if (HybridSupport.IsElectronActive)
{
    var window = await Electron.WindowManager.CreateWindowAsync();
    window.OnClosed += () => {
        Electron.App.Quit();
    };
}

app.Run();
