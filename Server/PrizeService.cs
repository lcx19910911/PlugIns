using Core.Model;
using IServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class PrizeService : BaseService, IBaseService
    {
        public PrizeService(WebClient client)
        {
            base.Client = client;
        }

    }
}
