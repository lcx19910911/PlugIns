using Core.Code;
using Core.Model;
using Core.Util;
using Core.Web;
using Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Extension;

namespace Service
{
    public class BaseService
    {

        public HttpContext ContextCurrent { get; set; }

        public WebClient Client
        {
            get
            {
                return new WebClient(ContextCurrent);
            }
        }

        /// <summary>
        /// list转换pageList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="allList">需要分页的数据</param>
        /// <returns></returns>
        private PageList<T> ConvertPageList<T>(List<T> allList, int pageIndex, int pageSize)
        {
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize <= 0 ? 10 : pageSize;
            int skip = (pageIndex - 1) * pageSize;
            var list = allList?.Skip(skip).Take(pageSize).ToList();
            return new PageList<T>(list, pageIndex, pageSize, allList == null ? 0 : allList.LongCount());
        }

        /// <summary>
        /// list转换pageList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="List">需要分页的数据</param>
        /// <returns></returns>
        private PageList<T> ConvertPageList<T>(List<T> list, int pageIndex, int pageSize, int recoredCount)
        {

            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize <= 0 ? 10 : pageSize;
            return new PageList<T>(list, pageIndex, pageSize, recoredCount);
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void ClearCache()
        {
            LogHelper.WriteCustom(string.Format("ClearCache At {0} \r\n", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")), @"ClearCache\");
            CacheHelper.Clear();
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="source">实体</param>
        /// <returns>影响条数</returns>
        public void Add<TSource, TEntity>(TSource source) where TEntity : class
        {
            using (DbRepository entities = new DbRepository())
            {
                var addEntity = source.AutoMap<TSource, TEntity>();
                entities.Entry(addEntity).State = System.Data.Entity.EntityState.Added;
                entities.SaveChanges();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unids">unid，多个id用逗号分隔</param>
        /// <returns>影响条数</returns>
        public int Delete<TEntity>(string unids) where TEntity : class
        {
            using (DbRepository entities = new DbRepository())
            {
                //按逗号分隔符分隔开得到unid列表
                var unidArray = unids.Split(',');
                //遍历unid列表逐个删除
                foreach (var unid in unidArray)
                {
                    DbSet<TEntity> dbSet = entities.Set<TEntity>();
                    var entity = dbSet.Find(unid);
                    if (entity != null)
                    {
                        entities.Entry(entity).State = EntityState.Deleted;
                    }
                }
                return entities.SaveChanges();
            }
        }

        /// <summary>
        /// 查找所有
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAll<TEntity>() where TEntity : class
        {
            using (DbRepository entities = new DbRepository())
            {
                DbSet<TEntity> dbSet = entities.Set<TEntity>();
                return dbSet.ToList();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="unid">unid</param>
        /// <param name="source">实体</param>
        public void Update<TSource, TEntity>(string unid, TSource source) where TEntity : class
        {
            using (DbRepository entities = new DbRepository())
            {
                DbSet<TEntity> dbSet = entities.Set<TEntity>();
                var sourceEntity = dbSet.Find(unid);
                if (sourceEntity != null)
                {
                    source.AutoMap<TSource, TEntity>(sourceEntity);
                }
                entities.SaveChanges();
            }
        }



        public WebResult<T> Result<T>(T model)
        {
            return Result(model, ErrorCode.sys_success);
        }

        public WebResult<T> Result<T>(T model, ErrorCode code)
        {
            return new WebResult<T> { Code = code, Result = model };
        }

        public WebResult<T> Result<T>(T model, ErrorCode code, string append)
        {
            return new WebResult<T> { Code = code, Result = model, Append = append };
        }
        public WebResult<PageList<T>> ResultPageList<T>(List<T> model, int pageIndex, int pageSize)
        {
            return Result(ConvertPageList<T>(model, pageIndex, pageSize));
        }

        public WebResult<PageList<T>> ResultPageList<T>(List<T> model, int pageIndex, int pageSize, int recoredCount)
        {
            return Result(ConvertPageList<T>(model, pageIndex, pageSize, recoredCount));
        }

        public WebResult<PageList<T>> ResultPageList<T>(List<T> model, int pageIndex, int pageSize, ErrorCode code)
        {
            return Result(ConvertPageList<T>(model, pageIndex, pageSize), code);
        }

        /// <summary>
        /// 创建分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">查询对象</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        protected PageList<T> CreatePageList<T>(IQueryable<T> queryable, int pageIndex, int pageSize)
        {
            int recordCount = 0;
            try
            {
                recordCount = queryable.Count();
                List<T> list = queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return new PageList<T>(list, pageIndex, pageSize, recordCount);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 创建分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">查询对象</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        protected PageList<T> CreatePageList<T>(List<T> list, int pageIndex, int pageSize, int recordCount)
        {
            try
            {
                return new PageList<T>(list, pageIndex, pageSize, recordCount);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 创建分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable">查询对象</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        protected List<T> CreateList<T>(IQueryable<T> queryable, int pageIndex, int pageSize)
        {
            try
            {
                List<T> list = queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
