﻿using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientWebPartDefinitionValidator : WebPartModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<WebPartDefinition>("model", value => value.RequireNotNull());

            var pageItem = listItemModelHost.HostListItem;

            WithWithExistingWebPart(pageItem, definition, spObject =>
            {
                var assert = ServiceFactory.AssertService
                                           .NewAssert(model, definition, spObject)
                                                 .ShouldNotBeNull(spObject);

                // some of the properties can actually be validated
                // http://stackoverflow.com/questions/11814829/how-to-read-webpart-content-using-sharepoint-client-om
                // asmx calls are required to get additional information about the current web parts

                assert
                    .SkipProperty(m => m.ZoneIndex, "Property is not available in CSOM. Skipping.")

                    .SkipProperty(m => m.Id, "Property is not available in CSOM. Skipping.")
                    .SkipProperty(m => m.ZoneId, "Property is not available in CSOM. Skipping.")

                    .SkipProperty(m => m.WebpartFileName, "Property is not available in CSOM. Skipping.")
                    .SkipProperty(m => m.WebpartType, "Property is not available in CSOM. Skipping.")
                    .SkipProperty(m => m.WebpartXmlTemplate, "Property is not available in CSOM. Skipping.")

                    .ShouldBeEqual(m => m.Title, o => o.Title);
            });
        }
    }
}
