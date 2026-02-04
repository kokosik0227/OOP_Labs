using Models.Lab3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface IRepository<T> where T : Person
    {
        void Save(T[] entities, string path);
        T[] Load(string path);
    }
}
