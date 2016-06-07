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
    public interface IUserService
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        bool Update_User(WXUser model);

        /// <summary>
        /// 查找活动和奖品情况
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        PageList<User> Get_UserPageList(int pageIndex, int pageSize, string name, string openId, DateTime? createdTimeStart, DateTime? createdTimeEnd);
    }
}
