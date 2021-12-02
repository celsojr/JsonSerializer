using System;
using JsonSerializer;

/*
* Please provide implementation for the folowing method ToJson.
* Use of external libraries is FORBIDDEN.
* Method should convert any object with simple properties into JSON representation.
* Input object WILL contain the folowing:
* - properties of type int
* - properties of type string
* - properties of type object
* - properties of type int[], string[] or object[]
*/

static string ToJson(object model) {
    return JsonConsole.Serialize(model);
}

// Example

var person = new {
	name = "John Doe",
	age = 20,
	address = new[] { "Street name", "20"},
	description = new {
		height = 185,
		model = 90
	}
};

var json = ToJson(person);

// Expected output
// {"name":"John Doe","age":20,"address":["Street name","20"],"description":{"height":185,"model":90}}

Console.WriteLine(json);
