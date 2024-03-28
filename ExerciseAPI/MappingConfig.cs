using AutoMapper;
using DbDataAccess.Models;
using ExerciseAPI.Models;
using ExerciseAPI.Models.DTO;

namespace ExerciseAPI;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<ExerciseModel, Exercise>().ReverseMap();
        CreateMap<ExerciseModel, ExerciseDTO>().ReverseMap();
        CreateMap<ExerciseModel, ExerciseCreateDTO>().ReverseMap();
        CreateMap<ExerciseModel, ExerciseUpdateDTO>().ReverseMap();
    }
}
