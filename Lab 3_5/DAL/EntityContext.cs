using IOLib;
using System.Collections.Generic;
namespace IOLib
{
[Serializable]
public class EntityWrapper
{
public List<StudentEntity> Students { get; set; } = new();
public List<DentistEntity> Dentists { get; set; } = new();
public List<StorytellerEntity> Storytellers { get; set; } = new();
}


public class EntityContext
{
private readonly DataProvider _provider;

public EntityContext(DataProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

public void SaveAll(List<StudentEntity> students, List<DentistEntity> dentists, List<StorytellerEntity> storytellers, string path)
{
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(path))
                File.Create(path).Dispose();

            var wrapper = new EntityWrapper
            {
                Students = students,
                Dentists = dentists,
                Storytellers = storytellers
            };

            _provider.Save(wrapper, path);
        }

public void LoadAll(out List<StudentEntity> students, out List<DentistEntity> dentists, out List<StorytellerEntity> storytellers, string path)
{
    if (!File.Exists(path))
    {
        var wrapper = new EntityWrapper();
        SaveAll(wrapper.Students, wrapper.Dentists, wrapper.Storytellers, path);
    }

    var loadedWrapper = _provider.Load<EntityWrapper>(path);
    students = loadedWrapper.Students ?? new List<StudentEntity>();
    dentists = loadedWrapper.Dentists ?? new List<DentistEntity>();
    storytellers = loadedWrapper.Storytellers ?? new List<StorytellerEntity>();
}
}
}