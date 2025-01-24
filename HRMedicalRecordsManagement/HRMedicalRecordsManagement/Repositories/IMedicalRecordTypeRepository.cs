public interface IMedicalRecordTypeRepository
{
    Task<bool> MedicalRecordTypeExistsAsync(int medicalRecordTypeId);
}