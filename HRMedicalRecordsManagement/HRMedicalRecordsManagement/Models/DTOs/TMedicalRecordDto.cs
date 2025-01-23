namespace HRMedicalRecordsManagement.DTOs
{
    public class TMedicalRecordDto
    {
        public int MedicalRecordId { get; set; }
        public string Diagnosis { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int StatusId { get; set; }
        public int MedicalRecordTypeId { get; set; }
    }
}