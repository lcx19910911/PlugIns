using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Core.Model
{
    public class UserIdentity<TKey> : IIdentity
    {
        public UserIdentity(IUser<TKey> user)
        {
            if (user != null)
            {
                IsAuthenticated = true;
                PersonId = user.PersonId;
                ShopId = user.ShopId;
                Name = user.LoginName.ToString();
            }
        }

        public string AuthenticationType
        {
            get { return "CustomAuthentication"; }
        }

        public TKey PersonId { get; private set; }

        public TKey ShopId { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public string Name { get; private set; }
        
    }

    public class UserPrincipal<TKey> : IPrincipal
    {
        public UserPrincipal(UserIdentity<TKey> identity)
        {
            Identity = identity;
        }

        public UserPrincipal(IUser<TKey> user)
            : this(new UserIdentity<TKey>(user))
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public UserIdentity<TKey> Identity { get; private set; }

        IIdentity IPrincipal.Identity
        {
            get { return Identity; }
        }


        bool IPrincipal.IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }

    public interface IUser<T>
    {
        T PersonId { get; set; }
        T ShopId { get; set; }
        string LoginName { get; set; }
    }

    public class ApiLoginUsere:IUser<string>
    {
        public string PersonId { get; set; }
        public string ShopId { get; set; }
        public string LoginName { get; set; }
    }
}