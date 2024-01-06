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
        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> DropDownTenantName(ITenantsComplains _complain)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            IEnumerable<DDLTenantName> list = await _complain.DDLTenantNames();
            if (list==null)
            {
                responseDto.Message = "Data not found";
                return Results.Ok(responseDto);
            }

            if (!string.IsNullOrEmpty(_complain.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _complain.LastException;
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
