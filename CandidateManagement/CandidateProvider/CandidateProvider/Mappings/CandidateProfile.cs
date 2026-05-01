using AutoMapper;
using CandidateProvider.Models;
using CandidateProvider.DTOs;

namespace CandidateProvider.Mappings
{
    /// <summary>
    /// AutoMapper profile configuration for Candidate entity mappings.
    /// Defines the mapping rules between entity and DTO classes.
    /// </summary>
    public class CandidateProfile: Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CandidateProfile"/> class.
        /// Configures all mappings between entities and DTOs.
        /// </summary>
        public CandidateProfile()
            {
                // Map Candidate entity to CandidateReadDto
                // Position is mapped from JobTitle.Title, defaults to "Not Assigned" if null
                CreateMap<Candidate, CandidateReadDto>()
                    .ForMember(dest => dest.Position, opt => opt.MapFrom(src =>
                        src.JobTitle != null ? src.JobTitle.Title : "Not Assigned"))
                    .ForMember(dest => dest.SkillNames, opt => opt.MapFrom(src =>
                        src.Skills.Select(s => s.Name).ToList()));

                // Map CandidateCreateDto to Candidate
                CreateMap<CandidateCreateDto, Candidate>();

                // Map CandidateUpdateDto to Candidate
                CreateMap<CandidateUpdateDto, Candidate>();

                // Additional mapping for creating candidates (ignores skills as they are handled separately in the controller)
                CreateMap<CandidateCreateDto, Candidate>()
                    .ForMember(dest => dest.Skills, opt => opt.Ignore());
            }
    }
}
