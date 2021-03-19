using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LINQ.XML
{
    public class Student
    {
        private int _ordinal; 
        private string _name;
        private string _birthDate;
        private string _id;

        static public int _studentCount;

        public Student()
        {
            this._ordinal = ++_studentCount;
            Console.Write("\nMolimo unesite ime i prezime studenta: ");
            this._name = Console.ReadLine();

            bool inputCorrect = false;
            do
            {
                Console.Write("Molimo unesite datum rodjenja studenta u obliku DD/MM/GGGG: ");
                var regExDate = Console.ReadLine();
                if (Regex.IsMatch(regExDate, @"^(?:(?:31(/)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(/)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(/)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(/)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"))
                {
                    this._birthDate = regExDate;
                    inputCorrect = true;
                }
            } while(!inputCorrect);

            Console.Write("Molimo unesite broj indeksa studenta: ");
            this._id = Console.ReadLine();
        }

        public Student(int ordinal, string name, string birthDate, string id)
        {
            this._ordinal = ordinal;
            this._name = name;
            this._birthDate = birthDate;
            this._id = id;

            _studentCount++;
        }
        public int GetOrdinal() { return this._ordinal; }
        public string GetName() { return this._name; }
        private void SetName() 
        {
            Console.Write("Molimo unesite ime studenta: ");
            this._name = Console.ReadLine();
        }
        public string GetBirthDate() { return this._birthDate; }
        private void SetBirthDate()
        {
            Console.Write("Molimo unesite datum rodjenja studenta: ");
            this._birthDate = Console.ReadLine();
        }
        public string GetID() { return this._id; }
        private void SetID()
        {
            Console.Write("Molimo unesite broj indeksa studenta: ");
            this._id = Console.ReadLine();
        }
        public static void InitializeStudents(List<Student> list)
        {
            int odgovor;
            do
            {
                Console.WriteLine("\nDa li zelite unijeti novog studenta?\n\n\t1 - Da.\n\t2 - Ne.\n");
                Console.Write("Molim unesite vas odgovor: ");
                Int32.TryParse(Console.ReadLine(), out odgovor);
                if (odgovor == 1)
                {
                    odgovor = 0;
                    ReadSingleStudent(list);
                }
            }
            while (odgovor != 1 && odgovor != 2);
        }
        private static void ReadSingleStudent(List<Student> list)
        {
            Console.Write("\nMolimo unesite ime i prezime studenta: ");
            var name = Console.ReadLine();
            Console.Write("Molimo unesite datum rodjenja studenta u obliku DD/MM/GGGG: "); 
            var birthDate = Console.ReadLine(); // var birthDate = .ReadFormattedDate("Molimo unesite datum rodjenja studenta u obliku DD/MM/GGGG: ");
            Console.Write("Molimo unesite broj indeksa studenta: ");
            var id = Console.ReadLine();

            list.Add(new Student(_studentCount+1, name, birthDate, id));
        }
        public static void PresentStudents(List<Student> list)
        {
            foreach (var student in list)
            {
                Console.WriteLine("Redni broj: {0}", student._ordinal);
                Console.WriteLine("Student: {0}", student._name);
                Console.WriteLine("Datum rodjenja: {0}.", student._birthDate);
                Console.WriteLine("Broj indeksa: {0}", student._id);
                Console.WriteLine();
            }
        }
        private static void RemoveStudentAtPosition(List<Student> list, int ordinal)
        {
            list.RemoveAt(ordinal);

            foreach (var student in list)
                if (student._ordinal >= ordinal)
                    student._ordinal--;

            _studentCount--;
        }
        public static void EditStudents(List<Student> list)
        {
            PresentStudents(list);

            int answer;
            do
            {
                Console.WriteLine("\nDa li zelite da editujete studenta?\n\n\t1 - Da.\n\t2 - Ne.");
                Console.Write("\nUnesite odgovor: ");
                Int32.TryParse(Console.ReadLine(), out answer);

                if (answer == 1)
                {
                    int ordinal;
                    do
                    {
                        Console.WriteLine("\nUnesite redni broj studenta kojeg zelite editovati: ");
                        Console.Write("\nUnesite odgovor: ");
                        Int32.TryParse(Console.ReadLine(), out ordinal);
                    } while (ordinal < 1 || ordinal > _studentCount);

                    EditSpecificStudent(list, ordinal - 1);
                }

                else
                    return;

            } while (answer != 1 && answer != 2);
        }
        private static void EditSpecificStudent(List<Student> list, int ordinal)
        {
            var student = list.ElementAt(ordinal);
            int answer;
            do
            {
                Console.WriteLine("\nSta zelite da editujete?\n\t1 - Ime.\n\t2 - Datum rodjenja.\n\t3 - Broj indeksa.\n\t4 - Brisanje studenta.");
                Console.Write("\nUnesite odgovor: ");
                Int32.TryParse(Console.ReadLine(), out answer);
                Console.WriteLine();
            } while (answer < 1 || answer > 4);

            if (answer == 1)
                student.SetName();

            else if (answer == 2)
                student.SetBirthDate();

            else if (answer == 3)
                student.SetID();

            else
                Student.RemoveStudentAtPosition(list, ordinal);
        }
    }
}