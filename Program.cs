public class LinQTutorial
{
    public class Student
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int Age { get; set; }
        public int StandardID { get; set; }
    }

    public class Standard
    {
        public int StandardID { get; set; }
        public string StandardName { get; set; }
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("Hello LinQTutorial!");
        SampleQueries();
    }

    static void SampleQueries()
    {
        //Collections
        IList<Student> studentList = new List<Student>() {
            new Student() { StudentID = 1, StudentName = "John", Age = 18, StandardID = 1 } ,
            new Student() { StudentID = 2, StudentName = "Steve",  Age = 21, StandardID = 1 } ,
            new Student() { StudentID = 3, StudentName = "Bill",  Age = 18, StandardID = 2 } ,
            new Student() { StudentID = 4, StudentName = "Ram" , Age = 20, StandardID = 2 } ,
            new Student() { StudentID = 5, StudentName = "Ron" , Age = 21 }
        };

        IList<Standard> standardList = new List<Standard>() {
            new Standard(){ StandardID = 1, StandardName="Standard 1"},
            new Standard(){ StandardID = 2, StandardName="Standard 2"},
            new Standard(){ StandardID = 3, StandardName="Standard 3"}
        };

        //Multiple Select and where Operator
        var studentNames = studentList.Where(s => s.Age > 18)
                              .Select(s => s)
                              .Where(st => st.StandardID > 0)
                              .Select(s => s.StudentName);

        //LINQ Query returns Collection of Anonymous Objects
        var teenStudentsName = from s in studentList
                               where s.Age > 12 && s.Age < 20
                               select new { StudentName = s.StudentName };

        teenStudentsName.ToList().ForEach(s => Console.WriteLine(s.StudentName));
        Console.WriteLine("End LINQ Query returns Collection of Anonymous Objects");

        //LINQ GroupBy Query
        var studentsGroupByStandard = from s in studentList
                                      group s by s.StandardID into sg
                                      orderby sg.Key
                                      select new { sg.Key, sg };

        foreach (var group in studentsGroupByStandard)
        {
            Console.WriteLine("StandardID {0}:", group.Key);

            group.sg.ToList().ForEach(st => Console.WriteLine(st.StudentName));
        }
        Console.WriteLine("End LINQ GroupBy Query");

        //LINQ Left Outer Join
        var studentsGroup = from stad in standardList
                            join s in studentList
                            on stad.StandardID equals s.StandardID
                                into sg
                            select new
                            {
                                StandardName = stad.StandardName,
                                Students = sg
                            };

        foreach (var group in studentsGroup)
        {
            Console.WriteLine(group.StandardName);

            group.Students.ToList().ForEach(st => Console.WriteLine(st.StudentName));
        }
        Console.WriteLine("------------------------");

        var studentsWithStandard = from stad in standardList
                                   join s in studentList
                                   on stad.StandardID equals s.StandardID
                                   into sg
                                   from std_grp in sg
                                   orderby stad.StandardName, std_grp.StudentName
                                   select new
                                   {
                                       StudentName = std_grp.StudentName,
                                       StandardName = stad.StandardName
                                   };

        foreach (var group in studentsWithStandard)
        {
            Console.WriteLine("{0} is in {1}", group.StudentName, group.StandardName);
        }
        Console.WriteLine("End LINQ Left Outer Join");

        //LINQ Inner join
        var studentWithStandard = from s in studentList
                                  join stad in standardList
                                  on s.StandardID equals stad.StandardID
                                  select new
                                  {
                                      StudentName = s.StudentName,
                                      StandardName = stad.StandardName
                                  };

        studentWithStandard.ToList().ForEach(s => Console.WriteLine("{0} is in {1}", s.StudentName, s.StandardName));
        Console.WriteLine("End LINQ Inner Join");

        //Sorting
        var sortedStudents = from s in studentList
                             orderby s.StandardID, s.Age
                             select new
                             {
                                 StudentName = s.StudentName,
                                 Age = s.Age,
                                 StandardID = s.StandardID
                             };

        sortedStudents.ToList().ForEach(s => Console.WriteLine("Student Name: {0}, Age: {1}, StandardID: {2}", s.StudentName, s.Age, s.StandardID));
        Console.WriteLine("End LINQ Sorting");

        //Nested Query
        var nestedQueries = from s in studentList
                            where s.Age > 18 && s.StandardID ==
                                (from std in standardList
                                 where std.StandardName == "Standard 1"
                                 select std.StandardID).FirstOrDefault()
                            select s;

        nestedQueries.ToList().ForEach(s => Console.WriteLine(s.StudentName));
        Console.WriteLine("End LINQ Nested Queries");
    }
}
