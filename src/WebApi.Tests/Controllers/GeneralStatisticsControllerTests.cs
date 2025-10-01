using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.Services;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Infrastructure.Persistence;
using IAVH.BioTablero.CM.WebApi.Controllers.Rest.Reports;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace IAVH.BioTablero.CM.WebApi.Tests.Controllers;

/// <summary>
/// Integration tests for GeneralStatisticsController.
/// </summary>
public class GeneralStatisticsControllerTests : IDisposable
{
    private readonly GeneralContext _context;
    private readonly GeneralStatisticsController _controller;
    private readonly GeneralStatisticsService _service;

    public GeneralStatisticsControllerTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<GeneralContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new GeneralContext(options);
        _service = new GeneralStatisticsService(
            new Repository<Initiative>(_context),
            new Repository<InitiativeUser>(_context)
        );
        _controller = new GeneralStatisticsController(null, _service);

        // Seed test data
        SeedTestData();
    }

    private void SeedTestData()
    {
        var initiatives = new List<Initiative>
        {
            new() { Id = 1, Name = "Iniciativa 1", Enabled = true, PolygonArea = 10.5 },
            new() { Id = 2, Name = "Iniciativa 2", Enabled = true, PolygonArea = 5.2 },
            new() { Id = 3, Name = "Iniciativa 3", Enabled = false, PolygonArea = 2.0 },
            new() { Id = 4, Name = "Iniciativa 4", Enabled = true, PolygonArea = 0 }
        };

        var initiativeUsers = new List<InitiativeUser>
        {
            new() { Id = 1, InitiativeId = 1, UserName = "user1", LevelId = 1 },
            new() { Id = 2, InitiativeId = 1, UserName = "user2", LevelId = 2 },
            new() { Id = 3, InitiativeId = 2, UserName = "user3", LevelId = 1 },
            new() { Id = 4, InitiativeId = 3, UserName = "user4", LevelId = 1 }, // Iniciativa inactiva
            new() { Id = 5, InitiativeId = 4, UserName = "user5", LevelId = 2 }
        };

        _context.Initiatives.AddRange(initiatives);
        _context.InitiativeUsers.AddRange(initiativeUsers);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetGeneralStatistics_ShouldReturnCorrectData()
    {
        // Act
        var result = await _controller.GetGeneralStatistics();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value as CustomWebResponse;
        Assert.NotNull(response);
        Assert.False(response.IsError);

        var statistics = response.ResponseBody as GeneralStatisticsDto;
        Assert.NotNull(statistics);
        Assert.Equal(3, statistics.TotalActiveInitiatives);
        Assert.Equal(4, statistics.TotalPeopleInvolved);
        Assert.Equal(1570.0, statistics.TotalAreaInHectares);
    }

    [Fact]
    public async Task GetGeneralStatistics_WithEmptyDatabase_ShouldReturnZeroValues()
    {
        // Arrange
        _context.Initiatives.RemoveRange(_context.Initiatives);
        _context.InitiativeUsers.RemoveRange(_context.InitiativeUsers);
        _context.SaveChanges();

        // Act
        var result = await _controller.GetGeneralStatistics();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value as CustomWebResponse;
        var statistics = response.ResponseBody as GeneralStatisticsDto;
        
        Assert.Equal(0, statistics.TotalActiveInitiatives);
        Assert.Equal(0, statistics.TotalPeopleInvolved);
        Assert.Equal(0.0, statistics.TotalAreaInHectares);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
