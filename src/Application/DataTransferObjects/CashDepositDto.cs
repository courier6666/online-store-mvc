using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.DataTransferObjects
{
    public class CashDepositDto
    {
        public Guid Id { get; set; }
        public decimal CurrentMoneyBalance { get; set; }
        public Guid UserId { get; set; }
    }
}
