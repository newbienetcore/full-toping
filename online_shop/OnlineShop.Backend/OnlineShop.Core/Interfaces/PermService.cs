﻿using OnlineShop.Core;
using OnlineShop.Core.Schemas.Base;
namespace OnlineShop.Core.Interfaces
{

    public interface IPerm
    {
        List<PermSchema> GetPerms(int userId);
    }
     
}