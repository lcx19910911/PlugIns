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
    /// 门店
    /// </summary>
    public interface IDinnerShopService
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">门店名 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        PageList<Domain.DinnerShop.List> Get_DinnerShopPageList(int pageIndex, int pageSize, string name, DateTime? createdTimeStart, DateTime? createdTimeEnd);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Add_DinnerShop(Domain.DinnerShop.Update model);


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Update_DinnerShop(Domain.DinnerShop.Update model, string unid);


        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        string Enable_DinnerShop(string unids);


        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        string Disable_DinnerShop(string unids);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        bool Delete_DinnerShop(string unids);


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        Domain.DinnerShop.Update Find_DinnerShop(string unid);

        /// <summary>
        /// 获取当前用户的店铺
        /// </summary>
        /// <returns></returns>
        List<ApiDinnerShopModel> Get_DinnerShopList();
    }
}
