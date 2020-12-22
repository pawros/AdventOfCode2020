using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AoCHelper;

namespace AdventOfCode2020
{
    public sealed class Day21 : BaseDay
    {
        private List<string> remainingIngredients;
        private List<string> remainingAllergens;

        private Dictionary<string, string> IngredientsToAllergen = new Dictionary<string, string>();

        private class Food
        {
            public List<string> Ingredients { get; set; } = new List<string>();
            public List<string> Allergens { get; set; } = new List<string>();
        }

        private List<Food> food = new List<Food>();

        public Day21()
        {
            foreach (var line in File.ReadLines(InputFilePath))
            {
                var match = Regex.Match(line, @"([a-z ]*)\(contains\s(.*)\)$");

                var f = new Food
                {
                    Ingredients = match.Groups[1].Value.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList(),
                    Allergens = match.Groups[2].Value.Split(", ").ToList()
                };
                food.Add(f);
            }

            remainingAllergens = food.SelectMany(f => f.Allergens).Distinct().ToList();
            remainingIngredients = food.SelectMany(f => f.Ingredients).Distinct().ToList();
        }

        public override string Solve_1()
        {
            while (remainingAllergens.Any())
            {
                foreach (var allergen in remainingAllergens.ToList())
                {
                    var f0 = food.First(f => f.Allergens.Contains(allergen));

                    var commonIngredients = f0.Ingredients.ToList();
                    
                    var otherFood = food.Where(f => f.Allergens.Contains(allergen) && f != f0).ToList();
                    foreach (var of in otherFood)
                    {
                        commonIngredients = commonIngredients.Intersect(of.Ingredients).ToList();
                    }
                    
                    if (commonIngredients.Count == 1)
                    {
                        var ingredient = commonIngredients[0];
                        IngredientsToAllergen[ingredient] = allergen;
                        remainingAllergens.Remove(allergen);
                        remainingIngredients.Remove(ingredient);
                        
                        foreach (var f in food)
                        {
                            f.Allergens.Remove(allergen);
                            f.Ingredients.Remove(ingredient);
                        }
                    }

                }
            }

            return food.Sum(f => f.Ingredients.Count).ToString();
        }


        public override string Solve_2()
        {
            var orderedIngredients = IngredientsToAllergen.OrderBy(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);
            return string.Join(',', orderedIngredients.Keys);
        }
    }
}