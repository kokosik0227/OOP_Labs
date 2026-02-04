using System;
namespace IOLib
{
[Serializable]
public class StudentEntity
{
public string LastName { get; set; }
public string FirstName { get; set; }
public int Course { get; set; }
public string StudentCard { get; set; }
public string Sex { get; set; }
public double AverageScore { get; set; }
public string IdCode { get; set; }
}
}