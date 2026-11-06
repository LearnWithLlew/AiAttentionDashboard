using AIAttentionDashboard;

var files = new DashboardFiles(Directory.GetCurrentDirectory());
var parser = new CommandLineParser(files, new RealDashboardEnvironment(files));
return await parser.RunAsync(args, Console.Out, Console.Error);
