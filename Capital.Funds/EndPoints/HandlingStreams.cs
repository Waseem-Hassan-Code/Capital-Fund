using Capital.Funds.Models.DTO;
using Capital.Funds.Services.IServices;
using Capital.Funds.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Capital.Funds.EndPoints
{
    public static class HandlingStreamsExtensions
    {
        public static void ConfigureStreamsEndPoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("api/getComplaintImage", GetComplaintImage)
                .RequireAuthorization("AdminOnly")
                .WithName("GetComplaintImage")
                .Produces<FileContentResult>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest);
        }

        public static async Task<IResult> GetComplaintImage(
            [FromServices] ITenantsComplains _complains,
            [FromQuery] string complaintId)
        {
            try
            {
                ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };
                string stream = await _complains.GetUserComplaintImageAsync(complaintId);

               
                if (stream == null)
                {
                    responseDto.Results = stream;
                    return Results.NotFound(responseDto);
                }

                if (_complains.LastException != null)
                {
                    responseDto.StatusCode = 500;
                    responseDto.Message = "Internal Server Error.";
                    return Results.BadRequest(responseDto);
                }

                responseDto.IsSuccess=true;
                responseDto.StatusCode = 200;
                responseDto.Message = "Image retrived.";
                responseDto.Results = stream;
                return Results.Ok(responseDto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest("Internal Server Error: " + ex.Message);
            }
        }
    }
}
