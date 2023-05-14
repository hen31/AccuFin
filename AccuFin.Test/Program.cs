// See https://aka.ms/new-console-template for more information
using AccuFin.Api.Client;
using AccuFin.Api.Client.Authentication;
using AccuFin.Api.Models;
using AccuFin.Api.Models.Authentication;

Console.WriteLine("AccuFin test application");


MainAsync().Wait();

async Task MainAsync()
{
    var input = Console.ReadLine();

    if (input == "1")
    {
        await DoRegistrationFlow();
    }

    if (input == "2")
    {
        await DoEmailWorkFlow(null);
    }
}



async Task DoRegistrationFlow()
{
    (bool succes, string email) registerResult;
    do
    {
        registerResult = await TryRegister();
    } while (!registerResult.succes);
    await DoEmailWorkFlow(registerResult.email);

    Console.ReadLine();
}

static async Task<(bool, string email)> TryRegister()
{
    AuthenticationClient authenticationClient = new AuthenticationClient(new HttpClient());
    Console.WriteLine("Emailadress");
    string email = Console.ReadLine();
    Console.WriteLine("Password");
    string password = Console.ReadLine();
    var response = await authenticationClient.RegisterUserAsync(email, password);
    return HandleResult(email, response);
}

static async Task DoEmailWorkFlow(string email)
{
    if (string.IsNullOrEmpty(email))
    {
        Console.WriteLine("Emailadress");
        email = Console.ReadLine();
    }
    (bool succes, string email) registerResult;
    do
    {
        registerResult = await TryConfirm(email);
    } while (!registerResult.succes);
    
}

static async Task<(bool, string email)> TryConfirm(string email)
{
    AuthenticationClient authenticationClient = new AuthenticationClient(new HttpClient());
    Console.WriteLine("Enter code for email confirmation");
    string emailCode = Console.ReadLine();

    var reponse = await authenticationClient.ConfirmEmailAsync(email, emailCode);
    return HandleResult(email, reponse);
}

static (bool, string email) HandleResult(string email, Response<string, List<ValidationError>> response)
{
    if (!response.Success)
    {
        Console.WriteLine("Errors");
        foreach (var error in response.ErrorData)
        {
            Console.WriteLine(error.Description);

        }
        return (false, null);
    }
    else
    {
        Console.WriteLine("Succes");
        return (true, email);
    }
}