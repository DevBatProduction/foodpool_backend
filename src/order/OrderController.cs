using FoodPool.order.dtos;
using FoodPool.order.interfaces;
using FoodPool.provider.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodPool.order;

[ApiController]
[Route("api/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IHttpContextProvider _contextProvider;

    public OrderController(IOrderService orderService, IHttpContextProvider contextProvider)
    {
        _orderService = orderService;
        _contextProvider = contextProvider;
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<GetOrderDto>> GetById(int id)
    {
        var order = await _orderService.GetById(id);
        if (order.Value is null) return NotFound();
        if (_contextProvider.GetCurrentUser() != order.Value.User.Id) return Forbid();
        return Ok(order.Value);
    }

    [HttpGet("user/{id:int}")]
    [Authorize]
    public async Task<ActionResult<List<GetOrderDto>>> GetByUserId(int id)
    {
        if (_contextProvider.GetCurrentUser() != id) return Forbid();
        var orders = await _orderService.GetByUserId(id);
        if (orders.Value is null) return NotFound();
        return Ok(orders.Value);
    }

    [HttpGet("post/{id:int}")]
    [Authorize]
    public async Task<ActionResult<List<GetOrderDto>>> GetByPostId(int id)
    {
        var orders = await _orderService.GetByPostId(id, _contextProvider.GetCurrentUser());
        if (!orders.IsFailed) return Ok(orders.Value);
        return orders.Reasons[0].Message switch
        {
            "403" => Forbid(),
            "404" => NotFound(),
            _ => Ok(orders.Value)
        };
    }

    [HttpGet("post/{id:int}/anon")]
    [Authorize]
    public async Task<ActionResult<List<GetAnonOrderDto>>> GetAnonOrderByPostId(int id)
    {
        var orders = await _orderService.GetAnonOrderByPostId(id);
        if (orders.Value is null) return NotFound();
        return Ok(orders.Value);
    }

    [HttpGet("user/{id:int}/delivered")]
    [Authorize]
    public async Task<ActionResult<List<GetOrderDto>>> GetDeliveredOrderByUserId(int id)
    {
        if (_contextProvider.GetCurrentUser() != id) return Forbid();
        var orders = await _orderService.GetDeliveredOrderByUserId(id);
        if (orders.Value is null) return NotFound();
        return Ok(orders.Value);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Create(CreateOrderDto createOrderDto)
    {
        if (_contextProvider.GetCurrentUser() != createOrderDto.UserId) return Forbid();
        var order = await _orderService.Create(createOrderDto, _contextProvider.GetCurrentUser());
        if (!order.IsFailed) return Ok();
        return order.Reasons[0].Message switch
        {
            "404" => NotFound(),
            "403" => Forbid(),
            "400" => BadRequest(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    [HttpPut("post/{id:int}")]
    [Authorize]
    public async Task<ActionResult<UpdateOrderDto>> UpdateByPostUser(UpdateOrderDto updateOrderDto, int id)
    {
        var order = await _orderService.UpdateByPostUser(updateOrderDto, id, _contextProvider.GetCurrentUser());
        if (!order.IsFailed) return Ok(order.Value);
        return order.Reasons[0].Message switch
        {
            "404" => NotFound(),
            "400" => BadRequest(),
            "403" => Forbid(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    [HttpPut("user/{id:int}")]
    [Authorize]
    public async Task<ActionResult<UpdateOrderDto>> UpdateByOrderUser(UpdateOrderDto updateOrderDto, int id)
    {
        var order = await _orderService.UpdateByOrderUser(updateOrderDto, id, _contextProvider.GetCurrentUser());
        if (!order.IsFailed) return Ok(order.Value);
        return order.Reasons[0].Message switch
        {
            "404" => NotFound(),
            "400" => BadRequest(),
            "403" => Forbid(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}