using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using MiniLedger.Api.DTOs.JournalEntries;
using MiniLedger.Api.Services.Implementations;
using MiniLedger.Domain.Entities;
using MiniLedger.Domain.Enums;
using MiniLedger.Tests.Helpers;

namespace MiniLedger.Tests.Unit.JournalEntries;

public class JournalEntryServiceTests
{
    [Fact]
    public async Task CreateAsync_Should_Throw_When_No_Lines_Provided()
    {
        // Arrange
        var context = TestDbContextFactory.Create();
        var service = new JournalEntryService(context, new NullLogger<JournalEntryService>());

        var dto = new JournalEntryCreateDto
        {
            EntryNumber = "JV-001",
            Date = DateTime.UtcNow,
            Description = "Empty entry",
            Lines = new List<JournalEntryLineCreateDto>()
        };

        // Act
        Func<Task> act = async () => await service.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Journal entry must have at least one line.");
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Debit_And_Credit_Are_Not_Equal()
    {
        // Arrange
        var context = TestDbContextFactory.Create();

        context.Accounts.Add(new Account
        {
            Id = 1,
            Code = "1001",
            Name = "Cash",
            AccountType = AccountType.Asset,
            RequiresParty = false
        });

        await context.SaveChangesAsync();

        var service = new JournalEntryService(context, new NullLogger<JournalEntryService>());

        var dto = new JournalEntryCreateDto
        {
            EntryNumber = "JV-002",
            Date = DateTime.UtcNow,
            Description = "Unbalanced entry",
            Lines = new List<JournalEntryLineCreateDto>
            {
                new()
                {
                    AccountId = 1,
                    Debit = 100,
                    Credit = 0
                },
                new()
                {
                    AccountId = 1,
                    Debit = 0,
                    Credit = 50
                }
            }
        };

        // Act
        Func<Task> act = async () => await service.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Total debit and total credit must be equal.");
    }
}