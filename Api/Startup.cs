using Microsoft.EntityFrameworkCore;
using minimals_api.Dominio.DTOs;
using minimals_api.Dominio.Services;
using minimals_api.Infraestrutura.Context;
using minimals_api.Dominio.Entities;
using minimals_api.Dominio.Interfaces;
using minimals_api.Dominio.ModelViews;
using Microsoft.AspNetCore.Mvc;
using minimals_api.Dominio.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authorization;
using minimals_api;


namespace minimals_api
{
    
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            key = Configuration.GetSection("Jwt").ToString() ?? "123456";

        }


        public IConfiguration Configuration { get; set; }
        public string key { get; set; }

        #region Funções
            ErrosDeValidacao validaDTO(VeiculoDTO veiculoDTO){
                    var validacao = new ErrosDeValidacao{
                        Mensagens = new List<string>()
                    };
                    if(string.IsNullOrEmpty(veiculoDTO.Nome)){
                        validacao.Mensagens.Add("O Nome não pode não pode ficar em branco.");
                    }
                    if(string.IsNullOrEmpty(veiculoDTO.Marca)){
                        validacao.Mensagens.Add("A Marca não pode ficar em branco.");
                    }
                    if(veiculoDTO.Ano<1950){
                        validacao.Mensagens.Add("Veículo muito  antigo, aceito somente anos superiores a 1950.");
                    }
                    return validacao;
            }


            string GerarTokenJwt(Administrador administrador){
                if(string.IsNullOrEmpty(key)) return string.Empty;
                
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim> (){ 
                new Claim("Email",administrador.Email), 
                new Claim("Perfil", administrador.Perfil),
                new Claim(ClaimTypes.Role,administrador.Perfil)};
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials

                );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }

        #endregion
        public void ConfigureServices(IServiceCollection services){
            
            services.AddAuthentication(option =>{
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>{
            option.TokenValidationParameters = new TokenValidationParameters{
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
            };
            });
            services.AddAuthorization();

            services.AddScoped<IAdministradorService,AdministradorService>();
            services.AddScoped<IVeiculoService,VeiculoService>();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options=> {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o tokne JWT aqui: "
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement { 
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddDbContext<OrganizadorContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ConexaoPadrao")));
        }



        public void Configure(IApplicationBuilder app,IWebHostEnvironment env){
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>{
                            #region Home
                            endpoints.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
                            #endregion

                            #region Administradores


                            endpoints.MapPost("/Administradores/login", ([FromBody]LoginDTO loginDTO, IAdministradorService administradorService) =>{
                                var adm = administradorService.Login(loginDTO);
                                if(adm!=null){
                                    string token = GerarTokenJwt(adm);
                                    return Results.Ok(new AdministradorLogado{
                                        Email = adm.Email,
                                        Perfil = adm.Perfil,
                                        Token = token
                                    });
                                }
                                else{return Results.Unauthorized();}
                            }).AllowAnonymous().WithTags("Administrador");


                            endpoints.MapPost("/Administradores/Cadastro", ([FromBody] AdministradorDTO administradorDTO, IAdministradorService administradorService) => {
                                var validacao = new ErrosDeValidacao{
                                    Mensagens = new List<string>()
                                };
                                if(string.IsNullOrEmpty(administradorDTO.Email))
                                    validacao.Mensagens.Add("Email não pode estar em branco");
                                if(string.IsNullOrEmpty(administradorDTO.Password))
                                    validacao.Mensagens.Add("Senha não pode estar em branco");
                                if(string.IsNullOrEmpty(administradorDTO.Perfil.ToString()))
                                    validacao.Mensagens.Add("Perfil não pode estar em branco");
                                if(validacao.Mensagens.Count()>0)
                                    return Results.BadRequest(validacao.Mensagens);
                                if(administradorDTO.Perfil!= Perfil.Adm && administradorDTO.Perfil!= Perfil.Editor){
                                    administradorDTO.Perfil = 0;
                                }
                                var administrador = new Administrador(administradorDTO.Email,administradorDTO.Password,administradorDTO.Perfil.ToString());
                                administradorService.Incluir(administrador);
                                return Results.Ok(administrador);
                            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm"}).WithTags("Administrador");

                            endpoints.MapGet("/Administradores/Todos",([FromQuery]int? pagina, IAdministradorService administradorService)=>{
                                var adms = administradorService.Todos(pagina);
                                if(adms==null){return Results.NotFound();}
                                return Results.Ok(adms);

                            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm"} ).WithTags("Administrador");

                            endpoints.MapGet("/Administradores/BuscaPorId",([FromQuery] int id, IAdministradorService administradorService)=>{
                                var adm = administradorService.BuscarPorId(id);
                                if(adm==null){return Results.NotFound();}
                                return Results.Ok(adm);
                            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm"}).WithTags("Administrador");

                            #endregion

                            #region Veiculos 

                            endpoints.MapPost("/Veiculo/Cadastro",([FromBodyAttribute]VeiculoDTO veiculoDTO, IVeiculoService veiculoService) =>{
                                var validacao = validaDTO(veiculoDTO);
                                    if(validacao.Mensagens.Count()>0)
                                        return Results.BadRequest(validacao);
                                
                                
                                    var veiculo = new Veiculo{
                                    Nome = veiculoDTO.Nome,
                                    Marca = veiculoDTO.Marca,
                                    Ano = veiculoDTO.Ano};
                                    veiculoService.Incluir(veiculo);
                                    return Results.Created($"/veiculo/{veiculo.Id}",veiculo);
                                }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor"}).WithTags("Veiculo");
                                

                            endpoints.MapGet("/Veiculo/Todos",([FromQuery]int? pagina, IVeiculoService veiculoService)=>{
                                var veiculos = veiculoService.Todos(pagina);
                                if(veiculos==null){return Results.NotFound();}
                                return Results.Ok(veiculos);
                            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor"}).WithTags("Veiculo");


                            endpoints.MapGet("/Veiculo/BuscaPorId",([FromQuery]int id, IVeiculoService veiculoService)=>{
                                var veiculo = veiculoService.BuscaPorId(id);
                                if(veiculo==null){return Results.NotFound();}
                                return Results.Ok(veiculo);
                            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor"}).WithTags("Veiculo");


                            endpoints.MapPut("/Veiculo/Atualizar",([FromBodyAttribute]VeiculoDTO veiculoDTO,[FromQuery]int id, IVeiculoService veiculoService)=>{
                                var validacao = validaDTO(veiculoDTO);
                                    if(validacao.Mensagens.Count()>0)
                                        return Results.BadRequest(validacao);
                                var veiculo = new Veiculo{
                                    Id = id,
                                    Nome = veiculoDTO.Nome,
                                    Marca = veiculoDTO.Marca,
                                    Ano = veiculoDTO.Ano};        
                                veiculoService.Atualizar(veiculo);
                                return Results.Ok(veiculo);
                            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm"}).WithTags("Veiculo");

                            endpoints.MapDelete("/Veiculo/Apagar",([FromBody]Veiculo veiculo, IVeiculoService veiculoService)=>{
                                veiculoService.Apagar(veiculo);
                                return Results.Ok(veiculo);
                            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Adm"}).WithTags("Veiculo");
                            #endregion
            });
        }
    }
}