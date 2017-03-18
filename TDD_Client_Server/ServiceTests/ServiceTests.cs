using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using UserSerivice;
using UserSerivice.Exceptions;
using UserSerivice.Implimentation;

namespace ServiceTests
{
    [TestFixture]
    public class ServiceTests
    {

        [Test]
        public void Add_NullUser_ExceptionThrown()
        {
            // arrange  
            var s = new UserService();
            // act 
            // assert is handled by ExpectedException
            Assert.Throws<ArgumentNullException>(() => s.Add(null));
        }

        [Test]
        public void Add_ExistingUser_ExceptionThrown()
        {
            // arrange  
            var s = new UserService();
            var user = new User {DateOfBirth = DateTime.Today, FirstName = "Peter", LastName = "Parker"};
            // act 
            // assert is handled by ExpectedException
            Assert.Throws<ExistingUserException>(() =>
            {
                s.Add(user);
                s.Add(user);
            });
        }

        [Test]
        public void Add_User_Success()
        {
            // arrange  
            var s = new UserService(o => o.GetHashCode());
            var user = new User {DateOfBirth = DateTime.Today, FirstName = "Peter", LastName = "Parker"};
            // act 
            s.Add(user);
            // assert
            var actual = s.SearchByLastName(user.LastName).FirstOrDefault();
            Assert.AreEqual(user.FirstName, actual.FirstName);

            s.Delete(actual);
        }

        [Test]
        public void Delete_NullUser_ExceptionThrown()
        {
            // arrange  
            var s = new UserService(o => o.GetHashCode());
            // act 
            // assert is handled by ExpectedException

            Assert.Throws<ArgumentNullException>(() => s.Delete(null));
        }

        [Test]
        public void Delete_UnexistingUser_ExceptionThrown()
        {
            // arrange  
            var s = new UserService(o => o.GetHashCode());
            var user = new User {DateOfBirth = DateTime.Today, FirstName = "Peter", LastName = "Parker", Id = 123};
            // act 
            // assert is handled by ExpectedException
            Assert.Throws<NotExistingUserException>(() => s.Delete(user));
        }

        [Test]
        public void Delete_NotValidUser_ExceptionThrown()
        {
            // arrange  
            var s = new UserService(o => o.GetHashCode());
            var user = new User();
            // act 
            // assert is handled by ExpectedException
            Assert.Throws<NotValidUserException>(() => s.Delete(user));
        }

        [Test]
        public void Delete_User_Success()
        {
            // arrange  
            var s = new UserService(o => o.GetHashCode());
            var user = new User {DateOfBirth = DateTime.Today, FirstName = "Peter", LastName = "Parker"};
            s.Add(user);
            user.Id = user.GetHashCode();
            // act 
            s.Delete(user);
            // assert is handled by ExpectedException
            var actual = s.SearchByLastName(user.LastName).FirstOrDefault();
            Assert.IsNull(actual);
        }

        //[Test]
        //public void ServiceTest()
        //{
        //    Mock<IUser> user = new Mock<IUser>();
        //    user.Setup(user1 => user1.Name).Returns("Roman");
        //    user.Setup(user1 => user1.BirthDay).Returns(DateTime.Today);
        //    user.Setup(user1 => user1.Id).Returns(12);

        //    Mock<UserService> mock = new Mock<UserService>();
        //    var hs = new HashSet<IUser>() { user.Object };
        //    mock.Setup(service => service.Storage).Returns(hs);

        //    UserService userService = new UserService();
        //    userService.Storage.Add(user.Object);


        //    Console.WriteLine(userService.Storage.Count);
        //    Assert.AreEqual(1, userService.Storage.Count);
        //    Assert.AreEqual(12, user.Object.Id);

        //    var user2 = userService.Storage.FirstOrDefault();
        //    Console.WriteLine(user2.Name);
        //    Console.WriteLine(user2.BirthDay);
        //    Console.WriteLine(user2.Id);
        //    Console.WriteLine(user2.GetType());
        //}
    }
}