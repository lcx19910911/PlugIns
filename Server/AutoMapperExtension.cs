using Core;
using Core.Attributes;
using Core.Helper;
using Core.Model;
using Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extension
{
    public static class AutoMapperExtension
    {



        /// <summary>
        /// 自动映射
        /// </summary>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">原始对象</param>
        /// <param name="destination">目标对象</param>
        /// <returns>映射结果</returns>
        public static TDestination AutoMap<TSource, TDestination>(this TSource source, TDestination destination) where TDestination : class
        {

            List<TSource> sourceList = new List<TSource>() { source };
            List<TDestination> destinationList = new List<TDestination>() { destination };

            return sourceList.AutoMap<TSource, TDestination>(destinationList).First();
        }

        /// <summary>
        /// 自动映射
        /// </summary>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">原始对象</param>
        /// <returns>映射结果</returns>
        public static TDestination AutoMap<TSource, TDestination>(this TSource source) where TDestination : class
        {
            List<TSource> list = new List<TSource>() { source };
            return list.AutoMap<TSource, TDestination>().First();
        }

        /// <summary>
        /// 自动映射列表
        /// </summary>
        /// <typeparam name="TSource">原始类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="sourceList">原始列表</param>
        /// <returns></returns>
        public static List<TDestination> AutoMap<TSource, TDestination>(this List<TSource> sourceList, List<TDestination> destinationList = null) where TDestination : class
        {
            Dictionary<object, TDestination> targetDictionary = new Dictionary<object, TDestination>();

            if (destinationList != null)
            {
                //如果有目标原始值时的操作

                foreach (var source in sourceList)
                {
                    //创建autoMapper映射
                    AutoMapper.Mapper.CreateMap<TSource, TDestination>();

                    //使用autoMapper映射                
                    int index = sourceList.IndexOf(source);
                    if (destinationList.Count > index)
                    {
                        TDestination target = AutoMapper.Mapper.Map<TSource, TDestination>(source, destinationList[index]);
                        targetDictionary.Add(source, target);
                    }
                    else
                    {

                        TDestination target = AutoMapper.Mapper.Map<TSource, TDestination>(source);
                        targetDictionary.Add(source, target);
                    }
                }
            }
            else
            {

                foreach (var source in sourceList)
                {
                    //创建autoMapper映射
                    AutoMapper.Mapper.CreateMap<TSource, TDestination>();

                    //使用autoMapper映射                
                    TDestination target = AutoMapper.Mapper.Map<TSource, TDestination>(source);
                    targetDictionary.Add(source, target);
                }
            }

            //取出包含EnumAutoMapperAttribute特性的属性
            var enumAutoMapperPropertys = typeof(TDestination).GetProperties().Where(x => x.GetCustomAttributes(typeof(EnumAutoMapperAttribute), false).Count() > 0);
            //遍历属性
            foreach (var enumAutoMapProperty in enumAutoMapperPropertys)
            {
                var enumAutoMapperAttribute = enumAutoMapProperty.GetCustomAttributes(typeof(EnumAutoMapperAttribute), false).FirstOrDefault() as EnumAutoMapperAttribute;
                if (enumAutoMapperAttribute != null)
                {
                    var dictionary = EnumHelper.GetDictionary(enumAutoMapperAttribute.EnumType);
                    foreach (var target in targetDictionary)
                    {
                        //取枚举值
                        var key = Convert.ToInt32(target.Key.GetType().GetProperty(enumAutoMapperAttribute.SourceProperty).GetValue(target.Key));
                        //赋值上新属性值
                        if (dictionary.Keys.Contains(key))
                        {
                            enumAutoMapProperty.SetValue(target.Value, dictionary[key]);
                        }
                    }
                }
            }



            //取出包含DataAutoMapperAttribute特性的属性
            var dataAutoMapperPropertys = typeof(TDestination).GetProperties().Where(x => x.GetCustomAttributes(typeof(DataAutoMapperAttribute), false).Count() > 0);
            //遍历属性
            foreach (var dataAutoMapProperty in dataAutoMapperPropertys)
            {
                var dataAutoMapperAttribute = dataAutoMapProperty.GetCustomAttributes(typeof(DataAutoMapperAttribute), false).FirstOrDefault() as DataAutoMapperAttribute;
                if (dataAutoMapperAttribute != null)
                {
                    Dictionary<object, object> dictionary = new Dictionary<object, object>();
                    List<string> sourceMatchPropertyValues = new List<string>();
                    //目标类型
                    var targetEntityType = dataAutoMapperAttribute.TargetEntityType;
                    foreach (var source in sourceList)
                    {
                        //原始属性值
                        var sourceMatchPropertyValue = source.GetType().GetProperty(dataAutoMapperAttribute.SourceMatchProperty).GetValue(source);
                        if (sourceMatchPropertyValue != null)
                        {
                            sourceMatchPropertyValues.Add(sourceMatchPropertyValue.ToString());
                        }
                    }
                    sourceMatchPropertyValues = sourceMatchPropertyValues.Distinct().ToList();
                    List<string> sqlParamSourceValues = new List<string>();
                    sourceMatchPropertyValues.ForEach(x =>
                    {
                        sqlParamSourceValues.Add("'" + x + "'");
                    });
                    DbContext entities;
                    if (EntityHelper.IsHaveDbSet(typeof(PlugInsEntities), targetEntityType))
                    {
                        entities = new PlugInsEntities();
                    }
                    else
                    {
                        return null;
                    }

                    if (sourceMatchPropertyValues.Count > 0)
                    {
                        string tableName = "";
                        var attribute = targetEntityType.GetCustomAttribute(typeof(TableAttribute));
                        if (attribute != null)
                        {
                            var tableAttribute = attribute as TableAttribute;
                            tableName = tableAttribute.Name;
                        }
                        string sql = "SELECT [" + dataAutoMapperAttribute.TargetMatchProperty + "] AS [Key],[" + dataAutoMapperAttribute.TargetValueProperty + "] AS [Value] FROM [dbo].[" + tableName + "] WHERE [" + dataAutoMapperAttribute.TargetMatchProperty + "] in (" + string.Join(",", sqlParamSourceValues) + ")";
                        var keyValue = entities.Database.SqlQuery<QueryKeyValue>(sql);

                        foreach (var item in keyValue)
                        {
                            dictionary.Add(item.Key.Trim(), item.Value);
                        }
                    }
                    entities.Dispose();
                    foreach (var target in targetDictionary)
                    {
                        //取原始属性值
                        var key = target.Key.GetType().GetProperty(dataAutoMapperAttribute.SourceMatchProperty).GetValue(target.Key);
                        if (key != null)
                        {
                            //赋值上新属性值
                            if (dictionary.Keys.Contains(key))
                            {
                                dataAutoMapProperty.SetValue(target.Value, dictionary[key]);
                            }
                        }
                    }

                }
            }

            return targetDictionary.Select(x => x.Value).ToList();
        }

        /// <summary>
        /// 自动映射分页列表
        /// </summary>
        /// <typeparam name="TSource">原始类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">原始对象</param>
        /// <returns></returns>
        public static PageList<TDestination> AutoMap<TSource, TDestination>(this PageList<TSource> source) where TDestination : class
        {
            PageList<TDestination> list = new PageList<TDestination>(source.List.AutoMap<TSource, TDestination>(), source.PageIndex, source.PageSize, source.RecordCount);
            return list;
        }

    }
}
