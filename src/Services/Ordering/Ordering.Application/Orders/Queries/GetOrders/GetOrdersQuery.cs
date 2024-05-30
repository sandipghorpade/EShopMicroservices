using BuildingBlocks.Pagination;
using Ordering.Application.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.Application.Orders.Queries.GetOrders
{
    public record GetOrdersQuery(PaginationRequest paginationRequest):IQuery<GetOrdersResult>;

    public record GetOrdersResult(PaginatedResult<OrderDto> Orders);
}
