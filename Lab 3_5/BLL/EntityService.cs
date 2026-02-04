using System;
using System.Collections.Generic;
using System.Linq;
using IOLib;

namespace Models
{
public class EntityService
{
private readonly EntityContext _context;
public EntityService(EntityContext context)=> _context=context;


public void SaveAll(List<Student> students,List<Dentist> dentists,List<Storyteller> storytellers,string path)
{
var se=students.Select(StudentMapper.ToEntity).ToList();
var de=dentists.Select(DentistMapper.ToEntity).ToList();
var st=storytellers.Select(StorytellerMapper.ToEntity).ToList();
_context.SaveAll(se,de,st,path);
}


public void LoadAll(out List<Student> students,out List<Dentist> dentists,out List<Storyteller> storytellers,string path)
{
_context.LoadAll(out var se,out var de,out var st,path);
students=se.Select(StudentMapper.ToModel).ToList();
dentists=de.Select(DentistMapper.ToModel).ToList();
storytellers=st.Select(StorytellerMapper.ToModel).ToList();
}


public int Count(IEnumerable<Student> students)=>students.Count(s=>s.Course==5&&(s.Sex?.ToLower()=="f"||s.Sex?.ToLower()=="female")&&Math.Abs(s.AverageScore-5.0)<1e-9);
}
}