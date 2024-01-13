using Capital.Funds.Database;
using Quartz;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Capital.Funds.Models;
using Microsoft.EntityFrameworkCore;

namespace Capital.Funds.ScheduledJobs
{
    public class TriggerPayments : IJob
    {
        private readonly ILogger<TriggerPayments> _logger;
        private readonly ApplicationDb _db;

        public TriggerPayments(ILogger<TriggerPayments> logger, ApplicationDb db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation("TriggerPayments job started.");

                if (context.JobDetail.Key.Name.EndsWith("_MonthlyPayments"))
                {
                    await TriggerMonthlyPayments();
                }
                else if (context.JobDetail.Key.Name.EndsWith("_LateFee"))
                {
                    await TriggerLateFee();
                }

               // _logger.LogInformation("TriggerPayments job completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in TriggerPayments job.", ex);
            }
        }

        private string TestFunction()
        {
            int count = 0;
           return $"Triggering info {count++}";
        }

        private async Task TriggerMonthlyPayments()
        {
            try
            {
                using (var transaction = await _db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        DateTime currentDate = DateTime.Now;
                        string currentMonth = currentDate.ToString("MMMM");

                        var tenantsResidencyInfo = await _db.TenatDetails.ToListAsync();
                        List<Models.TenantPayments> paymentsList = new List<Models.TenantPayments>();

                        foreach (var tenant in tenantsResidencyInfo)
                        {
                            Models.TenantPayments payments = new Models.TenantPayments()
                            {
                                Id = Guid.NewGuid().ToString("N"),
                                TenantId = tenant.Id,
                                Rent = tenant.RentPerMonth,
                                AreaMaintainienceFee = 0,
                                isLate = false,
                                LateFee = 0,
                                RentPayedAt = "",
                                Month = currentMonth,
                                CreatedAt = DateTime.Now,
                                ModifiedAt = DateTime.Now,
                                isPayable = true,
                            };
                            paymentsList.Add(payments);
                        }

                        await _db.TenantPayments.AddRangeAsync(paymentsList);
                        await _db.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in TriggerMonthlyPayments.", ex);
                throw new Exception("Error in TriggerMonthlyPayments.", ex);
            }
        }

        private async Task TriggerLateFee()
        {
            try
            {
                using (var transaction = await _db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        DateTime currentDate = DateTime.Now;

                        var tenantsLateFee = await _db.TenantPayments
                            .Where(t => t.isLate == true && t.RentPayedAt == "")
                            .ToListAsync();

                        foreach (var tenant in tenantsLateFee)
                        {
                            tenant.isLate = true;
                            tenant.LateFee = 50;
                            tenant.ModifiedAt = DateTime.Now;
                            tenant.isPayable = true;
                        }

                        await _db.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in TriggerLateFee.", ex);
                throw new Exception("Error in TriggerLateFee.", ex);
            }
        }
    }

}
