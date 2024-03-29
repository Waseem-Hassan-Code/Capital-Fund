﻿using Capital.Funds.Models;
using Capital.Funds.Models.DTO;
using Capital.Funds.Services.IServices;
using Capital.Funds.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Capital.Funds.EndPoints
{
    public static class ManageTenantsData
    {
        public static void ConfigureManageEmployeeEndPoints(this WebApplication app)
        {

            app.MapPost("/api/updateTenantInfo", updateTenantsInfo).WithName("UpdateTenant").Accepts<TenantPersonalInfoDto>("application/json")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapPost("/api/addNewTenant", AddUser).WithName("AddTenant").Accepts<Users>("application/json")
                .Produces<ResponseDto>(200).Produces(400);

            app.MapGet("/api/getAllTenants", getAllTenants)
                .WithName("GetAllTenants")
                .Produces<ResponseDto>(200)
                .Produces(400);

            app.MapGet("/api/deleteTenantInfo", deleteTenantInfo)
               .WithName("DeleteTenantInfo")
               .Produces<ResponseDto>(200)
               .Produces(400);

        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> AddUser(IManageTenants _manage, [FromBody] Users user)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            string Register = await _manage.addNewTenantAsync(user);
            if (Register == SD.AlreadyRegistered)
            {
                responseDto.Message = SD.AlreadyRegistered;
                return Results.BadRequest(responseDto);
            }

            if (Register == SD.RecordNotUpdated)
            {
                responseDto.Message = SD.RegistrationFailed;
                return Results.BadRequest(responseDto);
            }

            if (!string.IsNullOrEmpty(_manage.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _manage.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = user;
            responseDto.Message = SD.RegistrationSuccess;
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> updateTenantsInfo(IManageTenants _manage, [FromBody] TenantPersonalInfoDto personalInfo)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            string update = await _manage.updateTenantsPersonalInfoAsync(personalInfo);
            if (update == SD.UserNotFound)
            {
                responseDto.Message = SD.UserNotFound;
                return Results.BadRequest(responseDto);
            }

            if (update == SD.RecordNotUpdated)
            {
                responseDto.Message = SD.RegistrationFailed;
                return Results.BadRequest(responseDto);
            }

            if (!string.IsNullOrEmpty(_manage.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _manage.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = update;
            responseDto.Message = SD.RecordUpdated;
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> getAllTenants(IManageTenants _manage, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            PaginatedResult<TenantPersonalInfoDto> tenantsList = await _manage.getAllTenantsAsync(page,pageSize);
            if (tenantsList.Items.IsNullOrEmpty())
            {
                responseDto.Message = "Data not found";
                return Results.BadRequest(responseDto);
            }

            if (!string.IsNullOrEmpty(_manage.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _manage.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = tenantsList;
            responseDto.Message = "Record retrived successfully.";
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> deleteTenantInfo(IManageTenants _manage, [FromQuery] string Id)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            bool check = await _manage.DeleteTenantAsync(Id);
            if (check == false)
            {
                responseDto.Message = "Record not deleted.";
                return Results.BadRequest(responseDto);
            }

            if (!string.IsNullOrEmpty(_manage.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _manage.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = check;
            responseDto.Message = SD.RecordUpdated;
            return Results.Ok(responseDto);
        }
    }
}
