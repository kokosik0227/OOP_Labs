using System;

namespace lab
{
    public delegate string CompareSymbols(string input);

    public class ListClearedEventArgs : EventArgs
    {
        public DateTime Time { get; }
        
        public string Message { get; }

        public ListClearedEventArgs(string message)
        {
            Time = DateTime.Now;
            Message = message;
        }
    }

    public class ListModel
    {
        public event EventHandler<ListClearedEventArgs>? ListCleared;

        public void ClearList()
        {
            OnListCleared("List was cleared");
        }
        private void OnListCleared(string msg)
        {
            ListCleared?.Invoke(this, new ListClearedEventArgs(msg));
        }


    }


    public class EventHandler {
        private static void OnListClearedHandler(object? sender, ListClearedEventArgs e)
        {
            Console.WriteLine($"\nEvent: {e.Message}");
            Console.WriteLine($"Time: {e.Time}");
            Console.WriteLine($"Initiated by: {sender?.GetType().Name}");
        }
    class Program
    {
        static void Main(string[] args)
        {

            CompareSymbols anon = delegate (string s)
            {
                int letters = 0, digits = 0;

                foreach (char c in s)
                {
                    if (char.IsLetter(c)) letters++;
                    else if (char.IsDigit(c)) digits++;
                }

                if (letters > digits) return "Anon: more letter";
                if (digits > letters) return "Anon: more numbers";
                return "Anon: equal";
            };

            CompareSymbols lambda = (string s) =>
            {
                int letters = 0, digits = 0;

                foreach (char c in s)
                {
                    if (char.IsLetter(c)) letters++;
                    else if (char.IsDigit(c)) digits++;
                }

                if (letters > digits) return "Lambda: more letter";
                if (digits > letters) return "Lambda: more numbers";
                return "Lambda: equal";
            };

            Console.Write("Enter string: ");
            string input = Console.ReadLine() ?? "";

            Console.WriteLine(anon(input));
            Console.WriteLine(lambda(input));

            ListModel model = new ListModel();
            model.ListCleared += OnListClearedHandler;

            Console.WriteLine("\nClearing List");
            model.ClearList();
        }

       
        }
    }
}