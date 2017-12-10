# Siebe's Web Application

*This is the second mandatory assignment for the course C# and the .NET platform (I-CS-U1).*


## To get the project up and running

* Clone the folder as ZIP to your computer. Go to Visual Studio, click 'Open...' and import the solution 'SiebePaesschesoone.sln' under 'SiebePaesschesoone', normally that should also load the class library 'ClassLibraryForSiebePaesschesoone' ('SiebePaesschesoone' has a reference to 'ClassLibraryForSiebePaesschesoone'), since they are in the same folder if you clone.

* Change all three filepaths in the HomeController, so that it refers to the text.txt document that we are using. These filpaths are situated on line 43, 110 and 143 (in HomeController.cs under SiebePaesschesoone).




## Base structure
1. **Class Library:** *ClassLibraryForSiebePaesschesoone*

This library class contains the data or bussiness logic for my full solution. We have two *models* in it. For the ClientModel I chose to not use DataAnnotations, as that would make it a ViewModel (a model that interacts with my view). Instead, I used validation rules in my controller. In this way, the ClientModel only containts Data and can easily be re-used in other projects. For the ValidationModel, I did the same, just for reusability of my code.

2. **ASP .NET MVC Web App:** *SiebePaesschesoone*

In a MVC web app, we have basically three elements: Models, Views and Controllers. Since I have defined my models in the class library (described above), I only have Controllers and Views stored in this solution folder. Of course I have referenced my Class Library project, so that my MVC app knows where to find the models. In the Views we have 3 view pages defined: Index, Submissions and ValidationPage. Each view contains the necessary code to represent data to the user of the app. In a view, we typically define components that displays the app's user interface. My Index page is my homepage, where you can submit your form. We also have a link on this page to redirect to the submissions. The submissions page requires authenticated access, so if you click the 'view all submissions' button, you will be redirected to a validation page first. Obviously, in my case this is called the 'ValidationPage' view. If you type in the correct password (the password is "admin"), you will be redirected to the submissions page. The submissions page shows all the successfull submissions, per 10 on a page.

Secondly, we have also Controller classes. In my case, I only have 1: Homecontroller.cs. Every time a user does something, there is a browser request. These requests are handled by this controller. They retrieve the model data (from the class library) and call view templates that return a response.

3. **xUNIT Test Project:** *SiebePaesschesooneTest*

In this project, I set up 3 small tests, to see whether the controller methods are returning the view we actually want. If you simply run all the unit tests, you should see three 'passed' tests.


*Author: Siebe Paesschesoone*


