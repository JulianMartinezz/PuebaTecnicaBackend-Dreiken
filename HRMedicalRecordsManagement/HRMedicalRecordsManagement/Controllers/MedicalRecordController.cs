using HRMedicalRecordsManagement.Services;
using HRMedicalRecordsManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and PageSize must be greater than 0");
            }

            var medicalRecords = await _medicalRecordService.GetFilteredMedicalRecordsAsync(page, pageSize, statusId, startDate, endDate, medicalRecordTypeId);
            return Ok(medicalRecords);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicalRecordById(int id)
        {
            var medicalRecordDto = await _medicalRecordService.GetByIdAsync(id);
            return Ok(medicalRecordDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddMedicalRecord([FromBody] TMedicalRecord medicalRecord)
        {
            var currentUser = "admin"; //Hardcoded since there is no requisite for authorization/authentication

            await _medicalRecordService.AddAsync(medicalRecord, currentUser);
            return Ok(medicalRecord.MedicalRecordId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalRecord(int id, [FromBody] TMedicalRecord medicalRecord)
        {
            var currentUser = "admin"; //Hardcoded since there is no requisite for authorization/authentication
            medicalRecord.MedicalRecordId = id;
            await _medicalRecordService.UpdateAsync(medicalRecord, currentUser);
            return  Ok(medicalRecord);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalRecord(int id, string reason)
        {
            var currentUser = "admin"; //Hardcoded since there is no requisite for authorization/authentication

            await _medicalRecordService.DeleteAsync(id, currentUser, reason);
            return Ok();
        }
    }
}