﻿using Core.Model;
using Domain;
using Domain.ScratchCard;
using System;
using System.Collections.Generic;

namespace IService
{
    /// <summary>
    /// 点餐订单
    /// </summary>
    public interface IDinnerOrderService
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="state">状态 - 搜索项</param>
        /// <param name="orderNum">订单号 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        PageList<Domain.Dinner.Order.List> Get_DinnerOrderPageList(int pageIndex, int pageSize, int state, string orderNum, DateTime? createdTimeStart, DateTime? createdTimeEnd);

        /// <summary>
        /// 增加订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Add_DinnerOrder(string info);


        /// <summary>
        /// 获取2两小时内的订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Domain.Dinner.OrderModel Get_CurrentOrder();


        /// <summary>
        /// 确认订单
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        bool Confirm_DinnerOrder(string unid);


        /// <summary>
        /// 订单无效
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        bool Invalid_DinnerOrder(string unid);
    }
}
