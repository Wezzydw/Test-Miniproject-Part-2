using System;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;
using Moq;
using System.Collections.Generic;

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
