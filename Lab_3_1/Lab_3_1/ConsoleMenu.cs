using Models.Lab3.Core;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3_1
{
    public class ConsoleMenu
    {
        private readonly IRepository<Person> _repository;
        private const string FilePath = "database.txt";
        private Person[] _data = new Person[0];

        public ConsoleMenu(IRepository<Person> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                DisplayMenu();
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    try
                    {
                        switch (choice)
                        {
                            case 1: LoadData(); break;
                            case 2: SaveData(); break;
                            case 3: CreateSampleData(); break;
                            case 4: AddNewPerson(); break;
                            case 5: ShowAllData(); break;
                            case 6: SearchByLastName(); break;
                            case 7: SearchByIdentifier(); break;
                            case 8: CountExcellentFemales5Course(); break;
                            case 9: RemovePerson(); break;
                            case 10: PerformAction(); break;
                            case 0: running = false; break;
                            default: Console.WriteLine("Wrong choice, try again."); break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\nRuntime Error: {ex.Message}\n");
                    }
                }
                else
                {
                    Console.WriteLine("Incoret input format.");
                }
                if (running)
                {
                    Console.WriteLine("\nPress enter to continue.");
                    Console.ReadLine();
                }
            }
            Console.WriteLine("Program stopped.");
        }

        public void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine($"DB, entries: {_data.Length})");
            Console.WriteLine("1. Download data");
            Console.WriteLine("2. Upload data");
            Console.WriteLine("3. Сcreate demonstration data");
            Console.WriteLine("4. Create new object");
            Console.WriteLine("5. Show all entries");
            Console.WriteLine("6. Search by lastname");
            Console.WriteLine("7. Search by studentID");
            Console.WriteLine("8. Find female students with AvgScore of 5");
            Console.WriteLine("9. Remove entry from list");
            Console.WriteLine("10. Activate skill");
            Console.WriteLine("0. Exit");
            Console.Write("\nYour Choice: ");
        }

        private void LoadData()
        {
            _data = _repository.Load(FilePath);
            Console.WriteLine($"Data was successfully downloaded from {FilePath}. Number of entries: {_data.Length}");
        }

        private void SaveData()
        {
            _repository.Save(_data, FilePath);
            Console.WriteLine($"Data was uploaded to {FilePath}.");
        }

        private void ShowAllData()
        {
            if (_data.Length == 0)
            {
                Console.WriteLine("DB is empty.");
                return;
            }
            Console.WriteLine("All entries");
            foreach (var p in _data)
            {
                Console.WriteLine($"> {p}");
            }
        }

        private void CreateSampleData()
        {
            try
            {
                var s1 = new Student("Mariya", "K", 5, "KB123123", Gender.Female, 5, "2000111101");
                var s2 = new Student("A", "B", 4, "KZ412345", Gender.Male, 4, "1999123456");
                var s3 = new Student("B", "A", 5, "AD500999", Gender.Female, 4, "2001000000");
                var s4 = new Student("S", "D", 3, "ER333444", Gender.Female, 4, "1998010101");

                var d1 = new Dentist("B", "V", 150, Gender.Male);

                var st1 = new Storyteller("S", "D", "Megamaster3000", Gender.Female);

                _data = new Person[] { s1, s2, s3, s4, d1, st1 };
                Console.WriteLine($"Створено {_data.Length} демонстраційних записів.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error while creating testing data: {ex.Message}");
            }
        }

        private void CountExcellentFemales5Course()
        {
            Console.WriteLine("Finding good female students");

            var excellentStudents = _data
                .OfType<Student>()
                .Where(s =>
                    s.Course == 5 &&
                    s.Gender == Gender.Female &&
                    s.GradeAvg >= 4.75)
                .ToArray();

            Console.WriteLine($"Found {excellentStudents.Length} сstudents.");

            if (excellentStudents.Length > 0)
            {
                Console.WriteLine("Their data:");
                foreach (var s in excellentStudents)
                {
                    Console.WriteLine($"- {s.FirstName} {s.LastName}, Student Id {s.StudentId}, AvgScore: {s.GradeAvg}");
                }
            }
        }

        private void SearchByLastName()
        {
            Console.Write("Enter lastname ");
            string searchLastName = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(searchLastName))
            {
                Console.WriteLine("LAstname cant be empty");
                return;
            }

            var results = _data
                .Where(p => p.LastName.Equals(searchLastName, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            DisplaySearchResults(results, $"Lastname: '{searchLastName}'");
        }

        private void SearchByIdentifier()
        {
            Console.Write("Enter ID");
            string searchId = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(searchId)) return;

            var results = _data
                .Where(p =>
                    (p is Student s && (s.StudentId.Equals(searchId) || s.IdCode.Equals(searchId))) ||
                    (p is Dentist d && d.PatientsTreated.Equals(searchId)))
                .ToArray();

            DisplaySearchResults(results, $"By ID '{searchId}'");


        }
        private void RemovePerson()
        {
            Console.Write("Enter ID ");
            string searchId = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(searchId)) return;

            int indexToRemove = -1;
            for (int i = 0; i < _data.Length; i++)
            {
                var p = _data[i];

                bool found = false;
                if (p is Student s && (s.StudentId.Equals(searchId) || s.IdCode.Equals(searchId)))
                {
                    found = true;
                }

                if (found)
                {
                    indexToRemove = i;
                    break;
                }
            }

            if (indexToRemove == -1)
            {
                Console.WriteLine($"Entry with ID '{searchId}' not found.");
                return;
            }

            Console.WriteLine($"Found entry for deletion: {_data[indexToRemove]}");
            Console.Write("Are you sure u want to delete? (y/n): ");

            if (Console.ReadLine().Trim().ToLower() == "y")
            {

                _data = RemoveAtHelper(_data, indexToRemove);
            }
            else
            {
                Console.WriteLine("Delete aborted");
            }
        }


        private Person[] RemoveAtHelper(Person[] arr, int indexToRemove)
        {
            if (indexToRemove < 0 || indexToRemove >= arr.Length) return arr;

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
        private void DisplaySearchResults(Person[] results, string searchCriteria)
        {
            if (results.Length == 0)
            {
                Console.WriteLine($"Nothing found{searchCriteria}.");
                return;
            }

            Console.WriteLine($"Found {results.Length} entry/ies) {searchCriteria}:");
            foreach (var p in results)
            {
                Console.WriteLine($"> {p}");
            }
        }

        private void PerformAction()
        {
            Console.Write("\nDoing actionn");
            Console.Write("ВEnter ID: ");
            string searchId = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(searchId)) return;

            Person targetPerson = null;
            foreach (var p in _data)
            {
                if (p is Student s && (s.StudentId.Equals(searchId) || s.IdCode.Equals(searchId)))
                {
                    targetPerson = s;
                    break;
                }
            }

            if (targetPerson == null)
            {
                Console.WriteLine($"ENtry'{searchId}' not found.");
                return;
            }

            Console.WriteLine($"ЗFound: {targetPerson}");


            int actionChoice = DisplayPersonActions(targetPerson);

            if (actionChoice == 0) return;
            try
            {
                switch (actionChoice)
                {
                    case 1 when targetPerson is IStudy s:
                        s.Study();
                        break;

                    case 2 when targetPerson is ICook c:
                        c.Cook();
                        break;

                    case 3 when targetPerson is ITreat t:
                        t.Treat();
                        break;

                    case 4 when targetPerson is ITell tell:
                        tell.Tell();
                        break;

                    default:
                        Console.WriteLine("Wrong/unsupportable action");
                        break;
                }
                Console.WriteLine($"New state of entry: {targetPerson}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deoing action: {ex.Message}");
            }
        }
        private int DisplayPersonActions(Person p)
        {
            Console.WriteLine("\nAvailable actions");
            
            if (p is IStudy) Console.WriteLine("1: Study");
            if (p is ICook) Console.WriteLine("2: Cook");
            if (p is ITreat) Console.WriteLine("3: Treat");
            if (p is ITell) Console.WriteLine("4: Tell");

            Console.WriteLine("0: Back");
            Console.Write("Choose action: ");

            if (int.TryParse(Console.ReadLine(), out int action))
            {
                return action;
            }
            return -1;
        }

        private void AddNewPerson()
        {
            Console.WriteLine("\n-Add new entry");
            Console.WriteLine("1: Student, 2: Dentist, 3: Storyteller");
            Console.Write("ВChoice: ");

            if (!int.TryParse(Console.ReadLine(), out int typeChoice))
            {
                Console.WriteLine("Wrong choice.");
            }

            Console.Write("Enter name "); string fn = Console.ReadLine();
            Console.Write("Enter lastname: "); string ln = Console.ReadLine();
            Console.Write("Enter Gender:"); string Gender = Console.ReadLine();
            Gender gn = Models.Gender.Female;
            Enum.TryParse<Gender>(Gender, true, out gn);
            Person newPerson = null;

            try
            {
                switch (typeChoice)
                {
                    case 1:
                        Console.Write("Course (1-6): "); int c = int.Parse(Console.ReadLine());
                        Console.Write("Student ID (KB123456): "); string sid = Console.ReadLine();
                        Console.Write("aVGsCORE: "); string avg = Console.ReadLine();
                        Console.Write("Id: "); string id = Console.ReadLine();
                        Student student = new Student(fn, ln, c, sid, gn, double.Parse(avg), id);

                        _data = AppendHelper(_data, student);
                        break;
                    case 2:
                        Console.WriteLine("Use samples.");
                        break;
                    case 3:
                        Console.WriteLine("Use samples");
                        break;
                }

                if (newPerson != null)
                {
                    ;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nWrong format or data {ex.Message}");
            }
        }

        
        private Person[] AppendHelper(Person[] arr, Person item)
{
    var newArr = new Person[arr.Length + 1];
    for (int i = 0; i < arr.Length; i++) newArr[i] = arr[i];
    newArr[arr.Length] = item;
    return newArr;
}
    }
}
