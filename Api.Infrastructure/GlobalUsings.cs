// SYSTEM
global using System.Text;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Linq.Expressions;
global using Swashbuckle.AspNetCore.SwaggerUI;

// MICROSOFT ASPNETCORE
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.ResponseCompression;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.AspNetCore.SignalR.Client;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.OpenApi.Models;

// INFRASTRUCTURE
global using Api.Infrastructure.AutoMapper;
global using Api.Infrastructure.Jwt;
global using Api.Infrastructure.Services;
global using Api.Infrastructure.SignalR;
global using Api.Infrastructure.Identity;
global using Api.Infrastructure.Repository;

// SHARED
global using Api.Shared.Data;
global using Api.Shared.Models;
global using Api.Shared.DTOs.Auth;
global using Api.Shared.DTOs.Identity.AspNetUsers;
global using Api.Shared.DTOs.Notifications;
global using Api.Shared.DTOs.Politics;
global using Api.Shared.DTOs.Emails;
global using Api.Shared.DTOs.Help;

// EMAILS LIBRARIES
global using MimeKit;
global using MailKit.Security;
global using RazorEngineCore;

// AUTOMAPPER
global using AutoMapper;

// IMAGE SHARP
global using SixLabors.ImageSharp;
global using SixLabors.ImageSharp.Formats.Jpeg;
global using SixLabors.ImageSharp.PixelFormats;

