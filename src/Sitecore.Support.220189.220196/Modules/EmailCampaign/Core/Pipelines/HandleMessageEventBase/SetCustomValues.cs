namespace Sitecore.Support.Modules.EmailCampaign.Core.Pipelines.HandleMessageEventBase
{
    using Sitecore.Analytics.Tracking.External;
    using Sitecore.Diagnostics;
    using Sitecore.EmailCampaign.Model.Analytics;
    using Sitecore.EmailCampaign.Model.Exceptions;
    using Sitecore.ExM.Framework.Diagnostics;
    using Sitecore.Modules.EmailCampaign.Factories;
    using Sitecore.Modules.EmailCampaign.Messages;
    using Sitecore.Modules.EmailCampaign.Core.Pipelines.HandleMessageEventBase;
    public class SetCustomValues
    {
        private readonly EcmFactory _factory;

        private readonly ILogger _logger;

        public SetCustomValues(ILogger logger) : this(EcmFactory.GetDefaultFactory(), logger)
        {
        }

        internal SetCustomValues(EcmFactory factory, ILogger logger)
        {
            Assert.ArgumentNotNull(factory, "factory");
            Assert.ArgumentNotNull(logger, "logger");
            this._factory = factory;
            this._logger = logger;
        }

        public void Process(HandleMessageEventPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentCondition(args.MessageItem != null, "args", "MessageItem not set");
            Assert.ArgumentCondition(args.TouchPointRecord != null, "args", "TouchPointRecord not set");
            MessageItem messageItem = args.MessageItem;
            TouchPointRecord arg_DE_0 = args.TouchPointRecord;
            ExmCustomValues exmCustomValues;
            //sitecore.support.220189.220196
            /*if (string.IsNullOrWhiteSpace(args.TargetLanguage) && args.MessageEventName != "Dispatch Failed")
            {
                exmCustomValues = this.GetCustomValues(args, messageItem);
            }*/
            //else
            {
                exmCustomValues = new ExmCustomValues
                {
                    Email = args.Email,
                    ManagerRootId = messageItem.ManagerRoot.InnerItem.ID.ToGuid(),
                    MessageLanguage = args.TargetLanguage,
                    MessageId = args.MessageId.ToGuid(),
                    TestValueIndex = args.TestValueIndex
                };
            }
            if (exmCustomValues == null)
            {
                throw new MessageEventPipelineException("Custom values not found for " + args);
            }
            string text;
            if (ExmCustomValuesHolder.ContainsCustomValuesHolderKey(arg_DE_0.CustomValues, out text))
            {
                throw new MessageEventPipelineException("Touch point already contains customvalues for " + args);
            }
            ExmCustomValuesHolder exmCustomValuesHolder = new ExmCustomValuesHolder();
            exmCustomValuesHolder.ExmCustomValues.Add(1, exmCustomValues);
            arg_DE_0.CustomValues["ScExmHolder"] = exmCustomValuesHolder;
        }

        //sitecore.support.220189.220196
        /*private ExmCustomValues GetCustomValues(HandleMessageEventPipelineArgs args, MessageItem messageItem)
        {
            ExmCustomValues customValues;
            try
            {
                customValues = this._factory.Bl.GetRecipientStateManager(args.ContactId.Guid, messageItem).CustomValues;
            }
            catch (Exception e)
            {
                this._logger.LogError("Failed to retrieve custom EXM values", e);
                throw;
            }
            return customValues;
        }*/
    }
}