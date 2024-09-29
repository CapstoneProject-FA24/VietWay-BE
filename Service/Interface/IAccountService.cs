﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.Interface
{
    public interface IAccountService
    {
        public Task<Account?> LoginByPhone(string phone, string password);
        public Task<Account?> LoginByEmail(string email, string password);
    }
}
