using Core.Model;
using Domain;
using Domain.ScratchCard;
using Repository;
using System;
using System.Collections.Generic;

namespace IService
{
    public interface IDinnerCategoryService
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
        PageList<DinnerCategory> Get_DinnerCategoryPageList(int pageIndex, int pageSize, string name);

        /// <summary>
        /// 增加刮刮卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Add_DinnerCategory(DinnerCategory model);


        /// <summary>
        /// 增加刮刮卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Update_DinnerCategory(DinnerCategory model, string unid);


        /// <summary>
        /// 查找活动和奖品情况
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        bool Delete_DinnerCategory(string unids);


        /// <summary>
        /// 查找活动和奖品情况
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        DinnerCategory Find_DinnerCategory(string unid);

        /// <summary>
        /// 参与刮刮卡活动
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        List<SelectItem> Get_DinnerCategorySelectItem(string dinnerCategoryId);


        /// <summary>
        /// 参与刮刮卡活动
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        List<SelectItem> Get_ItemByShopId(string shopId);
    }
}
