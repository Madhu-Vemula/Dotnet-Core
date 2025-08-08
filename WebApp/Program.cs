using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;

namespace WebApp
{
    class Program
    {
        public static void StartWebApplication(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1",
                    Description = "A simple weather forecast API",
                    Contact = new OpenApiContact
                    {
                        Name = "Madhu Vemula",
                        Email = "madhu.vemula@pal.tech"
                    }
                });
            });

            // Add authorization services (required for UseAuthorization)
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
        static void Main(string[] args)
        {
            StartWebApplication(args);
            new Program().InsertRecord();
        }
        public void InsertRecord()
        {
            SqlConnection? con = null;
            try
            {
                // Creating Connection  
                con = new SqlConnection("data source=.; database=paltech; integrated security=SSPI");
                // writing sql query  
                SqlCommand cm = new SqlCommand("insert into Employees values ('1', 'Madhu Vemula', 'madhu.vemula@pal.tech', '1','101','madhu@pal')", con);

                // Opening Connection  
                con.Open();
                // Executing the SQL query  
                cm.ExecuteNonQuery();
                // Displaying a message  
                Console.WriteLine("Record Inserted Successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine("OOPs, something went wrong." + e);
            }
            // Closing the connection  
            finally
            {
                con?.Close();
            }
        }
    }
}