using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAll", a => a.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});
builder.Services.AddDbContext<ProdDbContext>(x => x.UseNpgsql(builder.Configuration.GetConnectionString("Dbcon")));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();


app.MapGet("/Product", async (ProdDbContext db) =>
    await db.Products.ToListAsync()
);

app.MapGet("/Product/{ProductId}", async (int ProductId, ProdDbContext db) =>
    await db.Products.FirstOrDefaultAsync(q=>q.ProductId==ProductId)
    is Product product
    ? Results.Ok(product)
    : Results.NotFound()
);

app.MapPut("/Product/{ProductId}", async (int ProductId, Product product ,ProdDbContext db)=>
{
    var rec = await db.Products.FirstOrDefaultAsync(u => u.ProductId == ProductId);
    if (rec is null) return Results.NotFound();

    rec.Name = product.Name;
    rec.Description = product.Description;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPost("/Product", async (Product product, ProdDbContext db) =>
{
    db.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/Product/{product.ProductId}", product);
});

app.MapDelete("/Product/{ProductId}", async (int ProductId, ProdDbContext db) =>
{
    var rec = await db.Products.FirstOrDefaultAsync(x => x.ProductId == ProductId);
    if (rec is null) return Results.NotFound();
    db.Remove(rec);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
