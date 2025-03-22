using Prog_Lab_1.Factory;
using Prog_Lab_1.Presentation;
using Prog_Lab_1.VM;
using VM.Domain;

var lifecycle = new ApplicationLifecycle();
var dateTimeService = new SystemDateTimeService();

var intFactory = new IntVirtualMemoryFactory(dateTimeService);
var charFactory = new CharVirtualMemoryFactory(dateTimeService);
var varcharFactory = new VarcharVirtualMemoryFactory();

var vmService = new VirtualMemoryService(intFactory, charFactory, varcharFactory);
var commandsPerformer = new CommandsPerformer(vmService, lifecycle);
var app = new App(lifecycle, commandsPerformer);

app.Run();