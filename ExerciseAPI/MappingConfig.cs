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


        CreateMap<TrainingPlanModel, TrainingPlanCreateDTO>().ReverseMap();
        CreateMap<TrainingPlanModel, TrainingPlanUpdateDTO>().ReverseMap();

        CreateMap<TrainingPlanDTO, TrainingPlanCreateDTO>().ReverseMap();
        CreateMap<TrainingPlanDTO, TrainingPlanUpdateDTO>().ReverseMap();


		CreateMap<TrainingPlanExerciseModel, TrainingPlanExerciseCreateDTO>().ReverseMap();
		CreateMap<TrainingPlanExerciseModel, TrainingPlanExerciseUpdateDTO>().ReverseMap();

		CreateMap<TrainingPlanExerciseDTO, TrainingPlanExerciseCreateDTO>().ReverseMap();
		CreateMap<TrainingPlanExerciseDTO, TrainingPlanExerciseUpdateDTO>().ReverseMap();
	}
}
