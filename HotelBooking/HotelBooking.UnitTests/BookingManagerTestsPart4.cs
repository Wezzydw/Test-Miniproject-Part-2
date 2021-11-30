using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTestsPart4
    {
        private IBookingManager bookingManager;

        public BookingManagerTestsPart4()
        {
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);
        }


        [Theory]
        [InlineData(21, 22)]
        [InlineData(21, 23)]
        [InlineData(22, 29)]
        public void CreateBooking_StartAndEndAfterOccupiedDate_AssertTrue(int start, int end)
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

            b.StartDate = DateTime.Today.AddDays(start);
            b.EndDate = DateTime.Today.AddDays(end);

            Assert.True(bookingManagerWithMock.CreateBooking(b));
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(11, 2)]
        [InlineData(32, 31)]
        public void GetFullyOccupiedDates_StartDate_GreaterThanEndDate(int start, int end)
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

            DateTime startDate = DateTime.Today.AddDays(start);
            DateTime endDate = DateTime.Today.AddDays(end);

            Action act = () => bookingManager.GetFullyOccupiedDates(startDate, endDate);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(1, 9)]
        [InlineData(2, 3)]
        public void GetFullyOccupiedDates_NoBookings(int start, int end)
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

            DateTime startDate = DateTime.Today.AddDays(start);
            DateTime endDate = DateTime.Today.AddDays(end);

            // Assert
            Assert.Empty(bookingManagerWithMock.GetFullyOccupiedDates(startDate, endDate));
        }


        [Theory]
        [InlineData(1, 12)]
        [InlineData(1, 19)]
        [InlineData(2, 23)]
        public void GetFullyOccupiedDates_BookingsLessThanRooms(int start, int end)
        {
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=DateTime.Today.AddDays(10), EndDate=DateTime.Today.AddDays(20), IsActive=true, CustomerId=1, RoomId=1 },
                //new Booking { Id=2, StartDate=DateTime.Today.AddDays(10), EndDate=DateTime.Today.AddDays(20), IsActive=true, CustomerId=2, RoomId=2 },
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

            DateTime startDate = DateTime.Today.AddDays(start);
            DateTime endDate = DateTime.Today.AddDays(end);

            // Assert
            Assert.Empty(bookingManagerWithMock.GetFullyOccupiedDates(startDate, endDate));
        }

        [Theory]
        [InlineData(1, 12)]
        [InlineData(1, 19)]
        [InlineData(2, 23)]
        public void GetFullyOccupiedDates_BookingsEqualToRooms(int start, int end)
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

            DateTime startDate = DateTime.Today.AddDays(start);
            DateTime endDate = DateTime.Today.AddDays(end);

            // Assert
            Assert.NotEmpty(bookingManagerWithMock.GetFullyOccupiedDates(startDate, endDate));
        }
    }
}
