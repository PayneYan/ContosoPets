using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoPets.Domain.DataTransferObjects;
using ContosoPets.Domain.Models;

namespace ContosoPets.Interface
{
    public interface IOrderService
    {
        Task<List<CustomerOrder>> GetAll();
       
        // Add the GetById code

         Task<CustomerOrder> GetById(int id);

        // Add the Create code
        Task<Order> Create(Order newOrder);
        
        // Add the SetFulfilled code
        Task<bool> SetFulfilled(int id);
       
        // Add the Delete code
        Task<bool> Delete(int id);
      
    
    }
}