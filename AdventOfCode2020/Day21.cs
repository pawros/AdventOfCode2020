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
        private List<string> remainingIngredients = new List<string>();
        private List<string> remainingAllergens = new List<string>();

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

            foreach (var f in food)
            {
                foreach (var ingredient in f.Ingredients)
                {
                    if (!remainingIngredients.Contains(ingredient))
                    {
                        remainingIngredients.Add(ingredient);
                    }
                }

                foreach (var allergen in f.Allergens)
                {
                    if (!remainingAllergens.Contains(allergen))
                    {
                        remainingAllergens.Add(allergen);
                    }
                }
            }
            
        }
        
        public override string Solve_1()
        {
            while (remainingAllergens.Count > 0)
            {
                foreach (var f0 in food)
                {
                    var starOver = false;

                    if (f0.Ingredients.Intersect(remainingIngredients).Count() == 1 &&
                        f0.Allergens.Intersect(remainingAllergens).Count() == 1)
                    {
                        var ingredient = f0.Ingredients.Intersect(remainingIngredients).ToList()[0];
                        var allergen = f0.Allergens.Intersect(remainingAllergens).ToList()[0];

                        IngredientsToAllergen[ingredient] = allergen;
                        remainingIngredients.Remove(ingredient);
                        remainingAllergens.Remove(allergen);
                        break;
                    }
                    
                    foreach (var ingredient in f0.Ingredients.Intersect(remainingIngredients))
                    {
                        var otherAllergens = food.Where(x => x.Ingredients.Intersect(remainingIngredients).Contains(ingredient) && x != f0)
                            .SelectMany(x => x.Allergens.Intersect(remainingAllergens)).ToList();

                        var its = f0.Allergens.Intersect(remainingAllergens).Intersect(otherAllergens).ToList();

                        if (its.Count == 1)
                        {
                            var allergen = its[0];
                            IngredientsToAllergen[ingredient] = allergen;
                            remainingIngredients.Remove(ingredient);
                            remainingAllergens.Remove(allergen);
                            starOver = true;
                            break;
                        }
                    }

                    if (starOver)
                    {
                        break;
                    }
                }
            }

            var sum = 0;
            foreach (var ingredient in remainingIngredients)
            {
                foreach (var f in food)
                {
                    if (f.Ingredients.Contains(ingredient))
                    {
                        sum++;
                    }
                }
            }

            return sum.ToString();
        }

        public override string Solve_2()
        {
            throw new System.NotImplementedException();
        }
    }
}