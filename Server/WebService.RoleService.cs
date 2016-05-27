using Core.Model;
using Enum;
using Extension;
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
        /// 添加
        /// </summary>
        /// <param name="source">实体</param>
        /// <returns>影响条数</returns>
        public string Add_Role(Domain.Role.Add source)
        {
            using (DbRepository entities = new DbRepository())
            {
                var roleFlags = entities.Role.Where(x => (x.Flag & (long)GlobalFlag.Removed) == 0).Select(x => x.RoleFlag ?? 0).ToList();
                var roleFlagAll = 0L;
                // 获取所有角色位值并集
                roleFlags.ForEach(x => roleFlagAll |= x);
                var roleFlag = 0L;
                // 从低位遍历是否为空
                for (var i = 0; i < 64; i++)
                {
                    if ((roleFlagAll & (1 << i)) == 0)
                    {
                        roleFlag = 1 << i;
                        break;
                    }
                }
                
                var addEntity = source.AutoMap<Domain.Role.Add, Role>();
                addEntity.RoleFlag = roleFlag;
                addEntity.UNID = Guid.NewGuid().ToString("N");
                addEntity.CreatedTime = DateTime.Now;
                addEntity.UpdatedTime = DateTime.Now;
                addEntity.Flag = (long)GlobalFlag.Normal;
                entities.Role.Add(addEntity);
                return entities.SaveChanges() > 0 ? "" : "保存出错";
            }
        }



        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">名称 - 搜索项</param>
        /// <returns></returns>
        public PageList<Role> Get_RolePageList(int pageIndex, int pageSize, string name)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Role.AsQueryable();

                if (name != null)
                {
                    query = query.Where(x => x.Name.Contains(name));
                }
                return CreatePageList(query.OrderByDescending(x => x.CreatedTime), pageIndex, pageSize);
            }
        }

        /// <summary>
        /// 获取选择项
        /// </summary>
        /// <param name="roleFlag">角色flag值</param>
        /// <returns></returns>
        public List<SelectItem> Get_RoleSelectItem(long roleFlag)
        {
            using (DbRepository entities = new DbRepository())
            {
                List<SelectItem> list = new List<SelectItem>();
                entities.Role.Where(x => x.Flag == 0).ToList().ForEach(x =>
                {
                    list.Add(new SelectItem()
                    {
                        Selected = ((x.RoleFlag&roleFlag) !=0)?true:false,
                        Text = x.Name,
                        Value = x.RoleFlag.ToString()
                    });
                });
                return list;
             
            }
        }
    }
}
