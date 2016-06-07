using Core.Model;
using Domain;
using Domain.ScratchCard;
using Domain.UserJoinCounter;
using Repository;
using System;
using System.Collections.Generic;

namespace IService
{
    public interface IUserJoinCounterService
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
        PageList<UserJoinCounterModel> Get_UserJoinCounterPageList(int pageIndex, int pageSize, string name, string openId, int targetCode, string targetId, string SN, int prizeType, DateTime? createdTimeStart, DateTime? createdTimeEnd);

        /// <summary>
        /// 增加刮刮卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Cash(string unid);
    }
}
