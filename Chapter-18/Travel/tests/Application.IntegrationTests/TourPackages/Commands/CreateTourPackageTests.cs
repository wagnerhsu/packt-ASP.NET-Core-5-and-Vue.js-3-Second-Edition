using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Travel.Application.TourLists.Commands.CreateTourList;
using Travel.Application.TourPackages.Commands.CreateTourPackage;
using Travel.Domain.Entities;
using Xunit;

namespace Application.IntegrationTests.TourPackages.Commands
{
    using static Testing;
    
    [Collection("Database collection")]
    public class CreateTourPackageTests
    {
        public CreateTourPackageTests()
        {
            ResetState().GetAwaiter().GetResult();
        }

        [Fact]
        public void Should_Require_Minimum_Fields()
        {
            var command = new CreateTourPackageCommand();

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Fact]
        public async Task Should_Create_TourPackage()
        {
            var listId = await SendAsync(new CreateTourListCommand
            {
                City = "New York",
                Country = "USA",
                About = "Lorem Ipsum"
            });

            var command = new CreateTourPackageCommand
            {
                ListId = listId,
                Name = "Free Walking Tour New York",
                Duration = 2,
                Price = 10,
                InstantConfirmation = true,
                MapLocation = "Lorem Ipsum",
                WhatToExpect = "Lorem Ipsum"
            };

            var packageId = await SendAsync(command);
            var package = await FindAsync<TourPackage>(packageId);

            package.Should().NotBeNull();
            package.ListId.Should().Be(command.ListId);
            package.Name.Should().Be(command.Name);
            package.Price.Should().Be(command.Price);
            package.Duration.Should().Be(command.Duration);
            package.InstantConfirmation.Should().Be(command.InstantConfirmation);
            package.MapLocation.Should().Be(command.MapLocation);
            package.WhatToExpect.Should().Be(command.WhatToExpect);
        }
    }
}