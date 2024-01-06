using Capital.Funds.Models;
using Capital.Funds.Models.DTO;
using Capital.Funds.Services.IServices;
using Capital.Funds.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Capital.Funds.EndPoints
{
    public static class TenantPaymentsMangment
    {

        public static void ConfigureTenantsPaymentsInfo(this WebApplication app)
        {
            app.MapPost("/api/addNewPayment", addTenantPayment).WithName("AddNewPayment")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapGet("/api/deletePayment", deletePayment).WithName("DeletePayment").Accepts<string>("application/json")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapGet("/api/getAllPayments", getAllPayment).WithName("GetAllPayments")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapPost("/api/updatePayments", updatePayment).WithName("UpdatePayments")
            .Produces<ResponseDto>(200).Produces(400);

            app.MapGet("/api/getPaymentById", getPaymentById).WithName("GetPaymentById")
           .Produces<ResponseDto>(200).Produces(400);
        }

        [Authorize(Policy = "AdminOnly")]
        public static async Task<IResult> addTenantPayment(ITenantPayments _pay, [FromBody] TenantPayments _details)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            TenantPayments payment = await _pay.addTenantPaymentAsync(_details);
            if (payment == null)
            {
                responseDto.StatusCode = 400;
                responseDto.Message = SD.RecordNotUpdated;
                return Results.BadRequest(responseDto);
            }

            if (!string.IsNullOrEmpty(_pay.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _pay.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = payment;
            responseDto.Message = SD.RecordUpdated;
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "AdminOnly")]
        public static async Task<IResult> deletePayment(ITenantPayments _pay, [FromQuery] string recordId)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };


            var record = await _pay.deleteTenantPaymentAsync(recordId);
            if (record == SD.RecordNotUpdated)
            {
                responseDto.StatusCode = 400;
                responseDto.Message = SD.RecordNotUpdated;
                return Results.BadRequest(responseDto);
            }

            if (!string.IsNullOrEmpty(_pay.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _pay.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = "";
            responseDto.Message = SD.RecordUpdated;
            return Results.Ok(responseDto);

        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> getAllPayment(ITenantPayments _pay, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            PaginatedResult<TenantPaymentsDto> paymentList = await _pay.getAllTenatPaymentsAsync(page, pageSize);
            if (paymentList.TotalCount==0)
            {
                responseDto.StatusCode = 200;
                responseDto.Message = "Data not found";
                return Results.Ok(responseDto);
            }

            if (!string.IsNullOrEmpty(_pay.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _pay.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = paymentList;
            responseDto.Message = "Record retrived successfully.";
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> updatePayment(ITenantPayments _pay, [FromBody] TenantPayments payments)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            TenantPayments updateDetails = await _pay.updateTenantPaymentAsyn(payments);

            if (updateDetails == null)
            {
                responseDto.StatusCode = 400;
                responseDto.Message = SD.RecordNotUpdated;
                return Results.BadRequest(responseDto);
            }


            if (!string.IsNullOrEmpty(_pay.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _pay.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = updateDetails;
            responseDto.Message = SD.RecordUpdated;
            return Results.Ok(responseDto);
        }

        [Authorize(Policy = "AdminOnly")]
        public async static Task<IResult> getPaymentById(ITenantPayments _pay, [FromQuery] string paymentId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            ResponseDto responseDto = new() { IsSuccess = false, StatusCode = 400, Message = "", Results = { } };

            PaginatedResult<TenantPayments> payment = await _pay.getTenantPaymentByIdAsync(page,pageSize, paymentId);

            if (payment.TotalCount==0)
            {
                responseDto.Message = "Data not found";
                return Results.BadRequest(responseDto);
            }

            if (!string.IsNullOrEmpty(_pay.LastException))
            {
                responseDto.StatusCode = 500;
                responseDto.Message = "Internal Server Error: " + _pay.LastException;
                return Results.BadRequest(responseDto);
            }

            responseDto.IsSuccess = true;
            responseDto.StatusCode = 200;
            responseDto.Results = payment;
            responseDto.Message = "Record retrived successfully.";
            return Results.Ok(responseDto);
        }
    }
}
