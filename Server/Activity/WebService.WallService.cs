using Core.Extensions;
using Core.Model;
using Domain.Wall;
using  EnumPro;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public partial class WebService
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">标题 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        public WebResult<PageList<WallModel>> Get_WallPageList(int pageIndex, int pageSize, string name, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Wall.AsQueryable().Where(x => (x.Flag & (long)GlobalFlag.Removed) == 0);
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.Name.Contains(name));
                }
                //query = query.Where(x => x.AppId.Equals(this.Client.AppId));
                if (createdTimeStart != null)
                {
                    query = query.Where(x => x.OngoingTime >= createdTimeStart);
                }
                if (createdTimeEnd != null)
                {
                    createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                    query = query.Where(x => x.OverTime < createdTimeEnd);
                }

                var list = new List<WallModel>();
                var count = query.Count();
                query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList().ForEach(x =>
                {
                    if (x != null)
                    {
                        list.Add(new WallModel()
                        {
                            UNID = x.UNID,
                            CreatedTime = x.CreatedTime,
                            Sponsor = x.Sponsor,
                            Name = x.Name,
                            OngoingTime = x.OngoingTime,
                            OverTime = x.OverTime,
                            UpdatedTime = x.UpdatedTime
                        });
                    }
                });
                return ResultPageList(list, pageIndex, pageSize, count);
            }
        }


    }
}
