using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using MiniLedger.Api.Features.Accounts.Commands.CreateAccount;
using MiniLedger.Api.Services.Interfaces;
using MiniLedger.Domain.Entities;
using MiniLedger.Domain.Enums;
using MiniLedger.Tests.Helpers;

namespace MiniLedger.Tests.Unit.Accounts;

public class CreateAccountCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Account_When_Code_Is_Unique()
    {
        // Arrange
        var context = TestDbContextFactory.Create();

        var cacheServiceMock = new Mock<ICacheService>();

        var handler = new CreateAccountCommandHandler(
            context,
            cacheServiceMock.Object);

        var command = new CreateAccountCommand
        {
            Code = "1001",
            Name = "Cash",
            AccountType = (int)AccountType.Asset,
            RequiresParty = false
        };

        // Act
        var id = await handler.Handle(command, CancellationToken.None);

        // Assert
        id.Should().BeGreaterThan(0);

        var createdAccount = context.Accounts.FirstOrDefault(x => x.Id == id);
        createdAccount.Should().NotBeNull();
        createdAccount!.Code.Should().Be("1001");
        createdAccount.Name.Should().Be("Cash");
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Code_Already_Exists()
    {
        // Arrange
        var context = TestDbContextFactory.Create();

        context.Accounts.Add(new Account
        {
            Code = "1001",
            Name = "Existing Cash",
            AccountType = AccountType.Asset,
            RequiresParty = false
        });

        await context.SaveChangesAsync();

        var cacheServiceMock = new Mock<ICacheService>();

        var handler = new CreateAccountCommandHandler(
            context,
            cacheServiceMock.Object);

        var command = new CreateAccountCommand
        {
            Code = "1001",
            Name = "Duplicate Cash",
            AccountType = (int)AccountType.Asset,
            RequiresParty = false
        };

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Account code '1001' already exists.");
    }
}