using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D11P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_11_t_1.txt").ToList();
            Assert.AreEqual(26, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_11.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            enum SeatStatus { empty, occupied }
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

            private int maxX;
            private int maxY;

            public int RunChallenge(List<string> input)
            {
                var seats = new Dictionary<Coordinate, Seat>();
                for (int y = 0; y < input.Count; y++)
                {
                    for (int x = 0; x < input[0].Length; x++)
                    {
                        if (input[y][x] == 'L')
                        {
                            var coordinate = new Coordinate { X = x, Y = -y };
                            seats.Add(coordinate, new Seat { Coordinate = coordinate, Status = SeatStatus.empty });
                        }
                    }
                }

                maxX = seats.Max(z => z.Key.X);
                maxY = seats.Min(z => z.Key.Y);

                DebugPrintout(seats);

                int seatsChanged = 0;
                do
                {
                    var newSeatArrangement = seats.ToDictionary(entry => entry.Key, entry => entry.Value.Clone());

                    seatsChanged = 0;
                    foreach (var seat in seats)
                    {
                        if (seat.Value.Status == SeatStatus.empty && ShouldLeave(seat.Value, seats))
                        {
                            newSeatArrangement[seat.Key].Status = SeatStatus.occupied;
                            seatsChanged++;
                        }
                        else if (seat.Value.Status == SeatStatus.occupied && ShouldBecomeEmpty(seat.Value, seats))
                        {
                            newSeatArrangement[seat.Key].Status = SeatStatus.empty;
                            seatsChanged++;
                        }
                    }
                    seats = newSeatArrangement;

                    DebugPrintout(seats);
                } while (seatsChanged != 0);
                return seats.Values.Count(s => s.Status == SeatStatus.occupied);
            }

            private void DebugPrintout(Dictionary<Coordinate, Seat> seats)
            {
                for (int i = 0; i < 10; i++)
                {
                    Debug.Write(Environment.NewLine);

                    for (int j = 0; j < 10; j++)
                    {
                        Seat possibleSeat;
                        var value = seats.TryGetValue(new Coordinate { X = j, Y = -i }, out possibleSeat);
                        Debug.Write(value ? (possibleSeat.Status == SeatStatus.occupied ? '#' : 'L') : '.');
                    }
                }
                Debug.Write(Environment.NewLine);
            }

            private bool ShouldBecomeEmpty(Seat seat, Dictionary<Coordinate, Seat> seats)
            {
                return CountNearbyOccupiedSeats(seat, seats) >= 5;
            }

            private bool ShouldLeave(Seat seat, Dictionary<Coordinate, Seat> seats)
            {
                return CountNearbyOccupiedSeats(seat, seats) == 0;
            }

            private int CountNearbyOccupiedSeats(Seat seat, Dictionary<Coordinate, Seat> seats)
            {
                var visibleSeats = new List<bool>() {
                    findInDirection(Direction.UP, seat.Coordinate, seats),
                    findInDirection(Direction.DOWN, seat.Coordinate, seats),
                    findInDirection(Direction.LEFT, seat.Coordinate, seats),
                    findInDirection(Direction.RIGHT, seat.Coordinate, seats),
                    findInDirection(Direction.TOPRIGHT, seat.Coordinate, seats),
                    findInDirection(Direction.TOPLEFT, seat.Coordinate, seats),
                    findInDirection(Direction.BOTTOMLEFT, seat.Coordinate, seats),
                    findInDirection(Direction.BOTTOMRIGHT, seat.Coordinate, seats)};
                return visibleSeats.Count(b => b);
            }

            private bool findInDirection(Direction direction, Coordinate origo, Dictionary<Coordinate, Seat> seats)
            {
                var position = new Coordinate { X = origo.X, Y = origo.Y };
                Seat seat;
                var outOfBounds = false;
                do
                {
                    position = CoordinateMovement.Move(position, direction);
                    outOfBounds = !(position.X >= 0 && position.X <= maxX && position.Y <= 0 && position.Y >= maxY);
                    seats.TryGetValue(position, out seat);
                } while (seat == null && !outOfBounds);

                return seat != null && seat.Status == SeatStatus.occupied;
            }
        }
    }
}
