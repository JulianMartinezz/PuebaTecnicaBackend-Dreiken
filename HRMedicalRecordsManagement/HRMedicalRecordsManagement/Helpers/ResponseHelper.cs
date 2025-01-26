using HRMedicalRecordsManagement.Models.BaseResponse;

namespace HRMedicalRecordsManagement.Helpers;

public static class ResponseHelper
{
    public static BaseResponse<T> Success<T>(T data, string message = "Request successful", int totalRows = 0)
    {
        return new BaseResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Code = 200,
            TotalRows = totalRows
        };
    }

    public static BaseResponse<T> BadRequest<T>(string message = "Bad request", string exception = null)
    {
        return new BaseResponse<T>
        {
            Success = false,
            Message = message,
            Exception = exception,
            Data = default,
            Code = 400
        };
    }

    public static BaseResponse<T> NotFound<T>(string message = "Resource not found")
    {
        return new BaseResponse<T>
        {
            Success = false,
            Message = message,
            Data = default,
            Code = 404
        };
    }

    public static BaseResponse<T> InternalServerError<T>(string message = "Internal server error", string exception = null)
    {
        return new BaseResponse<T>
        {
            Success = false,
            Message = message,
            Exception = exception,
            Data = default,
            Code = 500
        };
    }
}