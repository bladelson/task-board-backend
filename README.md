local.settings.json for azure function

```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "MongoConnectionString": "mongodb://root:pw@localhost:27017/?authMechanism=SCRAM-SHA-256"
    }
}
```