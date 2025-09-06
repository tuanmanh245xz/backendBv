var builder = WebApplication.CreateBuilder(args);
//cau hinh ket noi csdl

// Add services to the container.
builder.Services.AddControllersWithViews();

//thêm  session để lưu trữ thông tin đăng nhập
builder.Services.AddDistributedMemoryCache(); // Sử dụng bộ nhớ trong để lưu trữ session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian tồn tại của session
    options.Cookie.HttpOnly = true; // Bảo mật cookie
    options.Cookie.IsEssential = true; // Cookie cần thiết cho ứng dụng
});

//thêm các dịch vụ API 
builder.Services.AddControllers();

var app = builder.Build();

// Cấu hình middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Kích hoạt session middleware
//Map các enpoint cho API
app.MapControllers();
// Cấu hình route cho MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
