using MTCGServer.DAL.Users;
using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Test
{
    public class TestMemoryUserDao
    {
        //Insert of a user which did
        [Test]
        public void TestInsertNotExistingUserReturnsTrue()
        {
            //arrange
            MemoryUserDao inMemoryDatabase = new MemoryUserDao();
            Credentials credentials = new Credentials("test", "password");
            User user = new User(credentials);

            //act
            bool result = inMemoryDatabase.InsertUser(user);

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void TestInsertAlreadyExistingUserReturnsFalse()
        {
            //arrange
            MemoryUserDao inMemoryDatabase = new MemoryUserDao();
            Credentials credentials1 = new Credentials("test", "password1");
            Credentials credentials2 = new Credentials("test", "password2");
            User user1 = new User(credentials1);
            User user2 = new User(credentials2);
            inMemoryDatabase.InsertUser(user1);

            //act
            bool result = inMemoryDatabase.InsertUser(user2);

            //assert
            Assert.IsFalse(result);
        }
    }
}
