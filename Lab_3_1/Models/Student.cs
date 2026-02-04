using Models.Lab3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Student : Person
    {
        public int Course { get; private set; }        
        public string StudentId { get; private set; }   
        public double GradeAvg { get; private set; }   
        public string IdCode { get; private set; }     

        public Student(string firstName, string lastName, int course, string studentId, Gender gender, double gradeAvg, string idCode)
        : base(firstName, lastName, gender) 
        {
            if (course < 1 || course > 6)
                throw new ArgumentOutOfRangeException(nameof(course), "Course must be between 1 and 6.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(studentId, @"^[A-Z]{2}\d{6}$"))
                throw new ArgumentException("Student ID format is invalid (e.g., KB123456).", nameof(studentId));

            if (gradeAvg < 0.0 || gradeAvg > 5.0) 
                throw new ArgumentOutOfRangeException(nameof(gradeAvg), "Average grade must be between 0.0 and 5.0.");

            Course = course;
            StudentId = studentId;
            Gender = gender;
            GradeAvg = gradeAvg;
            IdCode = idCode;
        }

        public override void Study()
        {
            if (Course < 6) Course++;
        }

        public override void Cook()
        {
            if (Gender == Gender.Male)
            {
                Gender = Gender.Female;
            }
            else
            {
                Gender = Gender.Male;
            }
        }

        public override string ToPersistenceString()
        {
            string baseData = base.ToPersistenceString();

            string objData = $"\"course\": \"{Course}\",\n" +
                                 $"\"studentId\": \"{StudentId}\",\n" +
                                 $"\"gender\": \"{Gender}\",\n" +
                                 $"\"gradeAvg\": \"{GradeAvg}\",\n" +
                                 $"\"idCode\": \"{IdCode}\"";

            return baseData + objData;
        }
        public override string ToString()
        {
            return $"Student {FirstName} {LastName}, Course={Course}, ID={StudentId}, Gender={Gender}, GradeAvg={GradeAvg}, IdCode={IdCode}";
        }
    }
}

