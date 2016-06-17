using Core.AuthAPI;
using Core.Model;
using Domain;
using Domain.ScratchCard;
using MPUtil.UserMng;
using Repository;
using System;
using System.Collections.Generic;

namespace IService
{
    /// <summary>
    /// 微信用户接口
    /// </summary>
    public interface IUserSignService
    {
        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="openId">微信openId</param>
        /// <returns></returns>
        bool User_Sign(string openId);

        /// <summary>
        /// 最近十天的签到
        /// </summary>
        /// <param name="openId">微信openId</param>
        /// <returns></returns>
        Dictionary<string, bool> Get_LastelyTenDaySign(string openId);

        /// <summary>
        /// 最近的一个签到
        /// </summary>
        /// <param name="openId">微信openId</param>
        /// <returns></returns>
        UserSign Get_LastSign(string openId);
    }
}
