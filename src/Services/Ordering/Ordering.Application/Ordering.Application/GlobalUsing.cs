global using BuildingBlocks.CQRS;

global using Microsoft.Extensions.Logging;
global using Microsoft.EntityFrameworkCore;

global using Ordering.Application.Extensions;
global using Ordering.Application.Data;
global using Ordering.Application.DTOs;
global using Ordering.Application.Exceptions;

global using Ordering.Domain.Models;
global using Ordering.Domain.ValueObjects;
global using Ordering.Domain.Events;

global using MediatR;