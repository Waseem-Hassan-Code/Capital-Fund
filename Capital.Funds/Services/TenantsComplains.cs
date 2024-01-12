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
                string sqlQuery = $"DELETE FROM TenantComplaints WHERE Id = @ComplainId";
                int rowsAffected = await _db.Database.ExecuteSqlRawAsync(sqlQuery, new SqliteParameter("@ComplainId", complainId));

                if (rowsAffected == 0)
                    return false;

                string fileId = await _db.ComplaintFiles.Select(u => u.FileURL).Where(c => complainId==complainId).FirstOrDefaultAsync();
                await _fileHandling.DeleteImage(fileId);

                return true;

            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            return false;
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

        public async Task<byte[]> GetUserComplaintImageAsync(string complaintId)
        {
            try
            {
                LastException = null;

                var file = await _db.ComplaintFiles.FirstOrDefaultAsync(f => f.ComplaintId == complaintId);

                if (file != null)
                {
                    string fileId = file.FileURL;
                    var readStream = await _fileHandling.ReadImageStream(fileId);
                    if (readStream == null)
                    {
                        LastException =  "An error occured while reading file stream.";
                    }
                    return readStream;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            return null;
        }


    }
}
