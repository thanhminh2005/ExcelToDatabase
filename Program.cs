using Ganss.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelToDatabase
{
    class Program
    {
        

        static void Main(string[] args)
        {
            var context = new FLSContext();

            var method = new Execute(context);
            method.Run();
        }
    }

    public class Execute
    {
        private readonly FLSContext _context;

        public Execute(FLSContext context)
        {
            _context = context;
        }

        public void Run()
        {
            var Filepath = @"C:\Users\MinhNT\OneDrive\Desktop\data\StudentGroup.xlsx";//
            var Students = new ExcelMapper(Filepath).Fetch<ExGroup>();
            foreach (var obj in Students)
            {
                var exist = _context.StudentGroup.SingleOrDefault(x => x.Name.Equals(obj.Name));
                if(exist == null)
                {
                    var student = new StudentGroup
                    {
                        Name = obj.Name
                    };

                    _context.StudentGroup.Add(student);
                    _context.SaveChanges();
                }
                var subcode = "CSD201";//
                var course = new Course
                {
                    Name = subcode + "_" + obj.Name,
                    SemesterId = 2,
                    StudentGroupId = _context.StudentGroup.SingleOrDefault(x => x.Name.Equals(obj.Name)).Id,
                    SubjectId = _context.Subject.SingleOrDefault(x => x.SubjectCode.Equals(subcode)).Id,
                };
                _context.Course.Add(course);
                _context.SaveChanges();
            }
        }

        public class ExSubject
        {
            public string Subject { get; set; }
            public string CourseDetail { get; set; }
        }

        public class ExGroup
        {
            public string Name { get; set; }
        }
        
    }
}
