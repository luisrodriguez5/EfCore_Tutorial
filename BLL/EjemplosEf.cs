using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using EfCore_Tutorial.Entidades;
using EfCore_Tutorial.Data;
using System.Linq;

namespace EfCore_Tutorial.BLL
{
    public class EjemplosEf
    {
                public static void GuardarStudentDB()
        {
            //Ejemplo de guardar en BD
            SchoolContext context = new SchoolContext();
            try
            {
                var auxStudent = new Student()
                {
                    StudentId = 0,
                    FirstName = "Michael",
                    LastName = "Madison"

                };
                context.Students.Add(auxStudent);
                bool save = context.SaveChanges() > 0;

                if (save)
                    Console.WriteLine("The Student was sucessfully saved!!");
                else
                    Console.WriteLine("We cant save the student..");
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                context.Dispose();
            }

        }

        public static void GuardarCourseDB()
        {
            //Ejemplo de guardar en BD
            SchoolContext context = new SchoolContext();
            try
            {
                var auxCourse = new Course()
                {
                    CourseId = 0,
                    studentId = 1,
                    CourseName = "Math"

                };
                context.Courses.Add(auxCourse);
                bool save = context.SaveChanges() > 0;

                if (save)
                    Console.WriteLine("The Course was sucessfully saved!!");
                else
                    Console.WriteLine("We cant save the course..");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }



        }


        public static void SimpleQueryDB()
        {
            //Ejemplo del Querying
            const string NAME = "Michael";
            SchoolContext context = new SchoolContext();
            try
            {
                var list = context.Students.Where(s => s.FirstName == NAME).ToList();
                if (list != null)
                    Console.WriteLine(list.Find(s => s.FirstName == NAME).FirstName);
                else
                    Console.WriteLine("We cant find the student!!");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        public static void DoubleQueryDB()
        {
            //Ejemplo del Querying con Inclue
            const string NAME = "Michael";
            SchoolContext context = new SchoolContext();
            try
            {
                var resultado = context.Courses.Where(c => c.CourseName == "Math")
                .Include(c => c.Student.FirstName == NAME).FirstOrDefault();

                if (resultado != null)
                    Console.WriteLine(resultado.CourseName.ToString());
                else
                    Console.WriteLine("We cant find the student!!");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                context.Dispose();
            }

        }

        public static void QuerryUsingSql()
        {
            //Ejemplo de querying usando FromSqlRaw
            SchoolContext context = new SchoolContext();
            List<Student> studentList = new List<Student>();
            try
            {

                studentList = context.Students.FromSqlRaw("Select *from dbo.Students").ToList();
                if (studentList != null)
                    Console.WriteLine(studentList.Find(s => s.FirstName == "Bill").FirstName);
                else
                    Console.WriteLine("We cant find the student!!");
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                context.Dispose();
            }

        }

        public static void UpdatingData()
        {
            //En este ejemplo modificamos el nombre del primer estudiante
            SchoolContext context = new SchoolContext();

            try
            {
                var std = context.Students.First<Student>();
                std.FirstName = "Steve";
                bool modified = context.SaveChanges() > 0;

                if (modified)
                    Console.WriteLine("Student modified..");
                else
                    Console.WriteLine("We cant modify the student..");

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        public static void DeletingData()
        {
            //En este ejemplo modificamos el nombre del primer estudiante
            SchoolContext context = new SchoolContext();

            try
            {
                var std = context.Students.First<Student>();
                context.Students.Remove(std);
                bool deleted = context.SaveChanges() > 0;

                if (deleted)
                    Console.WriteLine("Student deleted..");
                else
                    Console.WriteLine("We cant delete the student..");

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        public static void UpdatingOnDisconnectedScenario()
        {
            SchoolContext context = new SchoolContext();
            try
            {
                var modifiedStudent1 = new Student()
                {
                    StudentId = 1,
                    FirstName = "Bill",
                    LastName = "Madison"
                };

                var modifiedStudent2 = new Student()
                {
                    StudentId = 2,
                    FirstName = "Steve",
                    LastName = "Perez"
                };

                List<Student> modifiedStudents = new List<Student>()
                {
                    modifiedStudent1,
                    modifiedStudent2,
                };

                context.UpdateRange(modifiedStudents);
                bool modified = context.SaveChanges() > 0;
                if (modified)
                    Console.WriteLine("Modified");

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                context.Dispose();
            }

        }

        public static void DeletingOnDisconnectedScenario()
        {
            SchoolContext context = new SchoolContext();

            try
            {
                List<Student> StudentList = new List<Student>()
                {
                   new Student()
                   {
                       StudentId = 1
                   },
                   new Student()
                   {
                       StudentId = 2
                   }
                };

                context.RemoveRange(StudentList);
                bool removed = context.SaveChanges() > 0;

                if (removed)
                    Console.WriteLine("List Removed..");
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                context.Dispose();
            }
        }

        public static void ChangeTracker()
        {
            //ejemplo de hacer el tracking a los metodos que invoquen el Contexto
            //De igual manera se puede hacer para borrar, añadir entre otros
            SchoolContext contexto = new SchoolContext();
            try
            {
                var student = contexto.Students.First();
                student.LastName = "LastName Changed";
                MostrarEstado(contexto.ChangeTracker.Entries());

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                contexto.Dispose();
            }

        }

        private static void MostrarEstado(IEnumerable<EntityEntry> entries)
        {
            foreach (var entry in entries)
            {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: { entry.State.ToString()}");
            }
        }

        public static void DetachedContext()
        {
            //Este ejemplo sirve para separar un registro de la tabla de manera conectada
            SchoolContext contexto = new SchoolContext();
            try
            {
                var disconnectedEntity = new Student() { StudentId = 1, FirstName = "Bill" };
                Console.Write(contexto.Entry(disconnectedEntity).State);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public static void EntityGraphDisconnected()
        {
            //Ejemplo de hacer un graph en un escenario desconectado y actualizando data
            SchoolContext contexto = new SchoolContext();
            try
            {
                var course = new Course()
                {
                    CourseId = 1,
                    CourseName = "Math",
                    Student = new Student()
                    {
                        StudentId = 1,
                        FirstName = "Bill",
                        LastName = "Ben"
                    }
                };

                contexto.Update(course);
                MostrarEstado(contexto.ChangeTracker.Entries());

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public static void QuerryParametrizado()
        {
            //Ejemplo de querying parametrizado
            SchoolContext context = new SchoolContext();
            List<Student> studentList = new List<Student>();
            string name = "Michael";
            try
            {

                studentList = context.Students.FromSqlRaw($"Select * from dbo.Students where FirstName = '{name}'").ToList();


                if (studentList != null)
                    Console.WriteLine(studentList.Find(s => s.FirstName == name).FirstName);
                else
                    Console.WriteLine("We cant find the student!!");
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}