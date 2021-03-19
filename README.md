# C# .NET Core - LINQ & XML

- C# .NET Core 3.0 LINQ & XML Processing.
- Operations include:
    - Creating
    - Populating
    - Reading 
    - Adding Entries
    - Editing Entries

---
- Extensible Markup Language **(XML)** is a markup language that defines a set of rules for encoding documents in a format that is both human-readable and machine-readable.
``` XML
<note>
    <to> John Doe </to>
    <from> Jane Doe </from>
    <heading> Question </heading>
    <body> What's in the box? </body>
</note>
```

- Language-Integrated Query **(LINQ)** is the name for a set of technologies based on the integration of query capabilities directly into the C# language. 
``` C#
class LINQQueryExpressions
{
    static void Main()
    {

        // Specify the data source.
        int[] scores = new int[] { 97, 92, 81, 60 };

        // Define the query expression.
        IEnumerable<int> scoreQuery =
            from score in scores
            where score > 80
            select score;

        // Execute the query.
        foreach (int i in scoreQuery)
        {
            Console.Write(i + " ");
        }
    }
}
// Output: 97 92 81
```
