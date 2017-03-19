using System;
using System.Collections.Generic;
using System.Linq;
using UserSerivice.Exceptions;
using UserSerivice.Interfaces;

namespace UserSerivice.Implimentation
{
    
    public class UserService : MarshalByRefObject,IService<User>
    {
        private bool isMaster = false;
        public void Master()
        {
            isMaster = true;
        }
        //TODO: in iFace
        public AddRemoveNotifier notifier;
        private ILogger log;//TODO: static or singleton

        public string GetGenerator()
        {
            return IdGenerator.IdCreator.Method.ToString();
        }

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
            log = new Logger();
            log.LogInfo("ctor ()");
            //TODO:Eq comparer
            Storage = new HashSet<User>(new UserComparer());
            IdGenerator = new UserIdGenerator(o => o.GetHashCode());
            notifier = new AddRemoveNotifier();
        }

        public UserService(bool swh)
        {
            if (swh)
            {
                log = new Logger();
                log.LogInfo("ctor (switch)");
            }
            else
            {
                log = new Logger();
                log.LogInfo("ctor (without loging)");
                log = null;
            }
            //TODO:Eq comparer
            Storage = new HashSet<User>(new UserComparer());
            IdGenerator = new UserIdGenerator(o => o.GetHashCode());
            notifier = new AddRemoveNotifier();
        }

        public virtual HashSet<User> Storage { get; private set; }

        public void Add(User user)
        {
            if (!isMaster)
            {
                log?.LogWarn("Trying to you SLAVE instance as MASTER one");
                log?.LogError(new NotMasterException());
                throw new NotMasterException();
            }
            CheckArguments((object) user);
            if (this.Storage.Contains(user))
            {
                log?.LogTrace($"The user named {user.FirstName} is already exist in storage");
                log?.LogError(new ExistingUserException());
                throw new ExistingUserException();
            }


            log?.LogInfo($"adding user named {user.FirstName}");

            user.Id = this.IdGenerator.IdCreator(user);
            this.Storage.Add(user);
            notifier.AddNotification(this, new UserEventArgs(user));
            //OnAddRemoveEvent();
        }

        public void Delete(User user)
        {
            if (!isMaster)
            {
                log?.LogWarn("Trying to you SLAVE instance as MASTER one");
                log?.LogError(new NotMasterException());
                throw new NotMasterException();
            }

            CheckArguments(user);
            if (!this.Storage.Remove(user))
            {
                log?.LogTrace($"The user named {user.FirstName} doesn't exist in storage");
                log?.LogError(new NotExistingUserException());
                throw new NotExistingUserException();
            }

            notifier.RemoveNotification(this, new UserEventArgs(user));
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
            log?.LogInfo($"SearchByFirstName user named: {firstname}");
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
            log?.LogInfo($"SearchByLastName user named: {lastName}");
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

        public void UpdateStorage(object sender, EventArgs args)//TODO: It's a bind
        {
            Console.WriteLine("Upt Storage");
            log?.LogInfo($"UpdateStorage in {this} provided by {sender}");
            Storage = ((UserService) sender).Storage;
        }

        public void AddToStorage(object sender, EventArgs args)
        {
            Console.WriteLine("Upt Storage");
            log?.LogInfo($"UpdateStorage in {this} provided by {sender}");
            Storage.Add(((UserEventArgs)args).User);
        }

        public void RemoveFromStorage(object sender, EventArgs args)
        {
            Console.WriteLine("Upt Storage");
            log?.LogInfo($"UpdateStorage in {this} provided by {sender}");
            Storage.Remove(((UserEventArgs)args).User);
        }

        public IEnumerable<string> Show()
        {
            return Storage.Select(u => u.ToString()).ToList();
        }
    }
}