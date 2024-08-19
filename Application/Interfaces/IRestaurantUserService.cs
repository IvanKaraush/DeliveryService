﻿using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRestaurantUserService
    {
        public Task<List<string>> GetRestaurantsInCityAdresses(string city);
    }
}
