using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D11P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_11_t_1.txt").ToList();
            Assert.AreEqual(37, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_11.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            enum SeatStatus { Empty, Occupied}
            class Seat
            {
                public Coordinate Coordinate { get; set; }
                public SeatStatus Status { get; set; }
                public Seat Clone()
                {
                    return new Seat
                    {
                        Coordinate = Coordinate,
                        Status = Status,
                    };
                }
            }
            public int RunChallenge(List<string> input)
            {
                var seats = new Dictionary<Coordinate, Seat>();
                for (int y = 0; y < input.Count; y++)
                {
                    for (int x = 0; x < input[0].Length; x++)
                    {
                        if (input[y][x] == 'L')
                        {
                            var coordinate = new Coordinate { X = x, Y = y };
                            seats.Add(coordinate, new Seat { Coordinate = coordinate, Status = SeatStatus.Empty });
                        }
                    }
                }

                int seatsChanged = 0;
                do
                {
                    var seatsAfterRound = seats.ToDictionary(entry => entry.Key, entry => entry.Value.Clone());

                    seatsChanged = 0;

                    foreach (var seat in seats)
                    {
                        if (seat.Value.Status == SeatStatus.Empty && ShouldOccupie(seat.Value, seats))
                        {
                            seatsAfterRound[seat.Key].Status = SeatStatus.Occupied;
                            seatsChanged++;
                        } else if (seat.Value.Status == SeatStatus.Occupied && ShouldLeave(seat.Value, seats))
                        {
                            seatsAfterRound[seat.Key].Status = SeatStatus.Empty;
                            seatsChanged++;
                        }
                    }
                    seats = seatsAfterRound;

                } while (seatsChanged != 0);
                return seats.Values.Count(s => s.Status == SeatStatus.Occupied);
            }

            private bool ShouldLeave(Seat seat, Dictionary<Coordinate, Seat> seats)
            {
                var adjecentSeats = getNearbySeats(seat, seats);
                return adjecentSeats.Count(s => s.Status == SeatStatus.Occupied) >= 4;
            }

            private bool ShouldOccupie(Seat seat, Dictionary<Coordinate, Seat> seats)
            {
                var adjecentSeats = getNearbySeats(seat, seats);
                return adjecentSeats.All(s => s.Status == SeatStatus.Empty);
            }

            private IEnumerable<Seat> getNearbySeats(Seat seat, Dictionary<Coordinate, Seat> seats)
            {
                return seats.Where(s =>
                    s.Key.X >= seat.Coordinate.X - 1 &&
                    s.Key.X <= seat.Coordinate.X + 1 &&
                    s.Key.Y >= seat.Coordinate.Y - 1 &&
                    s.Key.Y <= seat.Coordinate.Y + 1 &&
                    !(s.Key.X == seat.Coordinate.X && s.Key.Y == seat.Coordinate.Y)
                ).Select(kvp => kvp.Value);
            }
        }
    }
}
