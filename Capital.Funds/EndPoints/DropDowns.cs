using Capital.Funds.Models;
using Capital.Funds.Models.DTO;
using Capital.Funds.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Capital.Funds.EndPoints
{
    public static class DropDowns
    {
        public static void ConfigureDropDownsEndPoints(this WebApplication app)
        {

            app.MapGet("/api/dropDownTenantName", DropDownTenantName).WithName("DropDownTenantName")
             .Produces<ResponseDto>(200).Produces(400);

            app.MapGet("/api/dropDownUserName", DropDownUserName).WithName("DropDownUserName")
             .Produces<ResponseDto>(200).Produces(400);

            app.MapGet("/api/dropDownPropertyName", DropDownPropertyName).WithName("DropDownPropertyName")
             .Produces<ResponseDto>(200).Produces(400);
        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> DropDownTenantName(IDropDownLists _ddl)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            IEnumerable<DDLTenantName> list = await _ddl.DDLTenantNames();
            if (list==null)
            {
                responseDto.Message = "Data not found";
                return Results.Ok(responseDto);
            }

            if (!string.IsNullOrEmpty(_ddl.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _ddl.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = list;
            responseDto.Message = "Record retrived successfully.";
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> DropDownUserName(IDropDownLists _ddl)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            IEnumerable<DDLUserName> list = await _ddl.DDLUserNames();
            if (list==null)
            {
                responseDto.Message = "Data not found";
                return Results.Ok(responseDto);
            }

            if (!string.IsNullOrEmpty(_ddl.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _ddl.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = list;
            responseDto.Message = "Record retrived successfully.";
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> DropDownPropertyName(IDropDownLists _ddl)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            IEnumerable<DDLPropertyName> list = await _ddl.DDLPropertyNames();
            if (list==null)
            {
                responseDto.Message = "Data not found";
                return Results.Ok(responseDto);
            }

            if (!string.IsNullOrEmpty(_ddl.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _ddl.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = list;
            responseDto.Message = "Record retrived successfully.";
            return Results.Ok(responseDto);
        }
    }
}
