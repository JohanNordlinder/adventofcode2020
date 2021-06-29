using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode2020
{

    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override bool Equals(object obj)
        {
            var other = (Coordinate)obj;
            return other.X == X && other.Y == Y && other.Z == Z;
        }

        public override int GetHashCode()
        {
            return X + Y + Z;
        }
    }

    public class CoordinateMovement
    {
        public static Coordinate Move(Coordinate lastPosition, Direction direction)
        {
            switch (direction)
            {
                case Direction.UP:
                    return new Coordinate { X = lastPosition.X, Y = lastPosition.Y + 1 };
                case Direction.RIGHT:
                    return new Coordinate { X = lastPosition.X + 1, Y = lastPosition.Y };
                case Direction.DOWN:
                    return new Coordinate { X = lastPosition.X, Y = lastPosition.Y - 1 };
                case Direction.LEFT:
                    return new Coordinate { X = lastPosition.X - 1, Y = lastPosition.Y };
                case Direction.TOPLEFT:
                    return new Coordinate { X = lastPosition.X - 1, Y = lastPosition.Y + 1 };
                case Direction.TOPRIGHT:
                    return new Coordinate { X = lastPosition.X + 1, Y = lastPosition.Y + 1 };
                case Direction.BOTTOMLEFT:
                    return new Coordinate { X = lastPosition.X - 1, Y = lastPosition.Y - 1 };
                case Direction.BOTTOMRIGHT:
                    return new Coordinate { X = lastPosition.X + 1, Y = lastPosition.Y - 1 };
                default:
                    throw new Exception("Invalid Move");
            }
        }
    }

    public class Velocity : Coordinate { }
    public class Acceleration : Coordinate { }

    public class Particle
    {
        public int Id { get; set; }
        public Coordinate Coordinate { get; set; }
        public Velocity Velocity { get; set; }
        public Acceleration Acceleration { get; set; }
    }

    public class StringConstants
    {
        public static string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    }

    static class StringExtensions
    {
        public static string Reverse(this string input)
        {
            return new string(input.ToCharArray().Reverse().ToArray());
        }

        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }

    public enum Direction
    {
        DOWN, UP, LEFT, RIGHT, TOPLEFT, TOPRIGHT, BOTTOMRIGHT, BOTTOMLEFT
    }

    public static class DirectionTurns
    {
        public static Direction TurnRight(Direction oldDirection)
        {
            switch (oldDirection)
            {
                case Direction.UP:
                    return Direction.RIGHT;
                case Direction.RIGHT:
                    return Direction.DOWN;
                case Direction.DOWN:
                    return Direction.LEFT;
                case Direction.LEFT:
                    return Direction.UP;
                default:
                    throw new Exception("Invalid turn");
            }
        }

        public static Direction TurnLeft(Direction oldDirection)
        {
            switch (oldDirection)
            {
                case Direction.UP:
                    return Direction.LEFT;
                case Direction.RIGHT:
                    return Direction.UP;
                case Direction.DOWN:
                    return Direction.RIGHT;
                case Direction.LEFT:
                    return Direction.DOWN;
                default:
                    throw new Exception("Invalid turn");
            }
        }
    }
    public enum Compass
    {
        NORTH, SOUTH, WEST, EAST, NORTHEAST, NORTHWEST, SOUTHEAST, SOUTHWEST
    }

    public static class Logging
    {
        public static void WriteTrace(string message)
        {
            System.Diagnostics.Trace.WriteLine(message);
        }
    }

    public enum CompareOperation
    {
        GreaterThan, GreaterOrEqualThan, LessThan, LessOrEqualThan, Equal, NotEqual
    }

    public static class CompareOperations
    {
        public static bool EvaluateCompareExpression(int target, int compareValue, CompareOperation operation)
        {
            switch (operation)
            {
                case CompareOperation.Equal:
                    return target == compareValue;
                case CompareOperation.NotEqual:
                    return target != compareValue;
                case CompareOperation.GreaterThan:
                    return target > compareValue;
                case CompareOperation.GreaterOrEqualThan:
                    return target >= compareValue;
                case CompareOperation.LessThan:
                    return target < compareValue;
                case CompareOperation.LessOrEqualThan:
                    return target <= compareValue;
                default:
                    throw new Exception("Unknown Operation");
            }
        }

        public static CompareOperation ParseCompareOperation(string operation)
        {
            switch (operation)
            {
                case ">":
                    return CompareOperation.GreaterThan;
                case ">=":
                    return CompareOperation.GreaterOrEqualThan;
                case "<":
                    return CompareOperation.LessThan;
                case "<=":
                    return CompareOperation.LessOrEqualThan;
                case "!=":
                    return CompareOperation.NotEqual;
                case "==":
                    return CompareOperation.Equal;
                default:
                    throw new Exception("Unknown Operation");
            }
        }
    }

    public enum ModifyOperation
    {
        Increase, Decrease,
    }

    public static class ModifyOperations
    {
        public static ModifyOperation ParseModifyOperation(string modifyOperation)
        {
            switch (modifyOperation)
            {
                case "inc":
                    return ModifyOperation.Increase;
                case "dec":
                    return ModifyOperation.Decrease;
                default:
                    throw new Exception("Unknown Operation");
            }
        }
    }
}
