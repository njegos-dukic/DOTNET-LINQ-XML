using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace LINQ.XML
{
    partial class Program
    {
        class XMLManipulation
        {
            public static void CreateAndPopulateXML(string path)
            {
                // Populating XML directly from input.
                var outputFile = new XElement("Students");
                AddEntryToCreatedXML(outputFile, path);
                outputFile.Save(path);
            }

            public static void AddEntryToCreatedXML(XElement XML, string path)
            {
                int odgovor;
                do
                {
                    Console.WriteLine("\nDa li zelite unijeti novog studenta?\n\n\t1 - Da.\n\t2 - Ne.\n");
                    Console.Write("Molim unesite vas odgovor: ");
                    Int32.TryParse(Console.ReadLine(), out odgovor);
                    if (odgovor == 1)
                    {
                        WriteSingleStudentToXML(new Student(), XML);
                        odgovor = 0;
                    }
                }
                while (odgovor != 1 && odgovor != 2);

                XML.Save(path);
            }

            private static void WriteSingleStudentToXML(Student student, XElement XMLFile)
            {
                XMLFile.Add(new XElement("Student",
                                new XElement("Ordinal", student.GetOrdinal()),
                                new XElement("Name", student.GetName()),
                                new XElement("BirthDate", student.GetBirthDate()),
                                new XElement("ID", student.GetID())));
            }

            private static void PresentStudentsFromXML(string path)
            {
                XDocument XMLFile = XDocument.Load(path);
                var studentList = XMLFile.Descendants("Students").Elements("Student");
                foreach (var student in studentList)
                {
                    Console.WriteLine("\nRedni broj studenta: {0}.", (string)student.Element("Ordinal"));
                    Console.WriteLine("Ime studenta: {0}.", (string)student.Element("Name"));
                    Console.WriteLine("Datum rodjenja studenta: {0}.", (string)student.Element("BirthDate"));
                    Console.WriteLine("Broj indeksa studenta: {0}.", (string)student.Element("ID"));
                }
            }

            public static void EditXML(string path)
            {
                int answer;
                do
                {
                    Console.WriteLine("\nDa li zelite da editujete studenta?\n\n\t1 - Da.\n\t2 - Ne.");
                    Console.Write("\nUnesite odgovor: ");
                    Int32.TryParse(Console.ReadLine(), out answer);

                    if (answer == 1)
                        EditStudentInXML(path);

                    else if (answer == 2)
                        return;

                    // Da bi petlja ponovo usla u editovanje.
                    answer = 0;

                } while (answer != 1 && answer != 2);
            }

            private static void EditStudentInXML(string path)
            {
                XDocument XMLFile = XDocument.Load(path);
                var studentList = XMLFile.Descendants("Students").Elements("Student");

                int ordinal;
                do
                {
                    if (studentList.Count() == 0)
                    {
                        Console.WriteLine("\nU listi nema studenata.");
                        return;
                    }

                    PresentStudentsFromXML(path);
                    Console.Write("\nUnesite redni broj studenta kojeg zelite editovati: ");
                    Int32.TryParse(Console.ReadLine(), out ordinal);

                } while (ordinal < 1 || ordinal > studentList.Count());

                var student = (studentList.Where(element => (int)element.Element("Ordinal") == ordinal)).Single(); // Converting to single because LINQ returns XContainer that needs to be iterated over.

                // var student = (XMLFile.Elements("Student").Where(x => (int)x.Element("Ordinal") == 3)).Single(); // XElement realization.

                int answer;
                do
                {
                    Console.WriteLine("\nSta zelite da editujete?\n\n\t1 - Ime.\n\t2 - Datum rodjenja.\n\t3 - Broj indeksa.\n\t4 - Brisanje studenta.");
                    Console.Write("\nUnesite odgovor: ");
                    Int32.TryParse(Console.ReadLine(), out answer);
                    Console.WriteLine();
                } while (answer < 1 || answer > 4);

                if (answer == 1) // Name.
                {
                    Console.Write("Molimo unesite novo ime studenta: ");
                    student.SetElementValue("Name", Console.ReadLine());

                }

                else if (answer == 2) // Birth date.
                {
                    bool inputCorrect = false;
                    string regExDate;
                    do
                    {
                        Console.Write("Molimo unesite datum rodjenja studenta u obliku DD/MM/GGGG: ");
                        regExDate = Console.ReadLine();
                        if (Regex.IsMatch(regExDate, @"^(?:(?:31(/)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(/)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(/)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(/)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"))
                            inputCorrect = true;
                    } while (!inputCorrect);

                    student.SetElementValue("BirthDate", regExDate);
                }

                else if (answer == 3) // ID.
                {
                    Console.Write("Molimo unesite novi broj indeksa studenta: ");
                    student.SetElementValue("ID", Console.ReadLine());
                }

                else // Delete node.
                {
                    Console.WriteLine("Brisanje studenta: {0}.", (string)student.Element("Name"));
                    student.Remove();
                    foreach (var tempStudent in studentList.Where(tempStudent => (int)tempStudent.Element("Ordinal") > ordinal))
                        tempStudent.SetElementValue("Ordinal", (int)tempStudent.Element("Ordinal") - 1);
                }

                XMLFile.Save(path);
            }

            public static List<Student> ReadXMLToList(string path)
            {
                return (from student in XElement.Load(path).Elements("Student")
                        select new Student((int)student.Element("Ordinal"),
                                           (string)student.Element("Name"),
                                           (string)student.Element("BirthDate"), // Convert.ToDateTime()
                                           (string)student.Element("ID"))).ToList();
            }

            private static void SaveListToXML() // NIJE ZAVRSENO
            { }

            public static void ReadFormattedDateRegEx()
            {
                Console.WriteLine(Regex.IsMatch(Console.ReadLine(), @"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$"));
            }
        }
    }
}
