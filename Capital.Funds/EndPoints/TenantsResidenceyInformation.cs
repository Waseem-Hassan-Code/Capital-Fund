using Capital.Funds.Models;
using Capital.Funds.Models.DTO;
using Capital.Funds.Services.IServices;
using Capital.Funds.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace Capital.Funds.EndPoints
{
    public static class TenantsResidenceyInformation
    {
        public static void ConfigureTenantsResidencyInfo(this WebApplication app)
        {
            app.MapGet("/api/getAllContract", getAllContracts).WithName("GetAllContracts")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapPost("/api/addNewContract", AssigningProperty).WithName("AddNewContract").Accepts<TenatDetails>("application/json")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapPost("/api/updateContract", update_Contract).WithName("UpdateContract").Accepts<TenatDetails>("application/json")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapGet("/api/deleteContract", deleteAssignedProperty).WithName("DeleteContract")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapGet("/api/getContractyId", getContractById).WithName("GetContractById")
           .Produces<ResponseDto>(200).Produces(400);
        }

        [Authorize(Policy = "AdminOnly")]
        public static async Task<IResult> AssigningProperty(ITenatsResidencyInfo _info, [FromBody] TenatDetails _details)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            TenatDetails newTenant = await _info.assignPropertyAsync(_details);
            if (newTenant == null)
            {
                responseDto.StatusCode = 400;
                responseDto.Message = SD.RecordNotUpdated;
                return Results.BadRequest(responseDto);
            }

            if (!string.IsNullOrEmpty(_info.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _info.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = newTenant;
            responseDto.Message = SD.RecordUpdated;
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "AdminOnly")]
        public static async Task<IResult> deleteAssignedProperty(ITenatsResidencyInfo _info, [FromQuery] string recordId)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };


            var record = await _info.deleteAssignedProperty(recordId);
            if (record == SD.RecordNotUpdated)
            {
                responseDto.StatusCode = 400;
                responseDto.Message = SD.RecordNotUpdated;
                return Results.BadRequest(responseDto);
            }

            if (!string.IsNullOrEmpty(_info.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _info.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = "";
            responseDto.Message = SD.RecordUpdated;
            return Results.Ok(responseDto);

        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> getAllContracts(ITenatsResidencyInfo _info, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            PaginatedResult<TenatDetails> propertyList = await _info.getAllContracts(page, pageSize);
            if (propertyList==null)
            {
                responseDto.StatusCode = 400;
                responseDto.Message = "Data not found";
                return Results.BadRequest(responseDto);
            }

            if (!string.IsNullOrEmpty(_info.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _info.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = propertyList;
            responseDto.Message = "Record retrived successfully.";
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> update_Contract(ITenatsResidencyInfo _info, [FromBody] TenatDetails property)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            TenatDetails updateDetails = await _info.updateAssignedPropertyAsync(property);

            if (updateDetails == null)
            {
                responseDto.StatusCode = 400;
                responseDto.Message = SD.RecordNotUpdated;
                return Results.BadRequest(responseDto);
            }


            if (!string.IsNullOrEmpty(_info.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _info.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = updateDetails;
            responseDto.Message = SD.RecordUpdated;
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> getContractById(ITenatsResidencyInfo _info, [FromQuery] string propertyId)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            TenatDetails property = await _info.getByIdAsync(propertyId);

            if (property.PropertyId.IsNullOrEmpty() || property.UserId.IsNullOrEmpty())
            {
                responseDto.Message = SD.UserNotFound;
                return Results.BadRequest(responseDto);
            }

            if (!string.IsNullOrEmpty(_info.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _info.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = property;
            responseDto.Message = "Record retrived successfully.";
            return Results.Ok(responseDto);
        }
    }
}
