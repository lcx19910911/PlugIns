using Core.Model;
using  EnumPro;
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
        public string Add_Menu(Domain.Menu.Add source)
        {
            using (DbRepository entities = new DbRepository())
            {
                var limitFlags = entities.Menu.Where(x => (x.Flag & (long)GlobalFlag.Removed) == 0).Select(x => x.LimitFlag ?? 0).ToList();
                var limitFlagAll = 0L;
                // 获取所有角色位值并集
                limitFlags.ForEach(x => limitFlagAll |= x);
                var limitFlag = 0L;
                // 从低位遍历是否为空
                for (var i = 0; i < 64; i++)
                {
                    if ((limitFlagAll & (1 << i)) == 0)
                    {
                        limitFlag = 1 << i;
                        break;
                    }
                }

                var addEntity = source.AutoMap<Domain.Menu.Add, Menu>();
                addEntity.LimitFlag = limitFlag;
                addEntity.UNID = Guid.NewGuid().ToString("N");
                addEntity.CreatedTime = DateTime.Now;
                addEntity.UpdatedTime = DateTime.Now;
                addEntity.Flag = (long)GlobalFlag.Normal;

                entities.Menu.Add(addEntity);

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
        public PageList<Menu> Get_MenuPageList(int pageIndex, int pageSize, string name)
        {
            using (DbRepository entities = new DbRepository())
            {
                var query = entities.Menu.AsQueryable();

                if (name != null)
                {
                    query = query.Where(x => x.Name.Contains(name));
                }
                return CreatePageList(query.OrderByDescending(x => x.CreatedTime), pageIndex, pageSize);
            }
        }

        /// <summary>
        /// 获取ZTree子节点
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <param name="groups">分组数据</param>
        /// <returns></returns>
        public List<ZTreeNode> GetZTreeChildren(string parentId, List<IGrouping<string, Menu>> groups)
        {
            List<ZTreeNode> ztreeNodes = new List<ZTreeNode>();
            var group = groups.FirstOrDefault(x => x.Key == parentId);
            if (group != null)
            {
                ztreeNodes = group.Select(
                    x => new ZTreeNode()
                    {
                        name = x.Name,
                        value = x.UNID,
                        children = GetZTreeChildren(x.UNID, groups)
                    }).ToList();
            }
            return ztreeNodes;
        }

        /// <summary>
        /// 获取ZTree子节点
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <param name="groups">分组数据</param>
        /// <returns></returns>
        public List<ZTreeNode> GetZTreeOperateChildren(string parentId, List<IGrouping<string, Menu>> groups)
        {
            List<ZTreeNode> ztreeNodes = new List<ZTreeNode>();
            var group = groups.FirstOrDefault(x => x.Key == parentId);
            if (group != null)
            {
                ztreeNodes = group.Select(
                    x => new ZTreeNode()
                    {
                        name = x.Name,
                        value = x.UNID,
                        children = GetZTreeOperateChildren(x.UNID, groups),
                        nocheck = true
                    }).ToList();
            }
            return ztreeNodes;
        }

        /// <summary>
        /// 获取ZTree子节点
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <param name="groups">分组数据</param>
        /// <returns></returns>
        public List<ZTreeNode> GetZTreeFlagChildren(string parentId, List<IGrouping<string, Menu>> groups)
        {
            List<ZTreeNode> ztreeNodes = new List<ZTreeNode>();
            var group = groups.FirstOrDefault(x => x.Key == parentId);
            if (group != null)
            {
                ztreeNodes = group.Select(
                    x => new ZTreeNode()
                    {
                        name = x.Name,
                        value = x.LimitFlag.ToString(),
                        children = GetZTreeFlagChildren(x.UNID, groups)
                    }).ToList();
            }
            return ztreeNodes;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public List<Domain.Menu.Item> GetChildrenMenu(string parentId, List<IGrouping<string, Menu>> groups)
        {
            List<Domain.Menu.Item> menuList = new List<Domain.Menu.Item>();
            var group = groups.FirstOrDefault(x => x.Key == parentId);
            if (group != null)
            {
                //
                menuList = group.Where(x => (x.LimitFlag & Client.LoginUser.MenuLimitFlag) != 0).Select(
                    x => new Domain.Menu.Item()
                    {
                        ClassName = x.ClassName,
                        Name = x.Name,
                        Link = x.Link,
                        Children = GetChildrenMenu(x.UNID, groups)
                    }).ToList();
            }
            return menuList;
        }


        /// <summary>
        /// 获取用户菜单权限
        /// </summary>
        /// <param name="roleLimit"></param>
        /// <returns></returns>
        public long Get_UserMenuLimit(long roleLimit)
        {
            using (DbRepository entities = new DbRepository())
            {
                long userMenuLimitFlag = 0;
                entities.Role.Where(x => (x.RoleFlag & roleLimit) != 0).Select(x=>x.LimitFlag).ToList().ForEach(x=> userMenuLimitFlag = (userMenuLimitFlag |(long)x));
                return userMenuLimitFlag;
            }
        }


        /// <summary>
        /// 根据角色Flag值判断是否有权限
        /// </summary>
        /// <param name="roleFlag">角色flag值</param>
        /// <param name="url">相对路径</param>
        /// <returns></returns>
        public static bool IsHaveAuthority(string url,long menuLimitFlag)
        {
            using (DbRepository entities = new Repository.DbRepository())
            {
                var count = entities.Menu.Where(x => x.Flag == (long)GlobalFlag.Normal).Where(x =>!string.IsNullOrEmpty(x.Link) && x.Link.IndexOf(url) != 0&&(menuLimitFlag&x.LimitFlag)!=0).Count();
                if (count > 0)
                {
                    return true;
                }
                //如果地址根本不在menu列表中，不在权限验证范围内，直接返回true
                else
                {
                    return false;;
                }

            }
        }
    }
}
