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
    /// 拼图接口
    /// </summary>
    public interface IPuzzleService
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">名称 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        PageList<Puzzle> Get_PuzzlePageList(int pageIndex, int pageSize, string name, DateTime? createdTimeStart, DateTime? createdTimeEnd);


        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Add_Puzzle(Puzzle model);


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string Update_Puzzle(Puzzle model, string unid);


        /// <summary>
        /// 查找拼图
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        Puzzle Find_Puzzle(string unid);



        /// <summary>
        /// 删除拼图
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        bool Delete_Puzzle(string unids);

        /// <summary>
        /// 获取下一个拼图
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        Puzzle Get_NextPuzzle(string unid,string openId,string personId);

        /// <summary>
        /// 完成拼图结果
        /// </summary>
        /// <returns></returns>
        Tuple<bool, string, string> Complete(string unid);
    }
}
