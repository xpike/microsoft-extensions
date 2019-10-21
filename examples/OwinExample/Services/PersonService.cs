using OwinExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OwinExample.Services
{
    public interface IPersonService
    {
        IEnumerable<Person> GetAll();

        Person GetById(int id);
    }

    public class PersonService : IPersonService
    {
        static Dictionary<int, Person> people = new Dictionary<int, Person> 
        {
            { 1, new Person { Id = 1, Name = "Elmer Fudd" } },
            { 2, new Person { Id = 2, Name = "Homer Simpson" } }
        };

        public Person GetById(int id)
        {
            return people[id];
        }

        public IEnumerable<Person> GetAll()
        {
            return people.Values;
        }
    }
}