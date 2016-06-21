using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace Domain.User
{
    public class UserCenterModel
    {
        public Repository.User User { get; set; }

        public int SignNum { get; set; }

        public int Score { get; set; }

        public Dictionary<string, bool> SignDic { get; set; }

        public bool TodayHadSign { get; set; } = false;
    }
}
