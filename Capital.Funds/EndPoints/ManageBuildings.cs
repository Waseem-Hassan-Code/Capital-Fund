using Capital.Funds.Models.DTO;
using Capital.Funds.Models;
using Capital.Funds.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Capital.Funds.Utils;

namespace Capital.Funds.EndPoints
{
    public static class ManageBuildingsData
    {
        public static void ConfigureManageBuildingsEndPoints(this WebApplication app)
        {

            app.MapGet("/api/getAllProperties", getAllProperties).WithName("GetAllProperties")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapPost("/api/addNewProperty", AddProperty).WithName("AddProperty").Accepts<PropertyDetails>("application/json")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapPost("/api/updateProperty", updateProperty).WithName("UpdateProperty").Accepts<PropertyDetails>("application/json")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapGet("/api/deleteProperty", deleteProperty).WithName("DeleteProperty")
            .Produces<ResponseDto>(200).Produces(400);
         
            app.MapGet("/api/getPropertyById", getBuildingById).WithName("GetPropertyById")
           .Produces<ResponseDto>(200).Produces(400);

        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> getAllProperties(IBuildingManagment _manage, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            PaginatedResult<PropertyDetails> propertyList = await _manage.getPropertyAsync(page, pageSize);
            if (propertyList==null)
            {
                responseDto.Message = "Data not found";
                return Results.Ok(responseDto);
            }

            if (!string.IsNullOrEmpty(_manage.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _manage.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = propertyList;
            responseDto.Message = "Record retrived successfully.";
            return Results.Ok(responseDto);
        }


        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> AddProperty(IBuildingManagment _manage, [FromBody] PropertyDetails property)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            string newProperty = await _manage.createPropertyAsync(property);
            if (newProperty == SD.RecordNotUpdated)
            {
                responseDto.Message = SD.RecordNotUpdated;
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
            responseDto.Results = newProperty;
            responseDto.Message = SD.RecordUpdated;
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> updateProperty(IBuildingManagment _manage, [FromBody] PropertyDetails property)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            string update = await _manage.updatePropertyAsync(property);

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
        public async static Task<IResult> deleteProperty(IBuildingManagment _manage, [FromQuery] string propertyId)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            string delete = await _manage.removePropertyAsync(propertyId);

            if (delete == SD.UserNotFound)
            {
                responseDto.Message = SD.UserNotFound;
                return Results.BadRequest(responseDto);
            }

            if (delete == SD.RecordNotUpdated)
            {
                responseDto.Message = SD.RecordNotUpdated;
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
            responseDto.Results = delete;
            responseDto.Message = SD.RecordUpdated;
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> getBuildingById(IBuildingManagment _manage, [FromQuery] string propertyId)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            PropertyDetails property = await _manage.getPropertyByIdAsync(propertyId);

            if (property.PropertyName.IsNullOrEmpty() || property.Id.IsNullOrEmpty())
            {
                responseDto.Message = SD.UserNotFound;
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
            responseDto.Results = property;
            responseDto.Message = "Record retrived successfully.";
            return Results.Ok(responseDto);
        }
    }
}
