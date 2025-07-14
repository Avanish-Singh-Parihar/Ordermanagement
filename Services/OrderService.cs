using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Models

{
public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }
        public async Task<List<Order>> GetAllAsync(string statusfilter = null)
        {
            var query = _context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(statusfilter))
            {
                query = query.Where(o => o.DeliveryStatus == statusfilter);
            }

            var list = await query.OrderByDescending(o => o.OrderDate).ToListAsync();

            return list ?? new List<Order>();
        }



        public async Task<Order> GetByIdAsync(int id) => await _context.Orders.FindAsync(id);

        public async Task AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync (Order order)
    {
        _context.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync (int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if(order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
}
