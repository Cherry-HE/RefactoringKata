using System.Collections.Generic;
using WalletKata.Users;
using WalletKata.Exceptions;
using System;

namespace WalletKata.Wallets
{
    public class WalletService
    {
        User loggedUser;

        public WalletService(User loggedUser)
        {
            this.loggedUser = loggedUser;
        }
        public List<Wallet> GetWalletsByUser(User user, Func<User, List<Wallet>> FindWalletsByUser)
        {
            List<Wallet> walletList = new List<Wallet>();
            //User loggedUser = UserSession.GetInstance().GetLoggedUser();
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
}