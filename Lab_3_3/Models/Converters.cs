using IOLib;

namespace Models
{
public static class StudentMapper
{
public static Student ToModel(StudentEntity e) => new Student{ LastName=e.LastName, FirstName=e.FirstName, Course=e.Course, StudentCard=e.StudentCard, Sex=e.Sex, AverageScore=e.AverageScore, IdCode=e.IdCode};
public static StudentEntity ToEntity(Student s) => new StudentEntity{ LastName=s.LastName, FirstName=s.FirstName, Course=s.Course, StudentCard=s.StudentCard, Sex=s.Sex, AverageScore=s.AverageScore, IdCode=s.IdCode};
}


public static class DentistMapper
{
public static Dentist ToModel(DentistEntity e)=> new Dentist{Name=e.Name, HasTools=e.HasTools};
public static DentistEntity ToEntity(Dentist d)=> new DentistEntity{Name=d.Name, HasTools=d.HasTools};
}


public static class StorytellerMapper
{
public static Storyteller ToModel(StorytellerEntity e)=> new Storyteller{Name=e.Name, HasTools=e.HasTools};
public static StorytellerEntity ToEntity(Storyteller s)=> new StorytellerEntity{Name=s.Name, HasTools=s.HasTools};
}
}