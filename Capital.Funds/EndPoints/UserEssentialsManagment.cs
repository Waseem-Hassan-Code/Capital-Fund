using Capital.Funds.Models.DTO;
using Capital.Funds.Models;
using Capital.Funds.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Capital.Funds.Utils;

namespace Capital.Funds.EndPoints
{
    public static class UserEssentialsManagment
    {
        public static void ConfigureUserEssentialsEndPoints(this WebApplication app)
        {

            app.MapGet("/api/getMonthlyFair", getMonthlyRent).WithName("GetMontlyFair")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapGet("/api/paymentsHistory", getPaymentsHistory).WithName("PaymentsHistory")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapGet("/api/getComplaints", getAllComplaints).WithName("GetComplaints")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapPost("/api/newComplaint", addNewComplaint).WithName("NewComplait").Accepts<TenantComplaints>("application/json")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapGet("/api/getTenantId", getTenantId).WithName("GetTenantId")
             .Produces<ResponseDto>(200).Produces(400);

        }

        [Authorize(Policy = "UserOnly")]
        public async static Task<IResult> getMonthlyRent(IUserEssentials _manage, [FromQuery] string userId)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            var result = await _manage.getMontlyRentAsync(userId);
            if (result==null)
            {
                responseDto.StatusCode = 203;
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
            responseDto.Results = result;
            responseDto.Message = "Record retrived successfully.";
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "UserOnly")]
        public async static Task<IResult> getPaymentsHistory(IUserEssentials _manage, [FromQuery] string userId, [FromQuery] int page,[FromQuery] int pageSize)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            PaginatedResult<TenantPayments> result = await _manage.getPaymentsHistoryAsync(userId,page, pageSize);
            if (result==null)
            {
                responseDto.StatusCode = 203;
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
            responseDto.Results = result;
            responseDto.Message = "Record retrived successfully.";
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "UserOnly")]
        public async static Task<IResult> getAllComplaints(IUserEssentials _manage, [FromQuery] string userId)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            IEnumerable<TenantComplaints> result = await _manage.getComplaintsAsync(userId);
            if (result==null)
            {
                responseDto.StatusCode = 203;
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
            responseDto.Results = result;
            responseDto.Message = "Record retrived successfully.";
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "UserOnly")]
        public async static Task<IResult> addNewComplaint(IUserEssentials _manage, [FromBody] TenantComplaints compaint)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            var result = await _manage.addComplaintAsync(compaint);
            if (result==SD.RecordNotUpdated)
            {
                responseDto.StatusCode = 400;
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
            responseDto.Results = result;
            responseDto.Message = SD.RecordUpdated;
            return Results.Ok(responseDto);
        }

        public async static Task<IResult> getTenantId(IUserEssentials _manage, [FromQuery] string userId)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            var result = await _manage.getTenantIdAsync(userId);
            if (result==null)
            {
                responseDto.StatusCode = 200;
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
            responseDto.Results = result;
            responseDto.Message = "Tenant Id Retrived.";
            return Results.Ok(responseDto);
        }
    }
}
