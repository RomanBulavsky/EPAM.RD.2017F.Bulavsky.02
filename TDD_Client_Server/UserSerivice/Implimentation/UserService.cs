using System;
using System.Collections.Generic;
using System.Linq;
using UserSerivice.Exceptions;
using UserSerivice.Interfaces;

namespace UserSerivice.Implimentation
{
    public class UserService : IService<User>
    {
        private IIdGenerator IdGenerator { get; }

        public UserService(Func<object, int> IdGenerator)
        {
            this.IdGenerator = new UserIdGenerator(IdGenerator);
            this.Storage = new HashSet<User>(new UserComparer());
        }

        public UserService(HashSet<User> storage) : this()
        {
            Storage = storage;
        }

        public UserService()
        {
            //TODO:Eq comparer
            Storage = new HashSet<User>(new UserComparer());
            IdGenerator = new UserIdGenerator(o => o.GetHashCode());
        }

        public virtual HashSet<User> Storage { get; private set; }

        public void Add(User user)
        {
            CheckArguments((object)user);
            if (this.Storage.Contains(user))
            {
                throw new ExistingUserException();
            }

            user.Id = this.IdGenerator.IdCreator(user);
            this.Storage.Add(user);
            //OnAddRemoveEvent();
        }

        public void Delete(User user)
        {
            CheckArguments(user);
            if (!this.Storage.Remove(user))
            {
                throw new NotExistingUserException();
            }
            //OnAddRemoveEvent();
        }
        /// <summary>
        /// Searches <see cref="User"/> entities by <see cref="firstname"/>
        /// </summary>
        /// <param name="firstname"> <see cref="string"/></param>
        /// <returns> <see cref="IEnumerable{User}"/> that consist the <see cref="firstname"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public IEnumerable<User> SearchByFirstName(string firstname)
        {
            CheckArguments(firstname);
            return this.Storage.Where(u => u.FirstName.Equals(firstname));
        }

        /// <summary>
        /// Searches <see cref="User"/> entities by <see cref="lastName"/>
        /// </summary>
        /// <param name="lastName"> <see cref="string"/></param>
        /// <returns> <see cref="IEnumerable{User}"/> that consist the <see cref="lastName"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public IEnumerable<User> SearchByLastName(string lastName)
        {
            CheckArguments(lastName);
            return this.Storage.Where(u => u.LastName.Equals(lastName));
        }

        private static void CheckArguments(object o)
        {
            if (o == null)
            {
                throw new ArgumentNullException();
            }

            if (o.ToString().Equals(string.Empty))
            {
                throw new ArgumentException("Empty String");
            }
        }

        private static void CheckArguments(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            if (user.Id == null || string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName))
            {
                throw new NotValidUserException();
            }
        }
    }
}
