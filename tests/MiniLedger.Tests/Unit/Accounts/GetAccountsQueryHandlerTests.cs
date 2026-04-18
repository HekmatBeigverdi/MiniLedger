using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using MiniLedger.Api.Common.Responses;
using MiniLedger.Api.Features.Accounts.Queries.GetAccounts;
using MiniLedger.Api.Services.Interfaces;
using MiniLedger.Application.DTOs.Accounts;
using MiniLedger.Domain.Entities;
using MiniLedger.Domain.Enums;
using MiniLedger.Tests.Helpers;

namespace MiniLedger.Tests.Unit.Accounts;

public class GetAccountsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_All_Accounts_When_No_Filter_Is_Applied()
    {
        // Arrange
        var context = TestDbContextFactory.Create();

        context.Accounts.AddRange(
            new Account { Code = "1001", Name = "Cash", AccountType = AccountType.Asset, RequiresParty = false },
            new Account { Code = "2001", Name = "Payable", AccountType = AccountType.Liability, RequiresParty = true }
        );

        await context.SaveChangesAsync();

        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value).Returns((string?)null);

        var cacheServiceMock = new Mock<ICacheService>();
        cacheServiceMock
            .Setup(x => x.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<PagedResponse<List<AccountDto>>>>>(),
                It.IsAny<TimeSpan>()))
            .Returns<string, Func<Task<PagedResponse<List<AccountDto>>>>, TimeSpan>(
                async (_, factory, _) => await factory());

        var handler = new GetAccountsQueryHandler(
            context,
            cacheServiceMock.Object,
            configurationMock.Object);

        var query = new GetAccountsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task Handle_Should_Filter_By_Search()
    {
        // Arrange
        var context = TestDbContextFactory.Create();

        context.Accounts.AddRange(
            new Account { Code = "1001", Name = "Cash", AccountType = AccountType.Asset, RequiresParty = false },
            new Account { Code = "1002", Name = "Bank", AccountType = AccountType.Asset, RequiresParty = false }
        );

        await context.SaveChangesAsync();

        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value).Returns((string?)null);

        var cacheServiceMock = new Mock<ICacheService>();
        cacheServiceMock
            .Setup(x => x.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<PagedResponse<List<AccountDto>>>>>(),
                It.IsAny<TimeSpan>()))
            .Returns<string, Func<Task<PagedResponse<List<AccountDto>>>>, TimeSpan>(
                async (_, factory, _) => await factory());

        var handler = new GetAccountsQueryHandler(
            context,
            cacheServiceMock.Object,
            configurationMock.Object);

        var query = new GetAccountsQuery { Search = "Cash" };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.Should().HaveCount(1);
        result.Data.First().Name.Should().Be("Cash");
    }

    [Fact]
    public async Task Handle_Should_Apply_Pagination()
    {
        // Arrange
        var context = TestDbContextFactory.Create();

        for (int i = 1; i <= 15; i++)
        {
            context.Accounts.Add(new Account
            {
                Code = $"10{i:D2}",
                Name = $"Account {i}",
                AccountType = AccountType.Asset,
                RequiresParty = false
            });
        }

        await context.SaveChangesAsync();

        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x.GetSection(It.IsAny<string>()).Value).Returns((string?)null);

        var cacheServiceMock = new Mock<ICacheService>();
        cacheServiceMock
            .Setup(x => x.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<PagedResponse<List<AccountDto>>>>>(),
                It.IsAny<TimeSpan>()))
            .Returns<string, Func<Task<PagedResponse<List<AccountDto>>>>, TimeSpan>(
                async (_, factory, _) => await factory());

        var handler = new GetAccountsQueryHandler(
            context,
            cacheServiceMock.Object,
            configurationMock.Object);

        var query = new GetAccountsQuery
        {
            PageNumber = 2,
            PageSize = 5
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Data.Should().HaveCount(5);
        result.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(5);
        result.TotalCount.Should().Be(15);
    }
}