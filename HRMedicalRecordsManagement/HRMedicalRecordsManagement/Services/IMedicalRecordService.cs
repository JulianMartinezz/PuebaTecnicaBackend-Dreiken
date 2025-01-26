using HRMedicalRecordsManagement.Common.PagedList;
using HRMedicalRecordsManagement.Models;
using HRMedicalRecordsManagement.DTOs;
using HRMedicalRecordsManagement.Models.BaseResponse;

public interface IMedicalRecordService
{
    Task<IEnumerable<TMedicalRecord>> GetAllAsync();
    Task<BaseResponse<TMedicalRecordDto>> GetByIdAsync(int id);
    Task<BaseResponse<TMedicalRecordDto>> AddAsync(TMedicalRecord medicalRecord, string currentUser);
    Task<BaseResponse<TMedicalRecordDto>> UpdateAsync(TMedicalRecord medicalRecord, string currentUser);
    Task<BaseResponse<TMedicalRecordDto>> DeleteAsync(int id, string currentUser, string reason);
    Task<BaseResponse<PagedList<TMedicalRecordDto>>> GetFilteredMedicalRecordsAsync(
        int page,
        int pageSize,
        int? statusId,
        DateTime? startDate,
        DateTime? endDate,
        int? medicalRecordTypeId);
}