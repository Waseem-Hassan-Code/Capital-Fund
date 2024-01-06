using Capital.Funds.Models.DTO;
using Capital.Funds.Models;
using Capital.Funds.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Capital.Funds.Utils;

namespace Capital.Funds.EndPoints
{
    public static class TenantsComplainsManagment
    {
        public static void ConfigureComplainsEndPoints(this WebApplication app)
        {

            app.MapGet("/api/removeComplain", RemoveComplain).WithName("RemoveTenantComplain").Accepts<TenantComplaints>("application/json")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapPost("/api/updateComplain", updateComplain).WithName("UpdateTenantComplain").Accepts<Users>("application/json")
                .Produces<ResponseDto>(200).Produces(400);

            app.MapGet("/api/getAllComplains", getAllComplains)
                .WithName("GetAllComplains")
                .Produces<ResponseDto>(200)
                .Produces(400);
        }

            [Authorize(Policy = "AdminOnly")]
            public async static Task<IResult> RemoveComplain(ITenantsComplains _complains, [FromQuery] string complainId)
            {
                ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

                bool status = await _complains.RemoveComplainAsync(complainId);
                if (status == false)
                {
                    responseDto.Message = SD.RecordNotUpdated;
                    return Results.BadRequest(responseDto);
                }

            if (!string.IsNullOrEmpty(_complains.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _complains.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
                responseDto.StatusCode = 200;
                responseDto.Results = "";
                responseDto.Message = SD.RegistrationSuccess;
                return Results.Ok(responseDto);
            }

            [Authorize(Policy = "AdminOnly")]
            public async static Task<IResult> updateComplain(ITenantsComplains _complains, [FromQuery] string complainId)
            {
                ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

                bool update = await _complains.ChangeStatusAsync(complainId);
                if (update == false)
                {
                    responseDto.Message = SD.UserNotFound;
                    return Results.BadRequest(responseDto);
                }

            if (!string.IsNullOrEmpty(_complains.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _complains.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
                responseDto.StatusCode = 200;
                responseDto.Results = update;
                responseDto.Message = SD.RecordUpdated;
                return Results.Ok(responseDto);
            }

            [Authorize(Policy = "AdminOnly")]
            public async static Task<IResult> getAllComplains(ITenantsComplains _complains, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
            {
                ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

                PaginatedResult<ComplaintsDTO> complainsList = await _complains.GetTenantsComplainsAsync(page, pageSize);
                if (complainsList.Items.IsNullOrEmpty())
                {
                    responseDto.Message = "Data not found";
                    return Results.BadRequest(responseDto);
                }

            if (!string.IsNullOrEmpty(_complains.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _complains.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
                responseDto.StatusCode = 200;
                responseDto.Results = complainsList;
                responseDto.Message = "Record retrived successfully.";
                return Results.Ok(responseDto);
            }

        }
}
