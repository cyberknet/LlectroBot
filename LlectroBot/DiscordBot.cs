using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Discord;
using Discord.Cards;
using Discord.Commands;
using Discord.WebSocket;
using LlectroBot.Core.Configuration;
using LlectroBot.Core.Modules;
using LlectroBot.Core.Services;
using LlectroBot.Logging;
using LlectroBot.UserTracking;
using Microsoft.Extensions.DependencyInjection;

namespace LlectroBot
{
    public partial class DiscordBot : IDiscordBot
    {
        private readonly DiscordSocketClient Client = null;
        public CommandService Commands { get; private set; }
        private readonly IServiceProvider ServiceProvider = null;
        private readonly IUserTracker UserTracker = null;
        private readonly IBotConfiguration BotConfiguration = null;
        private readonly ICardStackManager CardStackManager = null;
        private List<Type> loadedTypes = new List<Type>();

        private ILogger Logger { get; set; }

        public DiscordBot(IServiceCollection services)
        {
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            BotConfiguration = ServiceProvider.GetService<IBotConfiguration>();
            UserTracker = ServiceProvider.GetService<IUserTracker>();
            Client = ServiceProvider.GetService<IDiscordClient>() as DiscordSocketClient;
            Logger = ServiceProvider.GetService<ILogger>();
            Commands = ServiceProvider.GetService<CommandService>();
            CardStackManager = ServiceProvider.GetService<ICardStackManager>();
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<ILogger>(sp => new Logger(sp.GetService<IBotConfiguration>(), sp.GetService<IDiscordClient>()))
                .AddSingleton<IDiscordBot>(this)
                .AddSingleton<DiscordSocketClient>(new DiscordSocketClient())
                .AddSingleton<IDiscordClient>(sp => sp.GetService<DiscordSocketClient>())
                //.AddSingleton<ICardStackManager>(sp => new LlectroBotCardStackManager(sp.GetService<IDiscordClient>(), sp.GetService<IBotConfiguration>()))
                //.AddSingleton<IUserTracker>(sp => new UserTracker(sp.GetService<DiscordSocketClient>()))
                .AddSingleton<CommandService>(new CommandService());

            // get the executing assembly directory
            var location = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // load the assemblies, and push their types into the loadedTypes private variable, and return types tagged with RegisterService attribute
            var serviceTypes = LoadAssembliesFromDirectory(location);
            // add the services found to the service provider
            LoadServicesFromTypeList(serviceCollection, serviceTypes);
        }

        private void LoadServicesFromTypeList(IServiceCollection serviceCollection, IEnumerable<Type> types)
        {
            // loop over each of the types found
            foreach (var type in types)
            {
                // find out what interface the type implements
                var attr = type.GetCustomAttribute<RegisterServiceAttribute>();
                // make sure the Interface parameter was passed
                if (attr.Interface != null)
                {
                    // make sure the Interface parameter was actually an interface
                    if (attr.Interface.IsInterface)
                    {
                        // make sure the type implements the interface
                        if (attr.Interface.IsAssignableFrom(type))
                        {
                            // add as a singleton, and give an initialization function
                            serviceCollection.AddSingleton(attr.Interface, type);
                            //serviceCollection.AddSingleton(attr.Interface, (sp) => { return CreateServiceInstance(sp, type); });
                        }
                    }
                }
            }
        }

        private IEnumerable<Type> LoadAssembliesFromDirectory(string location)
        {
            loadedTypes = new List<Type>();
            // get a list of the assemblies in the current directory
            var assemblies = Directory.GetFiles(location, "*.dll");
            // loop through all the assemblies
            foreach (string file in assemblies)
            {
                // load the assembly
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);

                // capture the types
                loadedTypes.AddRange(assembly.GetTypes());
            }
            // if the current assembly didn't get loaded (i.e. EXE instead of DLL)
            if (!loadedTypes.Contains(GetType()))
                // pull the types from the current assembly into the list too
                loadedTypes.AddRange(Assembly.GetExecutingAssembly().GetTypes());

            // find all types with an IRegisterService attribute
            var types = loadedTypes.Where(t => t.GetCustomAttributes<RegisterServiceAttribute>().Count() > 0);
            return types;
        }

        private static object CreateServiceInstance(IServiceProvider sp, Type serviceType)
        {

            // find the first available constructor
            var typeInfo = serviceType.GetTypeInfo();
            var ctor = typeInfo.DeclaredConstructors.First();

            // get the parameters to the constructor
            var parameters = ctor.GetParameters();
            List<object> paramValues = new List<object>();
            foreach (var parameter in parameters)
            {
                paramValues.Add(sp.GetService(parameter.ParameterType));
            }

            return Activator.CreateInstance(serviceType, paramValues);
        }

        public async Task RunBotAsync()
        {
            // ensure log method is set up before anything else is done

            RegisterEvents();

            await RegisterCommandsAsync();

            await Client.LoginAsync(TokenType.Bot, BotConfiguration.AuthenticationToken);

            await Client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task RegisterCommandsAsync()
        {
            //await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);
            var moduleTypes = loadedTypes.Where(t => !t.IsAbstract && typeof(CommandModuleBase).IsAssignableFrom(t));
            foreach (var type in moduleTypes)
            {
                await Commands.AddModuleAsync(type, ServiceProvider);
            }
        }
    }
}
