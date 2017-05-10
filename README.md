![Datatrics](https://www.datatrics.com/wp-content/themes/datatrics/assets/img/logo/logo.png)

# Datatrics C# API client
This package is a convenience wrapper to communicate with the Datatrics REST-API

## Installation
The library uses Json.Net, you can get it at https://www.nuget.org/packages/newtonsoft.json/

## Usage
There are a lot of API resources that are accessible through this client. You can look them up by looking at the code. Their name matches the name in the documentation(https://doc-beta.datatrics.com).

```csharp
use Datatrics;

Client client = new Client("[api-key]", "[project-id]");
JObject response = await client.Content.Get();
```

__Explanation__

[api-key]
The API key you've received or created

[project-id]
id of the project

## Contributing
We love contributions, but please note that the API client is generated. If you have suggested changes, you may still create a PR, but your PR will not be merged. We will however adapt the generator to reflect your changes. You can also create a GitHub issue if there's something you miss.