﻿using System;
using EasyAccess.Authorization;
using EasyAccess.Infrastructure.Service;
using EasyAccess.Infrastructure.Util;
using EasyAccess.Model.EDMs;
using EasyAccess.Model.VOs;
using EasyAccess.Service.IServices;

namespace EasyAccess.Service.Services
{
    public class LoginSvc : ServiceBase, ILoginSvc
    {
        public bool Login(LoginUser loginUser, bool rememberMe = false)
        {
            var result = false;
            using (UnitOfWork)
            {
                var account = Account.VerifyLogin(loginUser);
                if (account != null)
                {
                    var authMgr = AuthorizationManager.GetInstance();
                    var token = authMgr.GetToken(account.Roles, account.GetPermissions());
                    authMgr.SetTicket(loginUser.UserName, token, rememberMe);
                    account.Register.LastLoginIP = IPAddress.GetIPAddress();
                    account.Register.LastLoginTime = DateTime.Now;
                    base.UnitOfWork.Commit();
                    result = true;
                }
            }
            return result;
        }

        public void Logout()
        {
            AuthorizationManager.GetInstance().ClearTicket();
        }
    }
}