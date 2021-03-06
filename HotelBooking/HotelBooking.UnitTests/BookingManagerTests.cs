using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;

        public BookingManagerTests(){
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);
        }

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            // Arrange
            DateTime date = DateTime.Today;

            // Act
            Action act = () => bookingManager.FindAvailableRoom(date, date);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        [Theory]
        [InlineData("25/10/2021", "30/10/2021")]
        [InlineData("22/10/2021", "30/10/2021")]
        [InlineData("12/11/2021", "20/11/2021")]
        public void CreateBooking_FromDifDates_AssertTrue(string start, string end)
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
            
            b.StartDate = DateTime.Parse(start);
            b.EndDate = DateTime.Parse(end);

            Assert.True(bookingManagerWithMock.CreateBooking(b));
        }

        [Theory]
        [InlineData("10/10/2021", "15/10/2021")]
        [InlineData("09/10/2021", "20/10/2021")]
        [InlineData("15/10/2021", "25/10/2021")]
        public void CreateBooking_FromDifDates_AssertFalse(string start, string end)
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

            b.StartDate = DateTime.Parse(start);
            b.EndDate = DateTime.Parse(end);

            Assert.False(bookingManagerWithMock.CreateBooking(b));
        }

        [Theory]
        [InlineData("10/09/2021", "15/09/2021")]
        [InlineData("25/09/2021", "30/10/2021")]
        [InlineData("25/10/2021", "30/09/2021")]
        public void CreateBooking_FromDifDates_TestException(string start, string end)
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

            b.StartDate = DateTime.Parse(start);
            b.EndDate = DateTime.Parse(end);

            Assert.Throws<ArgumentException>(() => bookingManagerWithMock.CreateBooking(b));
        }

        [Theory]
        [InlineData("02/10/2021", "01/10/2021")]
        [InlineData("03/10/2021", "01/10/2021")]
        [InlineData("01/11/2021", "01/10/2021")]
        public void GetFullyOccupiedDates_StartDateNotInTheFuture_ThrowsException(string start, string end)
        {


            Mock<IRepository<Booking>> bookingRepositoryMock = new Mock<IRepository<Booking>>();
            Mock<IRepository<Room>> roomRepositoryMock = new Mock<IRepository<Room>>();


            IBookingManager bookingManagerMock = new BookingManager(bookingRepositoryMock.Object, roomRepositoryMock.Object);


            DateTime dateStart = DateTime.Parse(start);
            DateTime dateEnd = DateTime.Parse(end);

            Assert.Throws<ArgumentException>(() => bookingManagerMock.GetFullyOccupiedDates(dateStart, dateEnd));
        }


        [Theory]
        [InlineData("01/10/2021", "05/10/2021")]
        [InlineData("03/10/2021", "08/10/2021")]
        [InlineData("01/10/2021", "03/10/2021")]
        public void GetFullyOccupiedDates_NumberOfRoomsGreaterThanBookings_EmptyList(string start, string end)
        {
            DateTime dateStart = DateTime.Parse(start);
            DateTime dateEnd = DateTime.Parse(end);


            List<Room> rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
                new Room { Id=3, Description="C" },
            };

            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, IsActive =  true, StartDate = dateStart,EndDate = dateEnd},
                new Booking { Id=2, IsActive =  true, StartDate = dateStart,EndDate = dateEnd},
            };


            Mock<IRepository<Booking>> bookingRepositoryMock = new Mock<IRepository<Booking>>();
            Mock<IRepository<Room>> roomRepositoryMock = new Mock<IRepository<Room>>();


            roomRepositoryMock.Setup(x => x.GetAll()).Returns(rooms);
            bookingRepositoryMock.Setup(x => x.GetAll()).Returns(bookings);

            IBookingManager bookingManagerMock = new BookingManager(bookingRepositoryMock.Object, roomRepositoryMock.Object);

            Assert.Empty(bookingManagerMock.GetFullyOccupiedDates(dateStart, dateEnd));

        }


        [Theory]
        [InlineData("01/10/2021", "05/10/2021")]
        [InlineData("03/10/2021", "08/10/2021")]
        [InlineData("01/10/2021", "03/10/2021")]
        public void GetFullyOccupiedDates_NumberOfRoomsEqualToBookings_FullList(string start, string end)
        {
            DateTime dateStart = DateTime.Parse(start);
            DateTime dateEnd = DateTime.Parse(end);


            List<Room> rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
                new Room { Id=3, Description="C" },
            };

            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, IsActive =  true, StartDate = dateStart,EndDate = dateEnd},
                new Booking { Id=2, IsActive =  true, StartDate = dateStart,EndDate = dateEnd},
                new Booking { Id=3, IsActive =  true, StartDate = dateStart,EndDate = dateEnd},
            };


            Mock<IRepository<Booking>> bookingRepositoryMock = new Mock<IRepository<Booking>>();
            Mock<IRepository<Room>> roomRepositoryMock = new Mock<IRepository<Room>>();


            roomRepositoryMock.Setup(x => x.GetAll()).Returns(rooms);
            bookingRepositoryMock.Setup(x => x.GetAll()).Returns(bookings);

            IBookingManager bookingManagerMock = new BookingManager(bookingRepositoryMock.Object, roomRepositoryMock.Object);

            Assert.Empty(bookingManagerMock.GetFullyOccupiedDates(dateStart, dateEnd));

        }
    }
}
