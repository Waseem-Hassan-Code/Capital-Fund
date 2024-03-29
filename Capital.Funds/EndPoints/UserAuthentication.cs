﻿using Capital.Funds.Models.DTO;
using Capital.Funds.Services.IServices;
using Capital.Funds.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Net;

namespace Capital.Funds.EndPoints
{
    public static class UserAuthentication
    {
        
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {

            app.MapPost("/api/login", Login).WithName("Login").Accepts<LoginDto>("application/json")
                .Produces<LoginResponseDto>(200).Produces(400);

            app.MapPost("/api/register", Register).WithName("Register").Accepts<UserRegistrationDto>("application/json")
                .Produces<ResponseDto>(200).Produces(400);

            app.MapPost("/api/verifyEmail", VerifyEmail)
                .WithName("EmailVerification")
                .Accepts<EmailVerificationDto>("application/json")
                .Produces<ResponseDto>(200)
                .Produces(400);

            app.MapGet("/api/resendEmail", ResendEmail)
               .Produces<ResponseDto>(200)
               .Produces(400);

        }

        
        public static async Task<IResult> Login(IAuthService _auth,
           [FromBody] LoginDto model)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            var login =await _auth.Login(model);
            if (login.Token == SD.UserNotFound) {
                responseDto.Results = login;
               return Results.NotFound(responseDto);
            }

            if(login.Token == SD.IncorrectPassword) {
                responseDto.Results = login;
                return Results.Forbid();
            }

            if (login == null)
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error.";
                return Results.BadRequest(responseDto);
            }

            if(login.Token == "Account inactive.")
            {
                responseDto.StatusCode = 401;
                responseDto.Message = "Account inactive.";
                return Results.BadRequest(responseDto);
            }

            if (login.Token == "Email not verified.")
            {
                responseDto.StatusCode = 402;
                responseDto.Message = "Email not verified.";
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess=true;
            responseDto.StatusCode = 200;
            responseDto.Message = "User login success.";
            responseDto.Results = login.Token;
            return Results.Ok(responseDto);
          }

        
        public async static Task<IResult> Register(IAuthService _auth,[FromBody] UserRegistrationDto registrationDto)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            string Register = await _auth.Register(registrationDto);
            if(Register == SD.AlreadyRegistered)
            {
                responseDto.Message = SD.AlreadyRegistered;
                return Results.BadRequest(responseDto);
            }

            if(Register == SD.RegistrationFailed)
            {
                responseDto.Message = SD.RegistrationFailed;
                return Results.BadRequest(responseDto);
            }

            if (Register == null)
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error.";
                return Results.BadRequest(responseDto);
            }

            if(Register ==SD.InvalidEmailAddress)
            {
                responseDto.Message = SD.InvalidEmailAddress;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess=true;
            responseDto.StatusCode = 200;
            responseDto.Results = registrationDto;
            responseDto.Message = SD.RegistrationSuccess;
            return Results.Ok(responseDto);
        }

        
        public async static Task<IResult> ResendEmail(IAuthService _auth, [FromQuery] string email)
         {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            string verify = await _auth.ResendEmail(email);
            if (verify == SD.InvalidEmailAddress)
            {
                responseDto.Message = SD.InvalidEmailAddress;
                return Results.BadRequest(responseDto);
            }

            if (verify == SD.Unverified)
            {
                responseDto.Message = SD.Unverified;
                return Results.BadRequest(responseDto);
            }

            if (verify == null)
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error.";
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess=true;
            responseDto.StatusCode = 200;
            responseDto.Results = "";
            responseDto.Message = "A 6 digit otp is sent to your email address";
            return Results.Ok(responseDto);
        }

        
        public async static Task<IResult> VerifyEmail(IAuthService _auth, [FromBody] EmailVerificationDto emailVerificationDto)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            string verify = await _auth.VerifyEmail(emailVerificationDto);
            if (verify == SD.IncorrectOtp)
            {
                responseDto.Message = SD.IncorrectOtp;
                return Results.BadRequest(responseDto);
            }

            if (verify == SD.InvalidEmailAddress)
            {
                responseDto.Message = SD.InvalidEmailAddress;
                return Results.BadRequest(responseDto);
            }

            if (verify == SD.Unverified)
            {
                responseDto.Message = SD.Unverified;
                return Results.BadRequest(responseDto);
            }

            if (verify == null)
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error.";
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess=true;
            responseDto.StatusCode = 200;
            responseDto.Results = "";
            responseDto.Message = SD.EmailVerified;
            return Results.Ok(responseDto);
        }

    } 
}
