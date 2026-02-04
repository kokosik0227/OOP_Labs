using System;
using System.Collections.Generic;
using IOLib;
using Models;

namespace PL
{
public static class Menu
{
private static List<Student> students=new();
private static List<Dentist> dentists=new();
private static List<Storyteller> storytellers=new();


public static void MainMenu()
{
while(true)
{
Console.Clear();
Console.WriteLine("1. Add sample entities");
Console.WriteLine("2. Input student manually");
Console.WriteLine("3. Save all to file");
Console.WriteLine("4. Load all from file");
Console.WriteLine("5. Count female excellent students (5th course)");
Console.WriteLine("6. Show all entities");
Console.WriteLine("7. Storytellers");
Console.WriteLine("0. Exit");


switch(Console.ReadLine())
{
case "1":AddSample();break;
case "2":InputStudent();break;
case "3":SaveMenu();break;
case "4":LoadMenu();break;
case "5":CountExcellent();break;
case "6":ShowAll();break;
case "7":TellStories();break;
case "0":return;
}
Console.WriteLine("Press any key...");
Console.ReadKey();
}
}


private static void AddSample()
{
students.AddRange(new List<Student>{
new Student{LastName="Ivanova",FirstName="Olena",Course=5,Sex="F",AverageScore=5.0,StudentCard="S001",IdCode="1001"},
new Student{LastName="Petrenko",FirstName="Oleg",Course=3,Sex="M",AverageScore=4.0,StudentCard="S002",IdCode="1002"}
});


dentists.AddRange(new List<Dentist>{
new Dentist{Name="Dr. Smith",HasTools=true},
new Dentist{Name="Dr. Brown",HasTools=false}
});


storytellers.AddRange(new List<Storyteller>{
new Storyteller{Name="Anna",HasTools=true},
new Storyteller{Name="Tom",HasTools=false}
});
}


private static void InputStudent()
{
var s=new Student();
Console.Write("Last name: ");s.LastName=Console.ReadLine()??"";
Console.Write("First name: ");s.FirstName=Console.ReadLine()??"";
Console.Write("Course: ");s.Course=int.TryParse(Console.ReadLine(),out var c)?c:1;
Console.Write("Sex(M/F): ");s.Sex=Console.ReadLine()??"";
Console.Write("Average score: ");s.AverageScore=double.TryParse(Console.ReadLine(),out var av)?av:0;
students.Add(s);
}


private static DataProvider PickProvider()
{
Console.WriteLine("Select provider: 1-XML, 2-JSON");
return Console.ReadLine() switch
{
"1"=>new XmlProvider(),
"2"=>new JsonProvider(),
_=>new JsonProvider()
};
}


private static void SaveMenu()
{
           var provider = PickProvider();
            Console.Write("File path: ");
            var path = Console.ReadLine();
            if (string.IsNullOrEmpty(path)) return;

            var context = new EntityContext(provider);
            var service = new EntityService(context);
            service.SaveAll(students, dentists, storytellers, path);
            Console.WriteLine("Saved successfully!");
}

private static void LoadMenu()
{
 var provider = PickProvider();
            Console.Write("File path: ");
            var path = Console.ReadLine();
            if (string.IsNullOrEmpty(path)) return;

            var context = new EntityContext(provider);
            var service = new EntityService(context);
            service.LoadAll(out students, out dentists, out storytellers, path);
            Console.WriteLine("Loaded successfully!");
}


private static void CountExcellent()
{
var ctx=new EntityContext(new JsonProvider());
var service=new EntityService(ctx);
var count=service.Count(students);
Console.WriteLine($"Female excellent (5th course): {count}");
}


private static void ShowAll()
{
Console.WriteLine("\nStudents");
foreach(var s in students)Console.WriteLine($"{s.LastName} {s.FirstName}, Course:{s.Course}, Sex:{s.Sex}, Avg:{s.AverageScore}");


Console.WriteLine("\nDentists");
foreach(var d in dentists)Console.WriteLine($"{d.Name}, HasTools:{d.HasTools}");


Console.WriteLine("\n--- Storytellers ---");
foreach(var st in storytellers)Console.WriteLine($"{st.Name}, HasTools:{st.HasTools}");
}


private static void TellStories()
{
foreach(var st in storytellers)
{
if(st.HasTools)
Console.WriteLine($"{st.Name} tells a story to the children!");
else
Console.WriteLine($"{st.Name} cannot tell story (no tools).");
}
}
}
}