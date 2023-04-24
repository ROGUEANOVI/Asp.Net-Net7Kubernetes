using AutoMapper;
using Net7Kubernetes.DTOs.Estate;
using Net7Kubernetes.Models;

namespace Net7Kubernetes.Mappers
{
    public class EstateProfile : Profile
    {
        public EstateProfile()
        {
            CreateMap<Estate, EstateResponseDTO>().ReverseMap();
            CreateMap<EstateRequestDTO, Estate>().ReverseMap();
        }
    }
}