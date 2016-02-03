# api-dotnet [![GroupByInc.Api](https://img.shields.io/nuget/v/GroupByInc.Api.svg)](https://www.nuget.org/packages/GroupByInc.Api) 


#### Development

`mvn -Punit` to install and test

#### Searching

```csharp
CloudBridge bridge = new CloudBridge("<client key>", "myCustomerId");
Query query = new Query().SetQuery("dvd");
// Search returns a Newtonsoft JObect
JObject results = bridge.Search(query);
```