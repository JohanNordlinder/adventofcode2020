using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020
{
    [TestClass]
    public class D21P2
    {

        [TestMethod]
        public void TestRun()
        {
            var input = System.IO.File.ReadAllLines("d_21_t_1.txt").ToList();
            Assert.AreEqual("mxmxvkd,sqjhc,fvjkl", new Program().RunChallenge(input));
        }

        [TestMethod]
        public void RealRun()
        {
            var input = System.IO.File.ReadAllLines("d_21.txt").ToList();
            Console.WriteLine("Result: " + new Program().RunChallenge(input));
        }

        public class Program
        {
            List<Allergen> AllAllergens = new List<Allergen>();
            List<Ingredient> AllIngredients = new List<Ingredient>();

            public string RunChallenge(List<string> input)
            {
                foreach (var row in input)
                {
                    var parts = row.Split(new[] { " (contains ", ")" }, StringSplitOptions.None);
                    var ingredients = parts[0].Split(' ').Select(s => parseIngredient(s)).ToList();
                    var allergens = parts[1].Split(new[] { ", ", }, StringSplitOptions.None).Select(s => parseAllergen(s)).ToList();
                    var food = new Food { Ingredients = ingredients };

                    allergens.ForEach(a =>
                    {
                        a.DefinitelyInFoods.Add(food);
                    });
                }

                foreach (var allergen in AllAllergens)
                {
                    var allPossibleIngredents = allergen.DefinitelyInFoods.SelectMany(f => f.Ingredients).Distinct();
                    var likely = allPossibleIngredents.Where(i => allergen.DefinitelyInFoods.All(f => f.Ingredients.Contains(i)));
                    allergen.PossiblyContainedIn.AddRange(likely);
                }


                while (AllAllergens.Any(a => a.PossiblyContainedIn.Count > 1))
                {
                    var toDeterine = AllAllergens.Where(a => a.PossiblyContainedIn.Count == 1);
                    foreach (var toset in toDeterine)
                    {
                        var ingredient = toset.PossiblyContainedIn.First();
                        ingredient.Allergen = toset;
                        AllAllergens.Where(a => a.Name != toset.Name).ToList().ForEach(a => a.PossiblyContainedIn.Remove(ingredient));
                    }
                }

                var ingredientsWithAllergens = AllIngredients.Where(i => i.Allergen != null).ToList();
                ingredientsWithAllergens.Sort((a, b) => a.Allergen.Name.CompareTo(b.Allergen.Name));
                return string.Join(',', ingredientsWithAllergens);
            }

            private Ingredient parseIngredient(string s)
            {
                var existing = AllIngredients.FirstOrDefault(i => i.Name == s);
                if (existing == null)
                {
                    var i = new Ingredient { Name = s };
                    AllIngredients.Add(i);
                    return i;
                }
                return existing;
            }

            private Allergen parseAllergen(string s)
            {
                var existing = AllAllergens.FirstOrDefault(i => i.Name == s);
                if (existing == null)
                {
                    var i = new Allergen { Name = s };
                    AllAllergens.Add(i);
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
            public Allergen Allergen { get; set; }
            public override string ToString()
            {
                return this.Name;
            }
        }
    }
}
