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
                .Produces(StatusCodes.Status200OK, typeof(Stream))
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest);

        }

        public static async Task<IResult> GetComplaintImage(
            [FromServices] ITenantsComplains _complains,
            [FromQuery] string complaintId)
        {
            try
            {
                var fileStream = await _complains.GetUserComplaintImageAsync(complaintId);

                if (fileStream == null)
                {
                    return Results.NotFound("File not found");
                }

                var fileName = "default_filename.jpg"; 

                var fileDetails = await _complains.FileDetails(complaintId);

                if (fileStream != null)
                {
                    fileName = fileDetails.FileName; 
                }

                var contentType = ContentTypeHelper.GetContentType(fileName);

                return Results.File(fileStream, contentType, fileName); 
            }
            catch (FileNotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (IOException ex)
            {
                return Results.BadRequest("Error reading file stream: " + ex.Message);
            }
            catch (Exception ex)
            {
                return Results.BadRequest("Internal Server Error: " + ex.Message);
            }
        }

    }
}
