using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClose.Application.Interfaces
{
    public interface IOrderCloseJob
    {
        public Task ClosePendingOrders();
    }
}
