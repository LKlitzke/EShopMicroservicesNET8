﻿
using Ordering.Application.Orders.Queries.GetOrderByName;

namespace Ordering.API.Endpoints;

public record GetOrdersByNameResponse(IEnumerable<OrderDTO> Orders);

public class GetOrdersByName : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{orderName}", async (string orderName, ISender sender) =>
        {
            var result = await sender.Send(new GetOrderByNameQuery(orderName));
            var response = result.Adapt<GetOrdersByNameResponse>();

            return Results.Ok(response);
        })
        .WithName("GetOrdersByName")
        .Produces<GetOrdersByNameResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Orders By Name")
        .WithDescription("Get Orders By Name");
    }
}
