using AutoMapper;
using Library.API.Entities;
using Library.API.Helpers;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Library.API
{
    public class Startup
    {
        public static IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
            });

            // register the DbContext on the container, getting the connection string from
            // appSettings (note: use this during development; in a production environment,
            // it's better to store the connection string in an environment variable)
            var connectionString = Configuration["connectionStrings:libraryDBConnectionString"];
            services.AddDbContext<LibraryContext>(o => o.UseSqlServer(connectionString));

            // register the repository
            services.AddScoped<ILibraryRepository, LibraryRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorDTO>()
                .ForMember(dest => dest.Name, from => from.MapFrom(src =>
                $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Age, from => from.MapFrom(src =>
                src.DateOfBirth.GetCurrentAge()));

                cfg.CreateMap<Book, BookDTO>();

                cfg.CreateMap<AuthorCreateResourceDTO, Author>();
                cfg.CreateMap<AuthorUpdateResourceDTO, Author>();

                cfg.CreateMap<BookCreateResourceDTO, Book>();
                cfg.CreateMap<BookUpdateResourceDTO, Book>();
                cfg.CreateMap<Book, BookUpdateResourceDTO>();
            });

            services.AddSingleton(config.CreateMapper());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, LibraryContext libraryContext)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler(appBuild =>
                {
                    appBuild.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Falha inesperada. Tenta novamente mais tarde!");
                    });
                });
            }
            else
            {
                app.UseExceptionHandler(appBuild =>
                {
                    appBuild.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Falha inesperada. Tenta novamente mais tarde!");
                    });
                });
            }

            libraryContext.EnsureSeedDataForContext();

            app.UseMvc();
        }
    }
}
