using AutoMapper;
using CandidateProvider.Models;
using CandidateProvider.DTOs;

namespace CandidateProvider.Mappings
{
    public class CandidateProfile: Profile
    {
        public CandidateProfile()
            {
                CreateMap<Candidate, CandidateReadDto>()
                    .ForMember(dest => dest.Position, opt => opt.MapFrom(src =>
                        src.JobTitle != null ? src.JobTitle.Title : "未分配"))
                    .ForMember(dest => dest.SkillNames, opt => opt.MapFrom(src =>
                        src.Skills.Select(s => s.Name).ToList()));

                CreateMap<CandidateCreateDto, Candidate>();
                CreateMap<CandidateUpdateDto, Candidate>();
                CreateMap<CandidateCreateDto, Candidate>()
                    .ForMember(dest => dest.Skills, opt => opt.Ignore());
            }
    }
}
