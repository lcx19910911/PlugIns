using Core.Model;
using Domain;
using Domain.API;
using Domain.ScratchCard;
using System;
using System.Collections.Generic;

namespace IService
{
    /// <summary>
    /// 刮刮卡接口
    /// </summary>
    public interface IScratchCardService
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
        PageList<ScratchCardModel> Get_ScratchCardPageList(int pageIndex, int pageSize, string title, DateTime? createdTimeStart, DateTime? createdTimeEnd);



        /// <summary>
        /// 获取用户所有的刮刮卡
        /// </summary>
        /// <returns></returns>
        List<ScratchCardResult> Get_AllScratchCardList();

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Add_ScratchCard(Domain.ScratchCard.Update model);


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Update_ScratchCard(Domain.ScratchCard.Update model, string unid);


        /// <summary>
        /// 查找活动和奖品情况
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        Domain.ScratchCard.Update Find_ScratchCard(string unid);


        /// <summary>
        /// 显示活动和奖品情况
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        Domain.ScratchCard.Update Show_ScratchCard(string unid);

        /// <summary>
        /// 参与刮刮卡活动
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        PrizeResult Do_ScratchCard(string unid);


        /// <summary>
        /// 删除刮刮卡
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        bool Delete_ScratchCard(string unids);
    }
}
