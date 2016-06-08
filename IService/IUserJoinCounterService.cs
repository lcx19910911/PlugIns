using Core.Model;
using Domain;
using Domain.ScratchCard;
using Domain.UserJoinCounter;
using Repository;
using System;
using System.Collections.Generic;

namespace IService
{
    /// <summary>
    /// 用户参与记录
    /// </summary>
    public interface IUserJoinCounterService
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">活动名 - 搜索项</param>
        /// <param name="openId">微信openid - 搜索项</param>
        /// <param name="targetCode">活动类型 - 搜索项</param>
        /// <param name="targetId">活动id - 搜索项</param>
        /// <param name="SN">sn吗 - 搜索项</param>
        /// <param name="prizeType">奖品等级 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        PageList<UserJoinCounterModel> Get_UserJoinCounterPageList(int pageIndex, int pageSize, string name, string openId, int targetCode, string targetId, string SN, int prizeType, DateTime? createdTimeStart, DateTime? createdTimeEnd);

        /// <summary>
        /// 兑奖
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        string Cash(string unid);
    }
}
