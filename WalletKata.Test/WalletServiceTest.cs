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
        public class FakeWalletDAO : IWalletDAO
        {
            List<Wallet> fakeWalletList;
            public FakeWalletDAO(List<Wallet> walletList)
            {
                fakeWalletList = walletList;
            }

            public List<Wallet> FindWalletsByUser(User user)
            {
                return fakeWalletList;
            }

        }
 
        [Test]
        public void UnloggedUserTest()
        {
            //Arrange
            User unloggedUser = null;
            User user = new User();
            WalletService walletService = new WalletService(new FakeLoggedUser(unloggedUser), new FakeWalletDAO(null));

            //Assert exception
            Assert.Throws<UserNotLoggedInException>(() => walletService.GetWalletsByUser(user));

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

            List<Wallet> expected = new List<Wallet>() { new Wallet(), new Wallet() };
            WalletService walletService = new WalletService(new FakeLoggedUser(loggedUser), new FakeWalletDAO(expected));

            //act
            List<Wallet> getListWallet = walletService.GetWalletsByUser(user);
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
            List<Wallet> expected = new List<Wallet>() { new Wallet(), new Wallet() };
            WalletService walletService = new WalletService(new FakeLoggedUser(loggedUser), new FakeWalletDAO(expected));

            //act
            List<Wallet> getListWallet = walletService.GetWalletsByUser(user);
            //assert
            CollectionAssert.AreNotEqual(expected, getListWallet);
        }
    }
}

