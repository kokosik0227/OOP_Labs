using Models.Lab3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Storyteller : Person, ITell
    {
        public string Speciality { get; private set; }

        public Storyteller(string firstName, string lastName, string speciality, Gender gender)
            : base(firstName, lastName, gender)
        {
            Speciality = speciality.ToLower();
        }

        public override void Study()
        {
            Speciality = "Storyteller";

        }

        public override void Cook()
        {

        }

        public void Tell()
        {
            Speciality = "Master" + Speciality;

        }

        public override string ToPersistenceString()
        {
            string baseData = base.ToPersistenceString();

            string objData = $"\"speciality\": \"{Speciality}\"\n";

            return baseData + objData;
        }

        public override string ToString()
        {
            return $"Storyteller {FirstName} {LastName}, Speciality={Speciality}";
        }
    }
}

