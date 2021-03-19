using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace LINQ.XML
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Users\Njegos Dukic\Documents\C#\LINQ - XML\Students.xml";
            var students = new List<Student>();

            int answer = -1;
            if (File.Exists(path))
            {
                do
                {
                    Console.WriteLine("\nDa li zelite da kreirate novi fajl ili da citate iz postojeceg?\n\n\t1 - Novi fajl.\n\t2 - Citanje iz postojeceg.");
                    Console.Write("\nUnesite odgovor: ");
                    Int32.TryParse(Console.ReadLine(), out answer);

                    if (answer == 1)
                    {
                        Console.WriteLine("\nBrisanje XML fajla.");
                        File.Delete(path);
                        XMLManipulation.CreateAndPopulateXML(path);
                    }

                    else if (answer == 2)
                    {
                        Student._studentCount = XElement.Load(path).Elements("Student").ToList().Count();
                        XMLManipulation.AddEntryToCreatedXML(XElement.Load(path), path);
                    }
                } while (answer != 1 && answer != 2);
            }

            if (answer == -1)
                XMLManipulation.CreateAndPopulateXML(path);

            XMLManipulation.EditXML(path);

            if (XElement.Load(path).IsEmpty)
            {
                Console.WriteLine("\nBrisanje XML fajla jer ne sadrzi korisni sadrzaj.\n");
                File.Delete(path);
            }
        }
    }
}
