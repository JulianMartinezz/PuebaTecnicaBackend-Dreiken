using HRMedicalRecordsManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRMedicalRecordsManagement.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilterMedicalRecords(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] int? statusId = null, 
            [FromQuery] DateTime? startDate = null, 
            [FromQuery] DateTime? endDate = null, 
            [FromQuery] int? medicalRecordTypeId = null)
        {
            var response = await _medicalRecordService.GetFilteredMedicalRecordsAsync(page, pageSize, statusId, startDate, endDate, medicalRecordTypeId);
            return StatusCode(response.Code ?? 500, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicalRecordById(int id)
        {
            var response = await _medicalRecordService.GetByIdAsync(id);
            return StatusCode(response.Code ?? 500, response);
        }

        [HttpPost]
        public async Task<IActionResult> AddMedicalRecord([FromBody] TMedicalRecord medicalRecord)
        {
            var currentUser = "admin"; //Hardcoded since there is no requisite for authorization/authentication

            var response = await _medicalRecordService.AddAsync(medicalRecord, currentUser);
            return StatusCode(response.Code ?? 500, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalRecord(int id, [FromBody] TMedicalRecord medicalRecord)
        {
            var currentUser = "admin"; //Hardcoded since there is no requisite for authorization/authentication
            medicalRecord.MedicalRecordId = id;
            var response = await _medicalRecordService.UpdateAsync(medicalRecord, currentUser);
            return  StatusCode(response.Code ?? 500, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalRecord(int id, string reason)
        {
            var currentUser = "admin"; //Hardcoded since there is no requisite for authorization/authentication

            var response = await _medicalRecordService.DeleteAsync(id, currentUser, reason);
            return StatusCode(response.Code ?? 500, response);
        }
    }
}