using Models.Lab3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Dentist : Person, ITreat
    {
        public int PatientsTreated { get; private set; }


        public Dentist(string firstName, string lastName, int patientsTreated, Gender gender)
            : base(firstName, lastName, gender)
        {
            PatientsTreated = patientsTreated;
        }

        public override void Study()
        {

        }

        public override void Cook()
        {

        }

        public void Treat()
        {
            PatientsTreated++;
        }

        public override string ToPersistenceString()
        {
            string baseData = base.ToPersistenceString();

            string objData = $"\"patientsTreated\": \"{PatientsTreated}\"\n"; 
                               
            return baseData + objData;
        }
        public override string ToString()
        {
            return $"Dentist {FirstName} {LastName}, Patients treated: {PatientsTreated}";
        }
    }
}
