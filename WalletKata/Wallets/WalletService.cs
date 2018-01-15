using System.Collections.Generic;
using WalletKata.Users;
using WalletKata.Exceptions;
using System;

namespace WalletKata.Wallets
{
    public class WalletService
    {
        ILoggedUser _ILoggedUser;
        IWalletDAO _IWalletDAO;

        public WalletService(ILoggedUser IloggedUser, IWalletDAO IwalletDAO)
        {
            _ILoggedUser = IloggedUser;
            _IWalletDAO = IwalletDAO;
        }
        public List<Wallet> GetWalletsByUser(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException();
            }

            List<Wallet> walletList = new List<Wallet>();
            User loggedUser = _ILoggedUser.GetUser();
            bool isFriend = false;

            if (loggedUser != null)
            {
                foreach (User friend in user.Friends)
                {
                    if (friend.Equals(loggedUser))
                    {
                        isFriend = true;
                        break;
                    }
                }

                if (isFriend)
                {
                    walletList = _IWalletDAO.FindWalletsByUser(user);
                }

                return walletList;
            }
            else
            {
                throw new UserNotLoggedInException();
            }
        }
    }

    public interface IWalletDAO
    {
        List<Wallet> FindWalletsByUser(User user);
    }

    public interface ILoggedUser
    {
        User GetUser();
    }
}