﻿using EventBus.Messages.Common;
using ShoppingAggregator.Models;

namespace ShoppingAggregator.Services.Interfaces
{
    public interface IBasketService
    {
        Task<ResponseMessage<BasketModel>> GetBasket(string userName);
    }
}
