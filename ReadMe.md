# Green Apron
![CI by Visual Studio Team Services](https://greenapron.visualstudio.com/_apis/public/build/definitions/8e466ee9-b105-4ab9-947d-57aaa6437761/1/badge)

A mobile-first, comprehensive meal planner and pantry tracking system. Search thousands of recipes; bookmark your favorites, or add one to a meal plan and let Green Apron manage a grocery list for you. Purchased products are added to an inventory, so you always have a digital representation of your pantry!

## Technologies
* Xamarin and Xamarin.Forms
* ASP.NET Core Web API
* Entity Framework
* C#
* LINQ
* xUnit
* CI via Visual Studio Team Services

## Installation
* Create a `Keys` class in `GreenApron/Factories` using the `GreenApron` namespace with the following values:
   * `public static string WebAPI`: url for the WebAPI service
   * `public static string SpoonURI`: url for the Spoonacular API service
   * `public static string SpoonKey`: API token for the Spoonacular API service
* To run, set your desired Startup Project in Visual Studio and press play
