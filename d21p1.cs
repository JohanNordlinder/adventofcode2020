using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D21P1
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_21_t_1.txt").ToList();
            Assert.AreEqual(5, new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_21.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            List<Allergen> Allergens = new List<Allergen>();
            List<Ingredient> Ingredients = new List<Ingredient>();

            public int RunChallenge(List<string> input)
            {
                foreach (var row in input)
                {
                    var parts = row.Split(new[] { " (contains ", ")" }, StringSplitOptions.None);
                    var ingredients = parts[0].Split(' ').Select(s => parseIngredient(s)).ToList();
                    var allergens = parts[1].Split(new[] { ", ", }, StringSplitOptions.None).Select(s => parseAllergen(s)).ToList();
                    var food = new Food { Ingredients = ingredients };
                    ingredients.ForEach(i => i.InFood.Add(food));
                    allergens.ForEach(a =>
                    {
                        a.DefinitelyInFoods.Add(food);
                    });
                }

                foreach (var allergen in Allergens)
                {
                    var allPossibleIngredents = allergen.DefinitelyInFoods.SelectMany(f => f.Ingredients);
                    var likely = allPossibleIngredents.Where(i => allergen.DefinitelyInFoods.All(f => f.Ingredients.Contains(i)));
                    allergen.PossiblyContainedIn.AddRange(likely);
                }

                var safeOnes = Ingredients.Where(i => !Allergens.Any(a => a.PossiblyContainedIn.Contains(i)));
                var numberOfTimesTotal = safeOnes.Sum(i => i.InFood.Count);

                return numberOfTimesTotal;
            }

            private Ingredient parseIngredient(string s)
            {
                var existing = Ingredients.FirstOrDefault(i => i.Name == s);
                if (existing == null)
                {
                    var i = new Ingredient { Name = s };
                    Ingredients.Add(i);
                    return i;
                }
                return existing;
            }

            private Allergen parseAllergen(string s)
            {
                var existing = Allergens.FirstOrDefault(i => i.Name == s);
                if (existing == null)
                {
                    var i = new Allergen { Name = s };
                    Allergens.Add(i);
                    return i;
                }
                return existing;
            }
        }

        internal class Food
        {
            public List<Ingredient> Ingredients = new List<Ingredient>();
        }

        internal class Allergen
        {
            public string Name { get; set; }
            public List<Ingredient> PossiblyContainedIn = new List<Ingredient>();
            public List<Food> DefinitelyInFoods = new List<Food>();
        }

        internal class Ingredient
        {
            public string Name { get; set; }
            public List<Food> InFood = new List<Food>();
        }
    }
}
