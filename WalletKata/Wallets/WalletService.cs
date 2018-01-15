using System.Collections.Generic;
using WalletKata.Users;
using WalletKata.Exceptions;
using System;

namespace WalletKata.Wallets
{
    public class WalletService
    {
        ILoggedUser _IloggedUser;

        public WalletService(ILoggedUser IloggedUser)
        {
            _IloggedUser = IloggedUser;
        }
        public List<Wallet> GetWalletsByUser(User user, Func<User, List<Wallet>> FindWalletsByUser)
        {
            List<Wallet> walletList = new List<Wallet>();
            User loggedUser = _IloggedUser.GetUser();
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
                    walletList = FindWalletsByUser(user);
                }

                return walletList;
            }
            else
            {
                throw new UserNotLoggedInException();
            }      
        }         
    }

    public interface ILoggedUser
    {
        User GetUser();
    }
}