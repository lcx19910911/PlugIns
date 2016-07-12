using Core.AuthAPI;
using Core.Model;
using Domain;
using Domain.ScratchCard;
using Model;
using System;
using System.Collections.Generic;

namespace IService
{
    /// <summary>
    /// 人员接口
    /// </summary>
    public interface IPersonService
    {
       /// <summary>
       /// 管理人员信息
       /// </summary>
       /// <param name="data">接口返回对象</param>
       /// <param name="account">账号</param>
       /// <param name="password">密码</param>
       /// <returns></returns>
        Person Manager_Person(ResultData data, string account, string password);
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Person Login(string account, string password);

        /// <summary>
        /// 根据平台id
        /// </summary>
        /// <param name="comId">平台id</param>
        /// <returns></returns>
        Person Get_ByComId(int comId);

        /// <summary>
        /// 根据店铺id
        /// </summary>
        /// <param name="shopId">平台id</param>
        /// <returns></returns>
        Person Get_ByShopId(string shopId);

        /// <summary>
        /// 新增平台人员信息
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="uid">uid</param>
        /// <returns></returns>
        Person Add_Person(string name, int uid);
    }
}
