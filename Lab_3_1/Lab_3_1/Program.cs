using IOLib;
using Lab_3_1;
using Models.Lab3.Core;
using Models;

class Program
{
    static void Main(string[] args)
    {
        IRepository<Person> repository = new FileRepository();
        ConsoleMenu menu = new ConsoleMenu(repository);

        menu.DisplayMenu();
        menu.Run();
    }
}