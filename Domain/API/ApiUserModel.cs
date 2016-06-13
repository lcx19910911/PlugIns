using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.API
{
    public class ApiUserModel
    {
        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImgUrl { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public Nullable<int> Sex { get; set; }
        public string MobilePhone { get; set; }
        public System.DateTime CreatedTime { get; set; }
    }
}
