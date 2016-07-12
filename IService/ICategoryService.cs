using Core.Model;
using Domain;
using Domain.ScratchCard;
using Model;
using System;
using System.Collections.Generic;

namespace IService
{
    /// <summary>
    /// 菜品分类
    /// </summary>
    public interface ICategoryService
    {
        #region 菜品分类

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">分类名 - 搜索项</param>
        /// <returns></returns>
        PageList<Category> Get_DinnerCategoryPageList(int pageIndex, int pageSize, string name);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Add_DinnerCategory(Category model);


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Update_DinnerCategory(Category model, string unid);


        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        bool Delete_DinnerCategory(string unids);


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        Category Find_DinnerCategory(string unid);

        /// <summary>
        /// 获取分类下拉框集合
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        List<SelectItem> Get_DinnerCategorySelectItem(string dinnerCategoryId);


        /// <summary>
        /// 获取店家的分类
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        List<SelectItem> Get_ItemByShopId(string shopId);


        #endregion

        #region 商品分类

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">分类名 - 搜索项</param>
        /// <returns></returns>
        PageList<Category> Get_MallCategoryPageList(int pageIndex, int pageSize, string name);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Add_MallCategory(Category model);


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Update_MallCategory(Category model, string unid);


        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        bool Delete_MallCategory(string unids);


        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        Category Find_MallCategory(string unid);

        /// <summary>
        /// 获取分类下拉框集合
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        List<SelectItem> Get_MallCategorySelectItem(string dinnerCategoryId);


        /// <summary>
        /// 获取店家的分类
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        List<SelectItem> Get_ItemByPersonId(string personId);


        /// <summary>
        /// 获取店家的分类
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        List<Category> Get_ListByPersonId(string personId);
        #endregion
    }
}
