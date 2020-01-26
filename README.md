## 🎓 <a href="https://lucyparry.github.io/degree-class-estimator/">Another OU Degree Class Estimator</a>

A simple web application to help <a href="https://www.open.ac.uk">Open University</a> students work out the class of UK undergraduate honours degree they will be awarded.

Made to test out <a href="https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor">Blazor WebAssembly</a>, an interesting new addition to <a href="https://dotnet.microsoft.com/apps/aspnet">ASP.NET</a> which uses WebAssembly to let you write web apps with C# and .NET on the client side.

#### My notes on how Blazor WebAssembly apps work as static sites and how to make the routing work on GitHub pages for when I forget

In Blazor WebAssembly, the whole compiled application and the .NET assembly .dlls are sent back to the browser, where they can run using a runtime based on <a href="https://webassembly.org/">WebAssembly</a>. This is cool because it means that apps can be served from a static site service like GitHub Pages without any server side code running, as you'd need with a 'normal' .NET web application.

##### Routing
* When the request to the root of the application is made, the response contains all of the assemblies plus the default html page (`index.html`). This page refers to a script which bootstraps the application, and then the Blazor routing takes care of rendering the default component of the app (e.g. `MainLayout.razor`).

* Any links to other components (e.g. `www.example.com/about`) are done by the router in the application on the client side too, they don't actually go back to the server and request the different page.

* We have a problem though if a direct request is made by the browser to a route (e.g. if the user copied and pasted `www.example.com/about` to a new tab). The static site service can't find any file called 'about' so it returns a 404 Not found.

* On something like IIS we could rewrite the route to send the request back to index.html, but we can't do this on GitHub pages.  But we can use the ability to serve a custom 404 page to rewrite the route, which is the approach taken by https://github.com/rafrex/spa-github-pages (which I've used here, thank you to the creator! 💙).

* A script in 404.html takes the URL, converts the routing part to a query string, and then redirects to this modified version, so the default page of the app is requested.

* Then a script in the default page (index.html) checks for a 'route' query string parameter, and converts it back to the proper route for the application routing to handle.

##### Deploying

* To deploy to GitHub, push the contents of the `bin\Release\netstandard2.0\publish` to the correct location on the repo (see the <a href="https://help.github.com/en/github/working-with-github-pages/getting-started-with-github-pages">GitHub docs on enabling GitHub pages for a project</a>)
* Also add a `.nojekyll` file, because we'e not using Jekyll!

* For Project Pages, note again that the repo name is added to to root URL of the application, so this needs to be added to the `base` element of `index.html`, e.g. -

      <base href="/my-lovely-repo-name/" />

* Adding this breaks things when you're running the app on localhost, but you can also pass the base path in to the dotnet run command like this to get around it -

      dotnet run --pathbase=/my-lovely-repo-name