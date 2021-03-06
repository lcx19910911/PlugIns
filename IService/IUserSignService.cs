﻿using Core.AuthAPI;
using Core.Model;
using Domain;
using Domain.ScratchCard;
using Model;
using MPUtil.UserMng;
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
        /// <returns></returns>
        bool User_Sign();

        /// <summary>
        /// 最近十天的签到
        /// </summary>
        /// <param name="openId">微信openId</param>
        /// <returns></returns>
        Dictionary<string, bool> Get_LastelyTenDaySign(string openId, string personId);

        /// <summary>
        /// 最近的一个签到
        /// </summary>
        /// <param name="openId">微信openId</param>
        /// <returns></returns>
        UserSign Get_LastSign(string openId, string personId);
    }
}
