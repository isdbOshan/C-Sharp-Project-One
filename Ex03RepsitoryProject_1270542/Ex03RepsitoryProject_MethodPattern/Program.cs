using System;
using System.Linq;
using System.Collections.Generic;
using ExamProject_Repository_Method.Models;
using Ex03RepsitoryProject_MethodPattern.Repositories;
namespace Ex03RepsitoryProject_MethodPattern
{
    public static class Program
    {
        public static void Main()
        {
            RepoUnit repoUnit = new RepoUnit();
            var repoCourse = repoUnit.GetRepository<Course>();
            var c1 = new Course(1, "ESAD", 980);
            var c2 = new Course(2, "DDD", 900);
            var c3 = new Course(3, "GAVE", 920);
            repoCourse.Add(c1);
            repoCourse.Add(c2);
            repoCourse.Add(c3);
            Console.WriteLine("Insert");
            Console.WriteLine("Read");
            repoCourse.Get()
            .ToList()
            .ForEach(c => Console.WriteLine(c.GetDetails()));
            Console.WriteLine("Update");
            c2.TotalHour = 910;
            repoCourse.Update(c2);
            repoCourse.Get()
            .ToList()
            .ForEach(c => Console.WriteLine(c.GetDetails()));
            Console.WriteLine("Delete");
            repoCourse.Delete(2);
            repoCourse.Get()
            .ToList()
            .ForEach(c => Console.WriteLine(c.GetDetails()));
            Console.ReadLine();
        }
    }
}
namespace ExamProject_Repository_Method.Models
{


    public interface IEntity
    {

        int Id { get; set; }
        string GetDetails();
    }
    public class Course : IEntity

    {

        public Course() { }
        public Course(int id, string name, int totalHour)
        {
            this.Id = id;
            this.Name = name;
            this.TotalHour = totalHour;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalHour { get; set; }
        public string GetDetails()
        {
            return $"Id: {Id} Name: {Name} Course Hour:{TotalHour}";
        }
    }
    public class Trainee : IEntity
    {
        public Trainee() { }
        public Trainee(int id, string name, int courseId, int round)
        {
            this.Id = id;
            this.Name = name;
            this.CourseId = courseId;
            this.Round = round;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int CourseId { get; set; }
        public int Round { get; set; }
        public string GetDetails()
        {
            return $"Id: {Id} Name: {Name} Course Id: {CourseId} Round: {Round}";
        }
    }
}
namespace Ex03RepsitoryProject_MethodPattern.Repositories
{


    public interface IGenericRepository<T> where T : class, IEntity, new()
    {
        IList<T> Get();
        T Get(int id);
        void Add(T obj);
        void Update(T obj);
        void Delete(int id);
    }
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity, new()
    {

        IList<T> data;
        public GenericRepository(IList<T> data)
        {
            this.data = data;
        }
        public IList<T> Get()
        {
            return this.data;
        }

        public T Get(int id)
        {
            return this.data.FirstOrDefault(x => x.Id == id);
        }
        public void Add(T obj)
        {
            this.data.Add(obj);
        }
        public void Update(T obj)
        {
            var x = this.data.FirstOrDefault(o => o.Id == obj.Id);
            if (x != null)
            {
                var i = this.data.IndexOf(x);
                this.data.RemoveAt(i);
                this.data.Insert(i, obj);
            }
        }
        public void Delete(int id)
        {
            var x = this.data.FirstOrDefault(o => o.Id == id);
            if (x != null)
            {
                var i = this.data.IndexOf(x);
                this.data.RemoveAt(i);
            }
        }

    }
    public interface IRepoUnit
    {

        IGenericRepository<T> GetRepository<T>() where T : class, IEntity, new();
    }
    public class RepoUnit : IRepoUnit
    {
        public IGenericRepository<T> GetRepository<T>() where T : class, IEntity, new()
        {
            return new GenericRepository<T>(new List<T>());
        }
    }
}
