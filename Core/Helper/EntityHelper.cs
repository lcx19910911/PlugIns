using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper
{
    public class EntityHelper
    {
        /// <summary>
        /// 判断是否有该类型的DbSet
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns></returns>
        public static bool IsHaveDbSet(Type entitiesType, Type dbSetGenericType)
        {
            var count = entitiesType.GetProperties().Where(x =>
            {
                if (x.PropertyType.Name == "DbSet`1")
                {
                    if (x.PropertyType.GenericTypeArguments.Where(g => g.Name == dbSetGenericType.Name).Count() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }).Count();
            if (count > 0)
                return true;
            else
                return false;
        }
    }
}
