using AutoMapper;
using Capital.Funds.Database;
using Capital.Funds.Models;
using Capital.Funds.Models.DTO;
using Capital.Funds.Services.IServices;
using Capital.Funds.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Capital.Funds.Services
{
    public class TenantsComplains : ITenantsComplains
    {
        private readonly ApplicationDb _db;
        private readonly IMapper _mapper;
        private readonly FileHandling _fileHandling;
        public string LastException { get; private set; }

        public TenantsComplains(ApplicationDb db, IMapper mapper, FileHandling fileHandling)
        {
            _db = db;
            _mapper = mapper;
            LastException = null;
            _fileHandling = fileHandling;
        }

        public async Task<bool> RemoveComplainAsync(string complainId)
        {
            try
            {
                LastException = null;

                using (var transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        LastException = null;
                        string sqlQuery = $"DELETE FROM TenantComplaints WHERE Id = @ComplainId";
                        int rowsAffected = await _db.Database.ExecuteSqlRawAsync(sqlQuery, new SqliteParameter("@ComplainId", complainId));

                        if (rowsAffected == 0)
                            return false;

                        string fileId = await _db.ComplaintFiles
                            .Where(c => c.ComplaintId == complainId)
                            .Select(u => u.FileURL)
                            .FirstOrDefaultAsync();

                        bool task = await _fileHandling.DeleteImage(fileId);

                        if (task)
                        {
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during transaction: {ex.Message}");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error outside transaction: {ex.Message}");
                return false;
            }

        }
        public async Task<bool> ChangeStatusAsync(UpdateComplaintStatusDto updateComplaintStatusDto)
        {
            try
            {
                DateTime date = DateTime.Now;
                LastException = null;
                string sqlQuery = $"UPDATE TenantComplaints SET IsFixed = @status, UpdatedAt = @currentDate WHERE Id = @ComplainId";
                int rowsAffected = await _db.Database.ExecuteSqlRawAsync(sqlQuery, new SqliteParameter("@ComplainId", updateComplaintStatusDto.Id)
                    ,  new SqliteParameter("@status", updateComplaintStatusDto.Status),
                    new SqliteParameter("@currentDate", date));

                if (rowsAffected == 0)
                    return false;
                return true;

            }
            catch (Exception ex)
            {
                LastException=ex.Message;
            }
            return false;
        }

        public async Task<PaginatedResult<ComplaintsDTO>> GetTenantsComplainsAsync(int page, int pageSize)
        {
            try
            {
                LastException = null;
                var tottalCount = await _db.TenantComplaints.CountAsync();

                var list = await (
                    from complaint in _db.TenantComplaints
                    join tenant in _db.TenatDetails on complaint.TenantId equals tenant.Id
                    join User in _db.Users on tenant.UserId equals User.Id
                       select new ComplaintsDTO
                       {
                              ComplaintId = complaint.Id,
                              TenantName = User.Name,
                              ComplaintTitle = complaint.Title,
                              ComplaintDetails = complaint.Details,
                              IsFixed = complaint.IsFixed,
                              ComplainDate = complaint.CreatedAt
                            })
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize).ToListAsync();

                if (list!=null)
                {
                    //IEnumerable<ComplaintsDTO> results = _mapper.Map<IEnumerable<ComplaintsDTO>>(list);
                    var paginatedResults = new PaginatedResult<ComplaintsDTO>
                    {
                        Items = list,
                        TotalCount = tottalCount,
                        PageSize = pageSize,
                        Page = page
                    };
                    return paginatedResults;
                }

                return new PaginatedResult<ComplaintsDTO>
                {
                    Items = Enumerable.Empty<ComplaintsDTO>(),
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize
                };

            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            return null;
        }

        public async Task<ComplaintFiles> FileDetails(string complaintId)
        {
            try
            {
                return await _db.ComplaintFiles.FirstOrDefaultAsync(f=>f.ComplaintId == complaintId);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the complaint image.", ex);
            }
        }
        public async Task<Stream> GetUserComplaintImageAsync(string complaintId)
        {
            try
            {
                var file = await _db.ComplaintFiles.FirstOrDefaultAsync(f => f.ComplaintId == complaintId);

                if (file == null)
                {
                    throw new FileNotFoundException("Complaint file not found.");
                }

                var fileId = file.Id;
                var readStream = await _fileHandling.GetFileAsStreamAsync(fileId);

                if (readStream == null)
                {
                    throw new IOException("Failed to read file stream.");
                }

                return readStream;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the complaint image.", ex);
            }
        }




    }
}
