using DoctorSystem.Models;
using DoctorSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace DoctorSystem.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public EmailService(
            IEmailSender emailSender,
            ICompositeViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _emailSender = emailSender;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        // ... existing methods ...

        public async Task SendPaymentConfirmationAsync(Payment payment)
        {
            var subject = "Payment Confirmation";
            var htmlMessage = await RenderEmailTemplateAsync("PaymentConfirmation", payment);
            await _emailSender.SendEmailAsync(payment.Patient.Email, subject, htmlMessage);
        }

        private async Task<string> RenderEmailTemplateAsync<TModel>(string templateName, TModel model)
        {
            using var scope = _serviceProvider.CreateScope();
            var httpContext = new DefaultHttpContext { RequestServices = scope.ServiceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using var sw = new StringWriter();
            var viewResult = _viewEngine.FindView(actionContext, $"EmailTemplates/{templateName}", false);

            if (!viewResult.Success)
            {
                throw new InvalidOperationException($"Couldn't find view '{templateName}'");
            }

            var viewDictionary = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewDictionary,
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }

        public Task SendAppointmentConfirmationAsync(Appointment appointment) => Task.CompletedTask;
        public Task SendTestRequestConfirmationAsync(TestRequest request) => Task.CompletedTask;
        public Task SendPrescriptionConfirmationAsync(Prescription prescription) => Task.CompletedTask;
        public Task SendTestResultConfirmationAsync(TestResult result) => Task.CompletedTask;

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await _emailSender.SendEmailAsync(to, subject, body);
        }

        public async Task SendTestResultNotificationAsync(string patientEmail, string patientName, string testType, string resultSummary)
        {
            var subject = $"Test Result: {testType}";
            var body = $"Dear {patientName},\n\nYour test result for {testType} is now available.\n\nResult Summary: {resultSummary}";
            await SendEmailAsync(patientEmail, subject, body);
        }
    }
} 