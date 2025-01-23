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
            var medicalRecord = await _medicalRecordService.GetByIdAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }
            return Ok(medicalRecord);
        }

        [HttpPost]
        public async Task<IActionResult> AddMedicalRecord([FromBody] TMedicalRecord medicalRecord)
        {
            if (medicalRecord == null)
            {
                return BadRequest("Medical record is null.");
            }

            await _medicalRecordService.AddAsync(medicalRecord);
            return CreatedAtAction(nameof(GetMedicalRecordById), new { id = medicalRecord.MedicalRecordId}, medicalRecord);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalRecord(int id, [FromBody] TMedicalRecord medicalRecord)
        {
            if (id != medicalRecord.MedicalRecordId)
            {
                return BadRequest("Medical record ID mismatch.");
            }

            var existingRecord = await _medicalRecordService.GetByIdAsync(id);
            if (existingRecord == null)
            {
                return NotFound();
            }

            await _medicalRecordService.UpdateAsync(medicalRecord);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            var existingRecord = await _medicalRecordService.GetByIdAsync(id);
            if (existingRecord == null)
            {
                return NotFound();
            }

            await _medicalRecordService.DeleteAsync(id);
            return NoContent();
        }
    }
}