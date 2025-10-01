namespace IAVH.BioTablero.CM.Application.Tests.Services;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IAVH.BioTablero.CM.Application.DTOs.Reports;
using IAVH.BioTablero.CM.Application.Services;
using IAVH.BioTablero.CM.Application.Utils;
using IAVH.BioTablero.CM.Core.Domain.Entities.Initiatives;
using IAVH.BioTablero.CM.Core.Interfaces.Repositories;

using Microsoft.EntityFrameworkCore;

using Moq;

using Xunit;

/// <summary>
/// Unit tests for GeneralStatisticsService.
/// </summary>
public class GeneralStatisticsServiceTests
{
    private readonly Mock<IRepository<Initiative>> initiativeRepositoryMock;
    private readonly Mock<IRepository<InitiativeUser>> initiativeUserRepositoryMock;
    private readonly GeneralStatisticsService service;

    public GeneralStatisticsServiceTests()
    {
        initiativeRepositoryMock = new Mock<IRepository<Initiative>>();
        initiativeUserRepositoryMock = new Mock<IRepository<InitiativeUser>>();
        service = new GeneralStatisticsService(initiativeRepositoryMock.Object, initiativeUserRepositoryMock.Object);
    }

    [Fact]
    public async Task GetGeneralStatisticsAsync_ShouldReturnCorrectStatistics()
    {
        // Arrange
        var initiatives = new List<Initiative>
        {
            new() { Id = 1, Enabled = true, PolygonArea = 10.5 }, // 1050 hectáreas
            new() { Id = 2, Enabled = true, PolygonArea = 5.2 },  // 520 hectáreas
            new() { Id = 3, Enabled = false, PolygonArea = 2.0 }, // No cuenta (inactiva)
            new() { Id = 4, Enabled = true, PolygonArea = 0 }     // No cuenta (sin área)
        };

        var initiativeUsers = new List<InitiativeUser>
        {
            new() { InitiativeId = 1, UserName = "user1" },
            new() { InitiativeId = 1, UserName = "user2" },
            new() { InitiativeId = 2, UserName = "user3" },
            new() { InitiativeId = 3, UserName = "user4" }, // No cuenta (iniciativa inactiva)
            new() { InitiativeId = 4, UserName = "user5" }
        };

        var initiativesQuery = initiatives.AsQueryable();
        var activeInitiativesQuery = initiativesQuery.Where(i => i.Enabled);
        var peopleInActiveInitiativesQuery = initiativeUsers.AsQueryable()
            .Where(iu => activeInitiativesQuery.Any(ai => ai.Id == iu.InitiativeId));

        initiativeRepositoryMock.Setup(x => x.GetQueryable())
            .Returns(initiativesQuery);

        initiativeRepositoryMock.Setup(x => x.QueryToListAsync(It.IsAny<IQueryable<Initiative>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IQueryable<Initiative> query, CancellationToken ct) => query.ToList());

        initiativeUserRepositoryMock.Setup(x => x.GetQueryable())
            .Returns(initiativeUsers.AsQueryable());

        initiativeUserRepositoryMock.Setup(x => x.QueryToListAsync(It.IsAny<IQueryable<InitiativeUser>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IQueryable<InitiativeUser> query, CancellationToken ct) => query.ToList());

        // Act
        var result = await service.GetGeneralStatisticsAsync();

        // Assert
            Assert.True(result.Success);
        Assert.NotNull(result.ResponseBody);

        var statistics = result.ResponseBody as GeneralStatisticsDto;
        Assert.NotNull(statistics);
        Assert.Equal(3, statistics.TotalActiveInitiatives); // Solo iniciativas activas
        Assert.Equal(4, statistics.TotalPeopleInvolved); // Usuarios en iniciativas activas
        Assert.Equal(1570.0, statistics.TotalAreaInHectares); // (10.5 + 5.2) * 100
    }

    [Fact]
    public async Task GetGeneralStatisticsAsync_WithNoActiveInitiatives_ShouldReturnZeroValues()
    {
        // Arrange
        var initiatives = new List<Initiative>
        {
            new() { Id = 1, Enabled = false, PolygonArea = 10.5 },
            new() { Id = 2, Enabled = false, PolygonArea = 5.2 }
        };

        var initiativesQuery = initiatives.AsQueryable();

        initiativeRepositoryMock.Setup(x => x.GetQueryable())
            .Returns(initiativesQuery);

        initiativeRepositoryMock.Setup(x => x.QueryToListAsync(It.IsAny<IQueryable<Initiative>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IQueryable<Initiative> query, CancellationToken ct) => query.ToList());

        initiativeUserRepositoryMock.Setup(x => x.GetQueryable())
            .Returns(new List<InitiativeUser>().AsQueryable());

        initiativeUserRepositoryMock.Setup(x => x.QueryToListAsync(It.IsAny<IQueryable<InitiativeUser>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IQueryable<InitiativeUser> query, CancellationToken ct) => query.ToList());

        // Act
        var result = await service.GetGeneralStatisticsAsync();

        // Assert
            Assert.True(result.Success);
        var statistics = result.ResponseBody as GeneralStatisticsDto;
        Assert.NotNull(statistics);
        Assert.Equal(0, statistics.TotalActiveInitiatives);
        Assert.Equal(0, statistics.TotalPeopleInvolved);
        Assert.Equal(0.0, statistics.TotalAreaInHectares);
    }

    [Fact]
    public async Task GetGeneralStatisticsAsync_WhenExceptionOccurs_ShouldReturnError()
    {
        // Arrange
        initiativeRepositoryMock.Setup(x => x.GetQueryable())
            .Throws(new Exception("Database connection failed"));

        // Act
        var result = await service.GetGeneralStatisticsAsync();

            // Assert
            Assert.False(result.Success);
        Assert.Contains("Error al obtener las estadísticas generales", result.Message);
    }
}
