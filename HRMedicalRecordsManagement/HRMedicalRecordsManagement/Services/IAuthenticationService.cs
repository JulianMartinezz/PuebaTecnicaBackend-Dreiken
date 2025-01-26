using HRMedicalRecordsManagement.Models.BaseResponse;
using Microsoft.AspNetCore.Identity.Data;

public interface IAuthenticationService
{
    Task<BaseResponse<string>> AuthenticateAsync(LoginRequest loginRequest);
}