public class MedicalRecord
{
    public int MedicalRecordId { get; set; }
    public string Audiometry { get; set; }
    public string PositionChange { get; set; }
    public string MotherData { get; set; }
    public string Diagnosis { get; set; }
    public string OtherFamilyData { get; set; }
    public string FatherData { get; set; }
    public string ExecuteMicros { get; set; }
    public string ExecuteExtra { get; set; }
    public string VoiceEvaluation { get; set; }
    public DateTime? DeletionDate { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ModificationDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime StartDate { get; set; }
    public int StatusId { get; set; }
    public int MedicalRecordTypeId { get; set; }
    public string Disability { get; set; }
    public string MedicalBoard { get; set; }
    public string DeletionReason { get; set; }
    public string Observations { get; set; }
    public decimal? DisabilityPercentage { get; set; }
    public string DeletedBy { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public string AreaChange { get; set; }

    public Status Status { get; set; }
    public MedicalRecordType MedicalRecordType { get; set; }
}