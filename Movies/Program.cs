using Microsoft.EntityFrameworkCore;
using Movies.Config;
using Movies.Data;
using Movies.Repository;
using Movies.Repository.Implementations;
using Movies.Repository.Interfaces;
using Movies.Services;
using Movies.Services.Implementations;
using Movies.Services.Interfaces;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new NotFound404ExceptionFilters());
    options.Filters.Add(new Duplicate409ConflictException());
    options.Filters.Add(new BadRequest400BadException());
})
    .AddNewtonsoftJson(option =>
{
    option.SerializerSettings.Converters.Add(new StringEnumConverter());
    option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
//Database Configurations
builder.Services
    .AddDbContext<DataContext>(
    option => option
        .UseNpgsql(builder
            .Configuration.GetConnectionString("connection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IMovieService, MovieService>();
builder.Services.AddTransient<IMoviesRepository, MoviesRepository>();
builder.Services.AddTransient<ITheatreService, TheatreService>();
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddTransient<IGenreRepository, GenreRepository>();
builder.Services.AddTransient<IGenreService, GenreService>();
builder.Services.AddTransient<IMovieShowService, MovieShowService>();
builder.Services.AddTransient<IMovieShowRepository, MovieShowRepository>();
builder.Services.AddTransient<ITicketRepository, TicketRepository>();
builder.Services.AddTransient<IMovieEventBookingService, MovieEventBookingService>();
builder.Services.AddTransient<IMovieEventBookingRepository, MovieEventBookingRepository>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
