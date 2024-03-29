using FluentResults;
using FoodPool.order.dtos;

namespace FoodPool.order.interfaces;

public interface IOrderService
{
    Task<Result> Create(CreateOrderDto createOrderDto, int userId);
    Task<Result<GetOrderDto>> GetById(int id);
    Task<Result<List<GetOrderDto>>> GetByUserId(int userId);
    Task<Result<List<GetOrderDto>>> GetByPostId(int postId, int userId);
    Task<Result<List<GetAnonOrderDto>>> GetAnonOrderByPostId(int postId);
    Task<Result<List<GetOrderDto>>> GetDeliveredOrderByUserId(int postId);
    Task<Result<GetOrderDto>> UpdateByPostUser(UpdateOrderDto updateOrderDto, int id, int userId);
    Task<Result<GetOrderDto>> UpdateByOrderUser(UpdateOrderDto updateOrderDto, int id, int userId);

}