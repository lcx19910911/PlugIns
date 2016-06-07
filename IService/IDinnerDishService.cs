﻿using Core.Model;
using Domain;
using Domain.ScratchCard;
using Repository;
using System;
using System.Collections.Generic;

namespace IService
{
    public interface IDinnerDishService
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
        PageList<Domain.DinnerDish.List> Get_DinnerDishPageList(int pageIndex, int pageSize, string name, string categoryId, DateTime? createdTimeStart, DateTime? createdTimeEnd);

        /// <summary>
        /// 增加刮刮卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Add_DinnerDish(DinnerDish model);


        /// <summary>
        /// 增加刮刮卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Update_DinnerDish(DinnerDish model, string unid);


        /// <summary>
        /// 查找活动和奖品情况
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        bool Delete_DinnerDish(string unids);


        /// <summary>
        /// 查找活动和奖品情况
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        DinnerDish Find_DinnerDish(string unid);

        /// <summary>
        /// 参与刮刮卡活动
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        List<DinnerDish> Get_DishListByCategoryId(string cId);
    }
}
