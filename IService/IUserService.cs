using Core.AuthAPI;
using Core.Model;
using Domain;
using Domain.ScratchCard;
using MPUtil.UserMng;
using Repository;
using System;
using System.Collections.Generic;

namespace IService
{
    /// <summary>
    /// 微信用户接口
    /// </summary>
    public interface IUserService
    {
       /// <summary>
       /// 编辑管理用户
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
        bool Update_User(WXUser model);

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">昵称</param>
        /// <param name="openId"></param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        PageList<User> Get_UserPageList(int pageIndex, int pageSize, string name, string openId, DateTime? createdTimeStart, DateTime? createdTimeEnd);
    }
}
