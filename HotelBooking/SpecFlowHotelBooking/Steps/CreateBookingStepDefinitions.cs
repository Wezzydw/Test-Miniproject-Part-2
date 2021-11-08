using System;
using System.Collections.Generic;
using FluentAssertions;
using HotelBooking.Core;
using Moq;
using TechTalk.SpecFlow;

namespace SpecFlowHotelBooking.Steps
{
    [Binding]
    public sealed class CreateBookingStepDefinitions
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _isActive;

        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;

        public CreateBookingStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("the start date is (.*)")]
        public void GivenTheStartDateIs(int startDate)
        {
            //Save start date
            _startDate = DateTime.Today.AddDays(startDate);


        }

        [Given("the end date is (.*)")]
        public void GivenTheEndDateIs(int endDate)
        {
            //Save end date
            _endDate = DateTime.Today.AddDays(endDate);

        }

        [When("check if room is available")]
        public void CheckAvailability()
        {
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=DateTime.Today.AddDays(10), EndDate=DateTime.Today.AddDays(20), IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=DateTime.Today.AddDays(10), EndDate=DateTime.Today.AddDays(20), IsActive=true, CustomerId=2, RoomId=2 },
            };
            List<Room> rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };
            Mock<IRepository<Booking>> bookingMock = new Mock<IRepository<Booking>>();
            Mock<IRepository<Room>> roomMock = new Mock<IRepository<Room>>();
            bookingMock.Setup(x => x.GetAll()).Returns(bookings);
            roomMock.Setup(x => x.GetAll()).Returns(rooms);

            IBookingManager bookingManagerWithMock = new BookingManager(bookingMock.Object, roomMock.Object);


            Booking b = new Booking();
            b.StartDate = _startDate;
            b.EndDate = _endDate;
            _isActive = bookingManagerWithMock.CreateBooking(b);
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(bool isActive)
        {
            _isActive.Should().Be(isActive);


        }
    }
}
