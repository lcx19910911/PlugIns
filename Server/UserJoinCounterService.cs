using Core.Model;
using IServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class UserJoinCounterService : BaseService, IBaseService
    {
        public UserJoinCounterService(WebClient client)
        {
            base.Client = client;
        }

    }
}

