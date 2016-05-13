using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServer
{
    public interface IBaseService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="source">实体</param>
        void Add<TSource, TEntity>(TSource source) where TEntity : class;

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="unid">unid</param>
        /// <param name="source">实体</param>
        void Update<TSource, TEntity>(string unid, TSource source) where TEntity : class;


        /// <summary>
        /// 查询单个
        /// </summary>
        /// <param name="unid">unid</param>
        /// <returns>返回实体</returns>
        TEntity Find<TEntity>(string unid) where TEntity : class;

        /// <summary>
        /// 查找所有
        /// </summary>
        /// <returns></returns>
        List<TEntity> GetAll<TEntity>() where TEntity : class;
    }
}
