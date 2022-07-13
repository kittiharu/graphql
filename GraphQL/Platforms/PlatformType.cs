using System.Linq;
using CommanderGQL.Data;
using CommanderGQL.Models;
using HotChocolate;
using HotChocolate.Types;

namespace CommanderGQL.GraphQL.Platforms
{
    public class PlatformType : ObjectType<Platform>
    {
        protected override void Configure(IObjectTypeDescriptor<Platform> descriptor)
        {
            descriptor.Description("Represents any software or services that has a command line");

            descriptor.Field(p => p.LicenseKey).Ignore();

            descriptor.Field(p => p.Commands)
                .ResolveWith<PlatformResolvers>(p => p.GetCommands(default, default!))
                .UseDbContext<AppDbContext>()
                .Description("This is the list of available commands for the platform");

            //descriptor.Field<PlatformResolvers>(p => p.GetCommands(default, default));
        }
    }

    public class PlatformResolvers
        {
            public IQueryable<Command> GetCommands([Parent] Platform platform, [ScopedService] AppDbContext context)
            {
                return context.Commands.Where(p => p.PlatformId == platform.Id);
            }
        }
}