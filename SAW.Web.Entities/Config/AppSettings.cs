﻿namespace SAW.Web.Entities.Config
{
    public class AppSettings
    {
        public string SqlDatabaseConnectionString { get; set; }
        public string AzureCommunicationsConnectionString { get; set; }
        public JsonWebTokenConfig JsonWebTokenSecret { get; set; }
        public AzureEmailSettings AzureEmailSettings { get; set; }
    }
}
