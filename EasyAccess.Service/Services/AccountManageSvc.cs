﻿using System.ComponentModel;
using System.Linq;
using EasyAccess.Infrastructure.Service;
using EasyAccess.Infrastructure.Util.ConditionBuilder;
using EasyAccess.Infrastructure.Util.PagingData;
using EasyAccess.Model.DTOs;
using EasyAccess.Model.EDMs;
using EasyAccess.Repository.IRepositories;
using EasyAccess.Service.IServices;

namespace EasyAccess.Service.Services
{
    public class AccountManageSvc : ServiceBase, IAccountManageSvc
    {
        public IAccountRepository AccountRepository { get; set; }

        public PagingData<AccountDto> GetAccountPagingData(PagingCondition pagingCondition, IQueryCondition<Account> queryCondition = null)
        {
            return GetPagingDtoData<AccountDto, Account>(AccountRepository, pagingCondition, queryCondition);
        }
    }
}