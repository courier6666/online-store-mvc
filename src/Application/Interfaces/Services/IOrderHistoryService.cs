﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Application.DataTransferObjects;

namespace Store.Application.Interfaces.Services
{
    /// <summary>
    /// Provides services for managing order history that consists of entries.
    /// Contains operations to retrieve logging history of order.
    /// </summary>
    public interface IOrderHistoryService : ICrudService<EntryDto>
    {
        Task<IEnumerable<EntryDto>> GetOrderHistoryAsync(Guid orderId);
    }
}