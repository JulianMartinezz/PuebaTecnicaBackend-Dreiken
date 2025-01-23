using AutoMapper;
using HRMedicalRecordsManagement.DTOs;
using HRMedicalRecordsManagement.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapping TMedicalRecord to TMedicalRecordDto with default values for null fields
        CreateMap<TMedicalRecord, TMedicalRecordDto>()
            .ForMember(dest => dest.Diagnosis, opt => opt.MapFrom(src => src.Diagnosis ?? "Unknown Diagnosis")) // Provide default if null
            .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId ?? 0)) // Provide default if null
            .ForMember(dest => dest.MedicalRecordTypeId, opt => opt.MapFrom(src => src.MedicalRecordTypeId ?? 0)); // Provide default if null

        // Mapping Status to StatusDto
         CreateMap<Status, StatusDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name ?? "Unknown Status")); // Provide default if null

        // Mapping MedicalRecordType to MedicalRecordTypeDto
        CreateMap<MedicalRecordType, MedicalRecordTypeDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name ?? "Unknown Type")); // Provide default if null
    }
}