var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.BackendProject>("backendproject");

builder.Build().Run();
