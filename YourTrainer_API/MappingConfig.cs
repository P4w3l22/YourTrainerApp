using AutoMapper;
using YourTrainer_DBDataAccess.Models;
using YourTrainer_API.Models;
using YourTrainer_API.Models.DTO;

namespace YourTrainer_API;

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
