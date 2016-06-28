using Core.Model;
using Domain;
using Domain.ScratchCard;
using Repository;
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
        /// <param name="categoryId">分类id - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        PageList<Domain.Mall.Recommend.RecommendModel> Get_RecommendPageList(int pageIndex, int pageSize, string name, int targetCode);

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
        List<Goods> Get_RecommendGoods();

        /// <summary>
        /// 获取推荐分类
        /// </summary>
        /// <returns></returns>
        List<Category> Get_RecommendCategory();

    }
}
