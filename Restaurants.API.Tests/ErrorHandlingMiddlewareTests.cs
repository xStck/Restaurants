using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Tests;

public class ErrorHandlingMiddlewareTests
{
    private readonly HttpContext _context;
    private readonly Mock<ILogger<ErrorHandlingMiddleware>> _loggerMock;
    private readonly ErrorHandlingMiddleware _middleware;

    public ErrorHandlingMiddlewareTests()
    {
        _context = new DefaultHttpContext();
        _loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        _middleware = new ErrorHandlingMiddleware(_loggerMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_WhenNoExceptionIsThrown_ShouldCallNextDelegate()
    {
        //arrange
        var nextDelegateMock = new Mock<RequestDelegate>();
        //act
        await _middleware.InvokeAsync(_context, nextDelegateMock.Object);

        //assert
        nextDelegateMock.Verify(next => next.Invoke(_context), Times.Once);
    }
    
    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionIsThrown_ShouldSetStatusCode404()
    {
        //arrange
        var notFoundException = new NotFoundException(nameof(Restaurant), "1");

        //act
        await _middleware.InvokeAsync(_context, _ => throw notFoundException);

        //assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task InvokeAsync_WhenForbidExceptionIsThrown_ShouldSetStatusCode403()
    {
        //arrange
        var forbidException = new ForbidException();

        //act
        await _middleware.InvokeAsync(_context, _ => throw forbidException);

        //assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task InvokeAsync_WhenExceptionIsThrown_ShouldSetStatusCode500()
    {
        //arrange
        var exception = new Exception();

        //act
        await _middleware.InvokeAsync(_context, _ => throw exception);

        //assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}