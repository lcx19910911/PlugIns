using Core.Model;
using Domain;
using Domain.API;
using Domain.ScratchCard;
using Repository;
using System;
using System.Collections.Generic;

namespace IService
{
    /// <summary>
    /// 刮刮卡接口
    /// </summary>
    public interface IMallOrderService
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">商品名 - 搜索项</param>
        /// <param name="nickName">用户昵称 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        PageList<Domain.Mall.Order.OrderModel> Get_OrderPageList(int pageIndex, int pageSize, string unid, string name,string nickName, DateTime? createdTimeStart, DateTime? createdTimeEnd);


        /// <summary>
        /// 获取用户所有的刮刮卡
        /// </summary>
        /// <returns></returns>
        List<Tuple<GoodsOrder,Goods>> Get_AllOrderList(string openId,string personId);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Add_Order(GoodsOrder model);

    }
}
