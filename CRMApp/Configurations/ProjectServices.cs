using CRMApp.Middleware;
using CRMApp.Services;

namespace CRMApp.Configurations
{
	public static class ProjectServices
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped<ICustomerService, CustomerService>();
			services.AddScoped<IContactService, ContactService>();
			services.AddScoped<IActivityLogger, ActivityLogService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<INoteService, NoteService>();
			services.AddScoped<IContactInquiryService, ContactInquiryService>();
			services.AddScoped<IAppLogger, AppExceptionHandlerAndLogger>();
			return services;
		}
	}
}
