# Green Apron
![CI by Visual Studio Team Services](https://greenapron.visualstudio.com/_apis/public/build/definitions/8e466ee9-b105-4ab9-947d-57aaa6437761/1/badge)

A mobile-first, comprehensive meal planner, grocery list and pantry inventory system. Search thousands of recipes (provided by [Spoonacular](https://spoonacular.com/food-api)). Bookmark your favorites, or add recipes to a meal plan and let Green Apron manage a grocery list for you. Purchased products are automatically added to your inventory, so you always have a digital representation of what's in the pantry!

## Distribution
### Android
A signed APK is available for Android versions 4.03 and above at [this link](https://drive.google.com/file/d/0BxTbSIwZ9yH5YlVqZ0NWSHI2dFU/view?usp=sharing).

Please note, to allow installation from non-marketplace sources, a user must enable the "Unknown sources" setting on a device before attempting to install an application. The setting for this may be found under Settings > Security.

### iOS
Coming soon...

In the meantime, if you have Spoonacular API access, you can deploy Green Apron to your iOS device from Visual Studio or xCode.

## Screenshots
Generated with [MockuPhone](http://mockuphone.com/)

![Search thousands of recipes](/Assets/screenshots/search-recipes.png)
![View plans by date and meal](/Assets/screenshots/view-plans.png)
![Green Apron manages your grocery list](/Assets/screenshots/grocery-list.png)
![Green Apron manages your pantry too](/Assets/screenshots/view-pantry.png)
![Quick controls available for updating either list](/Assets/screenshots/quick-edit.png)

## Technologies
* C#
* Xamarin and Xamarin.Forms
* ASP.NET Core Web API
* Entity Framework, LINQ
* xUnit
* CI via Visual Studio Team Services
* Hosted on Azure

## Development
* Open the solution file in Visual Studio and allow the Nuget packages to restore
* Host the WebAPI project as desired, a base url is required for the next step
* Create a `Keys` class in `GreenApron/Factories` using the `GreenApron` namespace with the following values:
   * `public static string WebAPI`: url for the WebAPI service
   * `public static string SpoonURI`: url for Spoonacular API service
   * `public static string SpoonKey`: API token for Spoonacular API service
* To run, set your desired Startup Project in Visual Studio and press play

Green Apron's style guide is [available here](https://app.frontify.com/d/QxVhCeuHB3jU/green-apron-style-guide) via Frontify.
