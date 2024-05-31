Console.WriteLine("Hello, World!");

/* This can be uncommented to test a call against the server (be sure it's running)

using Consumer;

const string baseUrl = "http://localhost:5107/";

// Didn't bother with Nwag base URL settings
var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
var client = new ExampleClient(baseUrl, httpClient);

var results = await client.TodosAsync(new [] {"1", "2"});

Console.WriteLine("Results: " + string.Join(", ", results));

*/
