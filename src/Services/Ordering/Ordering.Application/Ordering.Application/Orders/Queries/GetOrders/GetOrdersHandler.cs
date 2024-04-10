﻿using BuildingBlocks.Pagination;
using Ordering.Application.DTOs;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersHandler
    (IAppDbContext dbContext)
    : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;
        var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .OrderBy(o => o.OrderName)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
        .ToListAsync(cancellationToken);

        return new GetOrdersResult(
            new PaginatedResult<OrderDTO>(pageIndex, pageSize, totalCount, orders.ToOrderDTOList())
        );
    }
}