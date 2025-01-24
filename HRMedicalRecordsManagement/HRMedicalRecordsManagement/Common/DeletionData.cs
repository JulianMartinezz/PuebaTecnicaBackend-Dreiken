namespace HRMedicalRecordsManagement.Common.DeletionData;

public class DeletionData
{
    public string? Reason {get; set;}
    public string? CurrentUser {get; set;}

    public DateOnly? CurrentDate {get; set;}

    public DeletionData(string reason, string currentUser, DateOnly currentDate){
        Reason = reason;
        CurrentUser = currentUser;
        CurrentDate = currentDate;
    }
}