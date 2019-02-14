# MVVQueues #
.NET Core Queue. Easy to use


## Usage 
1. Create a queue with desired number of workers
2. Create a a command and push it to a queue whenver ready

## Queue Commands
Are the structures that have to be executed. 

Create your own command to be executed and make sure to override methods
- ExecuteCommand() with execution body
- OnError() with body that needs to be executed on error
- OnSuccess() with body that needs to executed after the completition of the first method


Note:
> Commands can (and should) create commands that need to happed in case of successful execution and on error

```csharp
public class DBCommandCreateEntity:MVVQueues.MVVQueueCommand
{
    string _path = "";
    

    public DBCommandCreateEntity(MVVQueues.MVVQueue queue, string bookName):base(queue)
    {
        _path = path;
    }

    public override void ExecuteCommand()
    {
        // add row with book to table
        
        // DO Following:
        // 1. Create Command to export pricelist for books
        // 2. Push it to queue
    }
    
    public override void OnError(Exception ex)
    {
        // if it was a connection exception:
        // 1. create your command to connect to you DB again
        // 2. push it to a queue
    }
}

```

Then create a queue with desired number of workers and push your commands, when ready

```csharp

      // queue create
      MVVQueues.MVVQueue queu = new MVVQueues.MVVQueue(1);

      // create book whenever next worker is free
      queu.Push(new DBCommandCreateEntity(queu, "Using MVVQueues"));

```


