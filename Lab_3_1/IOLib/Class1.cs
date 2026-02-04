using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using System;
using Models.Lab3.Core;
using Models;

namespace IOLib

{
    public class FileRepository : IRepository<Person>
        {
        public void Save(Person[] persons, string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                foreach (var p in persons)
                {
                    if (p == null) continue;

                    string header = $"{p.GetType().Name} {SanitizeName(p.FirstName + p.LastName)}";

                    sw.WriteLine(header);
                    sw.WriteLine("{");

                    sw.Write(p.ToPersistenceString());

                    sw.WriteLine("};");
                }
            }
        }

        public Person[] Load(string path)
        {
            if (!File.Exists(path)) return new Person[0];

            var lines = File.ReadAllLines(path, Encoding.UTF8);
            Person[] result = new Person[0];

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrEmpty(line) || line == "{") continue;

                var headerMatch = Regex.Match(line, @"^(?<type>\w+)\s+(?<name>\w+)$");
                if (!headerMatch.Success) continue;
                string type = headerMatch.Groups["type"].Value;

                i++; 
                var blockBuilder = new StringBuilder(); 
                
                while (i < lines.Length && !lines[i].Trim().EndsWith("};"))
                {
                    blockBuilder.Append(lines[i].Trim());
                    i++;
                }
                if (i < lines.Length && lines[i].Trim().EndsWith("};"))
                {
                    blockBuilder.Append(lines[i].Trim().Replace("};", "").Trim());
                }

                string attributeBlock = blockBuilder.ToString().Replace(",", "");

                string pattern = @"""(?<k>[^""]+)""\s*:\s*""(?<v>[^""]+)""";
                var matches = Regex.Matches(attributeBlock, pattern);

                string fn = null, ln = null, courseS = null, studentId = null, genderS = null;
                string gradeAvgS = null, idCode = null, spec = null, lic = null, ptS = null;

                foreach (Match m in matches)
                {
                    string key = m.Groups["k"].Value.Trim().Replace("\"", "").Replace(",", "");
                    string value = m.Groups["v"].Value.Trim();

                    switch (key.ToLower())
                    {
                        case "firstname": fn = value; break;
                        case "lastname": ln = value; break;
                        case "course": courseS = value; break;
                        case "studentid": studentId = value; break;
                        case "gender": genderS = value; break;
                        case "gradeavg": gradeAvgS = value; break;
                        case "idcode": idCode = value; break;
                        case "speciality": spec = value; break;
                        case "licensenumber": lic = value; break;
                        case "patientstreated": ptS = value; break;
                    }
                }
                
                Person newPerson = null;
                Enum.TryParse<Gender>(genderS, out Gender gender);
                
                try
                {
                    if (string.Equals(type, "Student", StringComparison.OrdinalIgnoreCase))
                    {
                        if (int.TryParse(courseS, out int course) &&
                            double.TryParse(gradeAvgS, out double gradeAvg))
                            {
                             newPerson = new Student(fn, ln, course, studentId, gender, gradeAvg, idCode);
                        }
                    }
                    else if (string.Equals(type, "Storyteller", StringComparison.OrdinalIgnoreCase))
                    {
                        newPerson = new Storyteller(fn, ln, spec, gender);
                    }
                    else if (string.Equals(type, "Dentist", StringComparison.OrdinalIgnoreCase))
                    {
                        if (int.TryParse(ptS, out int pt))
                             newPerson = new Dentist(fn, ln, int.Parse(ptS), gender);
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error loading {type}: {ex.Message}. Record skipped.");
                }


                if (newPerson != null)
                {                    
                    result = Append(result, newPerson);
                }
            }

            return result;
        }

        private Person[] Append(Person[] arr, Person item)
        {
            var newArr = new Person[arr.Length + 1];
            for (int i = 0; i < arr.Length; i++) newArr[i] = arr[i];
            newArr[arr.Length] = item;
            return newArr;
        }

        public Person[] RemoveAt(Person[] arr, int indexToRemove)
        {
            if (indexToRemove < 0 || indexToRemove >= arr.Length)
            {
                return arr; 
            }

            var newArr = new Person[arr.Length - 1];
            int newIndex = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                if (i != indexToRemove)
                {
                    newArr[newIndex] = arr[i];
                    newIndex++;
                }
            }
            return newArr;
        }

        private string SanitizeName(string s)
        {
            if (string.IsNullOrEmpty(s)) return "Obj";
            return Regex.Replace(s, @"\s+", "");
        }
    }
}