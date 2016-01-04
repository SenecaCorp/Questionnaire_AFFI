using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Questionnaire.Entities.Infrastructure
{
    public enum FacilitySize
    {
        [Display(Name = "Less than $20 million")]
        LessThan20M,
        [Display(Name = "$20-$100 million")]
        From20To100M,
        [Display(Name = "$100m+")]
        MoreThan100M,
        [Display(Name = "Private Company, Not Disclosed")]
        PrivateCompany
    }

    public enum IndustrialClassification
    {
        [Display(Name = "Acidified / LACF")]
        AcidifiedLACF,
        [Display(Name = "Animal Feed/Pet Food")]
        AnimalFeed,
        [Display(Name = "Bakery")]
        Bakery,
        [Display(Name = "Beverages")]
        Beverages,
        [Display(Name = "Breakfast Cereals")]
        BreakfastCereals,
        [Display(Name = "Chocolate/Confections/Candy")]
        Chocolate,
        [Display(Name = "Dairy")]
        Dairy,
        [Display(Name = "Dressing/Sauces/Gravies")]
        Dressing,
        [Display(Name = "Egg")]
        Egg,
        [Display(Name = "Frozen Foods")]
        FrozenFood,
        [Display(Name = "Fruit and Vegetable Products")]
        Fruits,
        [Display(Name = "Game Meats")]
        GameMeats,
        [Display(Name = "Meal Replacement/ Nutritional Food and Beverages")]
        MeatReplacement,
        [Display(Name = "Multiple Food Products")]
        MultipleFood,
        [Display(Name = "Nuts, Nut Products, and Seed Products")]
        Nuts,
        [Display(Name = "Oil/Margarine")]
        Oil,
        [Display(Name = "Pasta")]
        Pasta,
        [Display(Name = "Prepared Foods")]
        PreparedFood,
        [Display(Name = "Produce-Fresh Cut")]
        ProduceFresh,
        [Display(Name = "Produce-Raw Agricultural Commodities (RAC)")]
        ProduceRaw,
        [Display(Name = "Seafood")]
        Seafood,
        [Display(Name = "Snack Foods")]
        Snacks,
        [Display(Name = "Soup")]
        Soup,
        [Display(Name = "Spices/Seasonings")]
        Seasonings,
        [Display(Name = "Stabilizers, Emulsifiers, Flavors, Colors, and Texture Enhancers")]
        Stabilizers,
        [Display(Name = "Sweeteners")]
        Sweeteners,
        [Display(Name = "Whole & Milled Grains and Flours")]
        WholeGrain,
        [Display(Name = "Other")]
        Other
    }

    public enum AnotherProductClassification
    {
        [Display(Name = "Acidified / LACF")]
        AcidifiedLACF,
        [Display(Name = "Animal Feed/Pet Food")]
        AnimalFeed,
        [Display(Name = "Bakery")]
        Bakery,
        [Display(Name = "Beverages")]
        Beverages,
        [Display(Name = "Breakfast Cereals")]
        BreakfastCereals,
        [Display(Name = "Chocolate/Confections/Candy")]
        Chocolate,
        [Display(Name = "Dairy")]
        Dairy,
        [Display(Name = "Dressing/Sauces/Gravies")]
        Dressing,
        [Display(Name = "Egg")]
        Egg,
        [Display(Name = "Frozen Foods")]
        FrozenFood,
        [Display(Name = "Fruit and Vegetable Products")]
        Fruits,
        [Display(Name = "Game Meats")]
        GameMeats,
        [Display(Name = "Meal Replacement/ Nutritional Food and Beverages")]
        MeatReplacement,
        [Display(Name = "Multiple Food Products")]
        MultipleFood,
        [Display(Name = "Nuts, Nut Products, and Seed Products")]
        Nuts,
        [Display(Name = "Oil/Margarine")]
        Oil,
        [Display(Name = "Pasta")]
        Pasta,
        [Display(Name = "Prepared Foods")]
        PreparedFood,
        [Display(Name = "Produce-Fresh Cut")]
        ProduceFresh,
        [Display(Name = "Produce-Raw Agricultural Commodities (RAC)")]
        ProduceRaw,
        [Display(Name = "Seafood")]
        Seafood,
        [Display(Name = "Snack Foods")]
        Snacks,
        [Display(Name = "Soup")]
        Soup,
        [Display(Name = "Spices/Seasonings")]
        Seasonings,
        [Display(Name = "Stabilizers, Emulsifiers, Flavors, Colors, and Texture Enhancers")]
        Stabilizers,
        [Display(Name = "Sweeteners")]
        Sweeteners,
        [Display(Name = "Whole & Milled Grains and Flours")]
        WholeGrain,
        [Display(Name = "Other")]
        Other
    }

    public enum AdditionalProductClassification
    {
        [Display(Name = "Acidified / LACF")]
        AcidifiedLACF,
        [Display(Name = "Animal Feed/Pet Food")]
        AnimalFeed,
        [Display(Name = "Bakery")]
        Bakery,
        [Display(Name = "Beverages")]
        Beverages,
        [Display(Name = "Breakfast Cereals")]
        BreakfastCereals,
        [Display(Name = "Chocolate/Confections/Candy")]
        Chocolate,
        [Display(Name = "Dairy")]
        Dairy,
        [Display(Name = "Dressing/Sauces/Gravies")]
        Dressing,
        [Display(Name = "Egg")]
        Egg,
        [Display(Name = "Frozen Foods")]
        FrozenFood,
        [Display(Name = "Fruit and Vegetable Products")]
        Fruits,
        [Display(Name = "Game Meats")]
        GameMeats,
        [Display(Name = "Meal Replacement/ Nutritional Food and Beverages")]
        MeatReplacement,
        [Display(Name = "Multiple Food Products")]
        MultipleFood,
        [Display(Name = "Nuts, Nut Products, and Seed Products")]
        Nuts,
        [Display(Name = "Oil/Margarine")]
        Oil,
        [Display(Name = "Pasta")]
        Pasta,
        [Display(Name = "Prepared Foods")]
        PreparedFood,
        [Display(Name = "Produce-Fresh Cut")]
        ProduceFresh,
        [Display(Name = "Produce-Raw Agricultural Commodities (RAC)")]
        ProduceRaw,
        [Display(Name = "Seafood")]
        Seafood,
        [Display(Name = "Snack Foods")]
        Snacks,
        [Display(Name = "Soup")]
        Soup,
        [Display(Name = "Spices/Seasonings")]
        Seasonings,
        [Display(Name = "Stabilizers, Emulsifiers, Flavors, Colors, and Texture Enhancers")]
        Stabilizers,
        [Display(Name = "Sweeteners")]
        Sweeteners,
        [Display(Name = "Whole & Milled Grains and Flours")]
        WholeGrain,
        [Display(Name = "Other")]
        Other
    }
}
