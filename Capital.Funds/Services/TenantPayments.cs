using AutoMapper;
using Capital.Funds.Database;
using Capital.Funds.Models;
using Capital.Funds.Services.IServices;
using Capital.Funds.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Capital.Funds.Services
{
    public class TenantPayment : ITenantPayments
    {
        private readonly ApplicationDb _db;
        private readonly IMapper _mapper;

        public TenantPayment(ApplicationDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<Models.TenantPayments> addTenantPaymentAsync(Models.TenantPayments tenantPayments)
        {
            try
            {
                Models.TenantPayments payments = new Models.TenantPayments()
                {
                   Id = Guid.NewGuid().ToString(),
                   TenantId = tenantPayments.TenantId,
                   Rent = tenantPayments.Rent,
                   AreaMaintainienceFee = tenantPayments.AreaMaintainienceFee,
                   isLate = tenantPayments.isLate,
                   LateFee = tenantPayments.LateFee,
                   RentPayedAt = tenantPayments.RentPayedAt,
                   Month = tenantPayments.Month,
                   CreatedAt = DateTime.Now,
                   ModifiedAt = DateTime.Now,
                };

                await _db.TenantPayments.AddAsync(payments);
                int count = await _db.SaveChangesAsync();

                if (count > 0)
                    return payments;

                return payments;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> deleteTenantPaymentAsync(string paymentId)
        {
            try
            {
                var payment = await _db.TenantPayments.FirstOrDefaultAsync(p => p.Id==paymentId);
                if(payment != null)
                {
                     _db.TenantPayments.Remove(payment);
                     int row = await _db.SaveChangesAsync();
                    if (row>0)
                        return SD.RecordUpdated;
                      
                }
                return SD.RecordNotUpdated;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PaginatedResult<Models.TenantPayments>> getAllTenatPaymentsAsync(int page, int pageSize)
        {
            try
            {
                var totalCount = await _db.TenantPayments.CountAsync();
                var details = await _db.TenantPayments
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (details!=null)
                {
                    IEnumerable<Models.TenantPayments> paymentList = _mapper.Map<IEnumerable<Models.TenantPayments>>(details);
                    var paginatedResults = new PaginatedResult<Models.TenantPayments>
                    {
                        Items = paymentList,
                        TotalCount = totalCount,
                        PageSize = pageSize,
                        Page = page
                    };

                    return paginatedResults;
                }

                return new PaginatedResult<Models.TenantPayments>
                {
                    Items = Enumerable.Empty<Models.TenantPayments>(),
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize
                };

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PaginatedResult<Models.TenantPayments>> getTenantPaymentByIdAsync(int page, int pageSize, string TenantId)
        {
            try
            {
                var totalCount = await _db.TenantPayments.CountAsync();
                var details = await _db.TenantPayments
                    .Where(p=>p.TenantId == TenantId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (details!=null)
                {
                    IEnumerable<Models.TenantPayments> paymentList = _mapper.Map<IEnumerable<Models.TenantPayments>>(details);
                    var paginatedResults = new PaginatedResult<Models.TenantPayments>
                    {
                        Items = paymentList,
                        TotalCount = totalCount,
                        PageSize = pageSize,
                        Page = page
                    };

                    return paginatedResults;
                }

                return new PaginatedResult<Models.TenantPayments>
                {
                    Items = Enumerable.Empty<Models.TenantPayments>(),
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize
                };

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Models.TenantPayments> updateTenantPaymentAsyn(Models.TenantPayments tenantPayments)
        {
            try
            {
                Models.TenantPayments payments = new Models.TenantPayments()
                {
                    Id = Guid.NewGuid().ToString(),
                    TenantId = tenantPayments.TenantId,
                    Rent = tenantPayments.Rent,
                    AreaMaintainienceFee = tenantPayments.AreaMaintainienceFee,
                    isLate = tenantPayments.isLate,
                    LateFee = tenantPayments.LateFee,
                    RentPayedAt = tenantPayments.RentPayedAt,
                    Month = tenantPayments.Month,
                    ModifiedAt = DateTime.Now,
                };

                int count = await _db.SaveChangesAsync();

                if (count > 0)
                    return payments;

                return payments;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
