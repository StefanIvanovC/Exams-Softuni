using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassroomProject
{
    public class Classroom
    {
        private new List<Student> students;

        
        public int Capacity { get; set; }
        public int Count { get { return students.Count; } }


        public string RegisterStudent(Student student)
        {
            
            if (students.Count < Capacity)
            {
                students.Add(student);
                return $"Added student {student.FirstName} {student.LastName}";
            }
            
                return $"No seats in the classroom";
            
        }

        public string DismissStudent(string firstName, string lastName)
        {
            Student studentToRemove = students.FirstOrDefault(x =>
            x.FirstName == firstName && x.LastName == lastName);
            if (studentToRemove != null)
            {
                students.Remove(studentToRemove);
                return $"Dismissed student {firstName} {lastName}";
            }
            return $"Student not found";
        }

        public string GetSubjectInfo(string subject)
        {
            if (students.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Subject: {subject} Students:");

                foreach (var person in students)
                {
                    sb.AppendLine(person.ToString());
                }
                
                return sb.ToString();
            }
            return $"No students enrolled for the subject";


        }

        public int GetStudentsCount()
        {
            return students.Count;
        }

        public string GetStudent(string firstName, string lastName)
        {
            return $"{firstName} {lastName}";
        }

        public override bool Equals(object obj)
        {
            return obj is Classroom classroom &&
                   Capacity == classroom.Capacity &&
                   Count == classroom.Count;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Capacity, Count);
        }
    }
}
