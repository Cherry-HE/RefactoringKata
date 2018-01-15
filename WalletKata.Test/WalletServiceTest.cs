using NUnit.Framework;
using System.Collections.Generic;
using WalletKata.Exceptions;
using WalletKata.Users;
using WalletKata.Wallets;

namespace WalletKata.Test
{
    [TestFixture]
    public class WalletServiceTest
    {
        public class FakeLoggedUser : ILoggedUser
        {
            User fakeUser;
            public FakeLoggedUser(User user)
            {
                fakeUser = user;
            }
            public User GetUser()
            {
                return fakeUser;
            }
        }
        [Test]
        public void UnloggedUserTest()
        {
            //Arrange
            User unloggedUser = null;
            User user = new User();
            WalletService walletService = new WalletService(new FakeLoggedUser(null));

            //Assert exception
            Assert.Throws<UserNotLoggedInException>(()=> walletService.GetWalletsByUser(user, delegate (User u)
            {
                return new List<Wallet>();
            }));

        }

        [Test]
        public void LoggedUserIsFriendTest()
        {
            //Arrange
            User user = new User();
            User friend = new User();
            User loggedUser = new User();
            user.AddFriend(friend);
            user.AddFriend(loggedUser);
            WalletService walletService = new WalletService(new FakeLoggedUser(loggedUser));

            List<Wallet> expected = new List<Wallet>() { new Wallet(),new Wallet()};

            //act
            List<Wallet> getListWallet =  walletService.GetWalletsByUser(user, delegate (User u)
            {
                return expected;
            });
            //assert
            CollectionAssert.AreEqual(expected, getListWallet);
        }

        [Test]
        public void LoggedUserIsNotFriendTest()
        {
            //Arrange
            User user = new User();
            User friend = new User();
            User loggedUser = new User();
            user.AddFriend(friend);
            WalletService walletService = new WalletService(new FakeLoggedUser(loggedUser));

            List<Wallet> expected = new List<Wallet>() { new Wallet(), new Wallet() };

            //act
            List<Wallet> getListWallet = walletService.GetWalletsByUser(user, delegate (User u)
            {
                return expected;
            });
            //assert
            CollectionAssert.AreNotEqual(expected, getListWallet);
        }
    }
}
