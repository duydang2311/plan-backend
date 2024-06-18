var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.WebApp_Api_V1>("api-v1");

builder.Build().Run();
