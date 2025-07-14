using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ordermanagement.Models;
using OrderManagement.Data;
using OrderManagement.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ordermanagement.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid && model.Username == "Admin" && model.Password == "Admin")
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.Username) };
                var identity = new ClaimsIdentity(claims, "BasicAuth");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("BasicAuth", principal);
                return RedirectToAction("Index", "Order");
            }

            ModelState.AddModelError("", "Invalid Credentials");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("BasicAuth");
            return RedirectToAction("Login");
        }
    }
}

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;


    public OrderController(IOrderService service, ILogger<OrderController> logger)
    {
        _orderService = service;
        _logger = logger;
    }

    private readonly ApplicationDbContext _context;

    public async Task<List<Order>> GetAllAsync(string statusfilter = null)
    {
        var orders = _context.Orders.AsQueryable();

        if (!string.IsNullOrEmpty(statusfilter))
        {
            orders = orders.Where(o => o.DeliveryStatus == statusfilter);
        }

        // Return an empty list if no matches
        return await orders.OrderByDescending(o => o.OrderDate).ToListAsync();
    }


    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create (Order order)
    {
        if(order.OrderDate > DateTime.Today)
        {
            ModelState.AddModelError("OrderDate","Order date can not be in future");
        }

        if(ModelState.IsValid)
        {
            await _orderService.AddAsync(order);
            return RedirectToAction("Index");
        }
        return View(order);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> Index(string status)
    {
        var orders = await _orderService.GetAllAsync(status);

        if (orders == null) orders = new List<Order>(); 

        return View(orders);
    }


    public async Task <IActionResult> Delete(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        return View(order);
    }

    [HttpPost,ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        await _orderService.DeleteAsync(id);
        return RedirectToAction("Index");
    }

}