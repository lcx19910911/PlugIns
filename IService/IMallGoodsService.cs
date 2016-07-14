using Core.Model;
using Domain;
using Domain.Mall.Goods;
using Domain.ScratchCard;
using Model;
using System;
using System.Collections.Generic;

namespace IService
{
    /// <summary>
    /// 商品接口
    /// </summary>
    public interface IMallGoodsService
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">菜品名 - 搜索项</param>
        /// <param name="categoryId">分类id - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <param name="isRecommand">是否显示未推荐的商品</param>
        /// <returns></returns>
        PageList<Domain.Mall.Goods.List> Get_MallGoodsPageList(int pageIndex, int pageSize, string name, string categoryId, DateTime? createdTimeStart, DateTime? createdTimeEnd, bool isRecommand);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Add_MallGoods(Goods model, string DetailsImage, string DetailsSort);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Update_MallGoods(Goods model, string DetailsImage, string DetailsSort);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        bool Delete_MallGoods(string unids);

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        GoodsModel Find_MallGoods(string unid);

        /// <summary>
        /// 根据分类获取商品
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        List<Goods> Get_GoodsListByCategoryId(string cId, string personId);
    }
}
