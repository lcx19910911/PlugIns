using Core.Model;
using Domain;
using Domain.ScratchCard;
using Model;
using System;
using System.Collections.Generic;

namespace IService
{
    /// <summary>
    /// 菜品接口
    /// </summary>
    public interface IMallRecommendService
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">菜品名 - 搜索项</param>
        /// <param name="recommendCode">分类id - 搜索项</param>
        /// <returns></returns>
        PageList<Domain.Mall.Recommend.RecommendModel> Get_RecommendPageList(int pageIndex, int pageSize, string name, int recommendCode);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Add_Recommend(Recommend model);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        bool Delete_Recommend(string unids);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Add_GoodsRecommend(Recommend model);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Add_CategoryRecommend(Recommend model);

        /// <summary>
        /// 获取推荐商品
        /// </summary>
        /// <returns></returns>
        List<Tuple<Model.Goods, string>> Get_RecommendGoods(string personId);

        /// <summary>
        /// 获取推荐分类
        /// </summary>
        /// <returns></returns>
        List<Tuple<Model.Category, string>> Get_RecommendCategory(string personId);

    }
}
