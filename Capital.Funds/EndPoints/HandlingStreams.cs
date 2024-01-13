using Capital.Funds.Models.DTO;
using Capital.Funds.Services.IServices;
using Capital.Funds.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Capital.Funds.EndPoints
{
    public static class HandlingStreamsExtensions
    {
        public static void ConfigureStreamsEndPoints(this WebApplication app)
        {
            app.MapGet("api/getComplaintImage", GetComplaintImage).WithName("GetComplaintImage")
                .Produces<ResponseDto>(200).Produces(400);
        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> GetComplaintImage(ITenantsComplains _complains, [FromQuery] string complaintId)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            byte[] stream = await _complains.GetUserComplaintImageAsync(complaintId);
            if (stream.IsNullOrEmpty())
            {
                responseDto.Message = "Image not found.";
                return Results.BadRequest(responseDto);
            }

            if (!string.IsNullOrEmpty(_complains.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _complains.LastException;
                return Results.BadRequest(responseDto);
            }

            var fileExtension = ".jpg";
            var contentType = SD.GetContentTypeDynamic(fileExtension);
            FileContentResult file = new FileContentResult(stream, contentType)
            {
                FileDownloadName = $"ComplainId_{Guid.NewGuid()}"
            };

            responseDto.Results = file;
            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Message = "Image retrieved successfully.";

            return Results.Ok(responseDto);
        }

    }
}
