using OrderManagement.Models;

public interface IOrderService
{
    Task<List<Order>> GetAllAsync(string statusfilter = null);
    Task<Order> GetByIdAsync(int id);
    Task AddAsync(Order order);
    Task UpdateAsync (Order order);
    Task DeleteAsync (int id);
    
}