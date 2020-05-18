# JobsityChat

To run the project follow the steps:

First of all change the App.Config inside the Jobsity.Service project to your RabbitMq server, if you don't have one you can install with this docker command:

docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management

For the windows service build the project and execute the install service.bat inside the output folder, this will install the service and start him.

After that change de appsettings.json of the projects Authentication and Presentation, then run both by clicking with the right button in the solution, select properties and multiple startup projects.

I tried to use different techniques to show some skills, but I think i overdid the project.

Here is a video with the chat working: https://www.youtube.com/watch?v=4mOQDle3z_I
