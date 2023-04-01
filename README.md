# TaskManagementSystem
To start app locally:
1. configure rabbitMq:
run command:
docker pull rabbitmq:management,
docker run –d --hostname my-rabbit --name some-rabbit –p 15672:15672 –p 5672:5672 rabbitmq:management
2. You can see basic rabbitMq configuration in file appsettings.json
3. run app as iis express:
app starts via swagger where you can see all controller methods
