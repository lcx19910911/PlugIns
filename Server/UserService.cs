using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IServer;
using Core.Model;

namespace Server
{
    public class UserService:BaseService,IBaseService
    {
        public UserService(WebClient client)
        {
            base.Client = client;
        }

    }
}
