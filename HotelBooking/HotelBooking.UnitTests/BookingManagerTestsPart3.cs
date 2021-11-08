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
    public class BookingManagerTestsPart3
    {

        private IBookingManager bookingManager;

        public BookingManagerTestsPart3()
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
        [InlineData(1, 2)]
        [InlineData(1, 3)]
        [InlineData(2, 9)]
        public void CreateBooking_StartAndEndBeforeOccupiedDate_AssertTrue(int start, int end)
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
        [InlineData(1, 21)]
        [InlineData(2, 21)]
        [InlineData(9, 21)]
        public void CreateBooking_StartBeforeOccupiedAndEndAfterOccupied_AssertFalse(int start, int end)
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

            Assert.False(bookingManagerWithMock.CreateBooking(b));
        }


        [Theory]
        [InlineData(1, 11)]
        [InlineData(2, 12)]
        [InlineData(9, 13)]
        public void CreateBooking_StartBeforeOccupiedAndEndIsOccupied_AssertFalse(int start, int end)
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

            Assert.False(bookingManagerWithMock.CreateBooking(b));
        }


        [Theory]
        [InlineData(21, 1)]
        [InlineData(21, 2)]
        [InlineData(23, 9)]
        public void CreateBooking_StartAfterOccupiedAndEndBeforeOccupied_AssertFalse(int start, int end)
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

            Action act = () => bookingManager.CreateBooking(b);

            // Assert
            Assert.Throws<ArgumentException>(act);


            //Assert.False(bookingManagerWithMock.CreateBooking(b));
        }


        [Theory]
        [InlineData(11, 21)]
        [InlineData(15, 22)]
        [InlineData(20, 29)]
        public void CreateBooking_StartIsOccupied_AssertFalse(int start, int end)
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


            Assert.False(bookingManagerWithMock.CreateBooking(b));
        }

    }
}
