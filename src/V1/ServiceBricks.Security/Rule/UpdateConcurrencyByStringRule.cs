using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ServiceBricks
{
    /// <summary>
    /// This is a business rule for domain objects that have the UpdateDate property.
    /// It ensures that it is populated for create and updates.
    /// </summary>
    public partial class UpdateConcurrencyByStringRule<TDomainObject> : BusinessRule where TDomainObject : IDomainObject<TDomainObject>
    {
        /// <summary>
        /// Internal.
        /// </summary>
        protected readonly ILogger _logger;

        public const string Key_PropertyName = "UpdateConcurrencyByStringRule_PropertyName";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="loggerFactory"></param>
        public UpdateConcurrencyByStringRule(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UpdateConcurrencyByStringRule<TDomainObject>>();
            Priority = PRIORITY_LOW;
        }

        /// <summary>
        /// Register a rule for a domain object.
        /// </summary>
        public static void RegisterRule(
            IBusinessRuleRegistry registry,
            string propertyName)
        {
            var custom = new Dictionary<string, object>();
            custom.Add(Key_PropertyName, propertyName);

            registry.RegisterItem(
                typeof(DomainCreateBeforeEvent<TDomainObject>),
                typeof(UpdateConcurrencyByStringRule<TDomainObject>),
                custom);

            registry.RegisterItem(
                typeof(DomainUpdateBeforeEvent<TDomainObject>),
                typeof(UpdateConcurrencyByStringRule<TDomainObject>),
                custom);
        }

        /// <summary>
        /// Execute the business rule.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IResponse ExecuteRule(IBusinessRuleContext context)
        {
            var response = new Response();

            try
            {
                if (context.Object is DomainUpdateBeforeEvent<TDomainObject> eu)
                {
                    //Get the property name from the custom context
                    if (context.Data == null || !context.Data.ContainsKey(Key_PropertyName))
                        throw new Exception("Context missing propertyname");
                    var propName = context.Data[Key_PropertyName];
                    if (propName == null)
                        throw new Exception("Context propertyname invalid");
                    string propertyName = propName.ToString();

                    var newProp = eu.DomainObject.GetType().GetProperty(propertyName);
                    newProp.SetValue(eu.DomainObject, Guid.NewGuid().ToString());
                }
                if (context.Object is DomainCreateBeforeEvent<TDomainObject> ei)
                {
                    //Get the property name from the custom context
                    if (context.Data == null || !context.Data.ContainsKey(Key_PropertyName))
                        throw new Exception("Context missing propertyname");
                    var propName = context.Data[Key_PropertyName];
                    if (propName == null)
                        throw new Exception("Context propertyname invalid");
                    string propertyName = propName.ToString();

                    var newProp = ei.DomainObject.GetType().GetProperty(propertyName);
                    newProp.SetValue(ei.DomainObject, Guid.NewGuid().ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
            }

            return response;
        }
    }
}