# [Nagornev.Graph](https://github.com/nagornev/Nagornev.Graph)

## Information

This library creates a node graph to manage a complex business process. 
The principle of graph creation is based on the "__[Chain of responsibility](https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern)__" pattern. 
If you know this pattern, then you can easily work with this library.

## Installation

Install the current version with __[dotnet](https://dotnet.microsoft.com/ru-ru/)__:

```C#
dotnet add package Nagornev.Graph
```

## Usage

### Quick start

Use this code to write console lines:

``` C#
using Nagornev.Graph.Commands;
using Nagornev.Graph.Invokers;
using Nagornev.Graph.Nodes;

AutomaticInvoker invoker = new AutomaticInvoker();

//Init
ConsistentNode startNode = new ConsistentNode(() =>
                                   FunctionalConsistentCommand.Create(async () =>
                                       {
                                           Console.WriteLine("Handling start node.");
                                       }));

ConditionNode middleNode = new ConditionNode(() =>
                                    FunctionalConditionCommand.Create(async () =>
                                        {
                                            Random random = new Random();

                                            int number = random.Next(10);

                                            bool response = number % 2 == 0;

                                            Console.WriteLine($"Handling middle node. Response {response}.");

                                            return response;
                                        }));

ConsistentNode firstEndNode = new ConsistentNode(() =>
                                    FunctionalConsistentCommand.Create(async () =>
                                        {
                                            Console.WriteLine("Handling first end node.");
                                        }));

ConsistentNode secondEndNode = new ConsistentNode(() =>
                                    FunctionalConsistentCommand.Create(async () =>
                                    {
                                        Console.WriteLine("Handling second end node.");
                                    }));
//Connecting
startNode.SetSuccessor(middleNode);

middleNode.SetSuccessor(true, firstEndNode);
middleNode.SetSuccessor(false, secondEndNode);

//Invoke
invoker.SetStart(startNode);
await invoker.Start();
```

### How to use it?

This library has 3 main components:
 - ```*Node``` - used for connecting between nodes;
 - ```*Command``` - used to set up the next handling node;
 - ```*Invoker``` - used to invoke the node handling;

#### Nodes:

This library has 4 main __nodes__:

- ```ConsistentNode``` - has one successor. Command type is ```ConsistentCommand```;
- ```ConditionNode``` - has two successors (true/false). Command type is ```ConditionCommand```;
- ```ConvertibleNode<T>``` - has an unknown number of successors (the number of successors depends on the type ```T```). Command type is ```ConvertibleCommand<T>```;
- ```ExceptionNode``` - has one successor. Also this node can repeat parent node (the parent node is the node where catched unhandled exception). Command type is ```ExceptionCommand```;

Each of the 3 types of main nodes (```ConsistentNode```, ```ConditionNode```,
```ConvertibleNode<T>```) has additional nodes:

- ```Oneway*Node``` - the command is executed once. After executing the command, the node will return the successor it returned the first time;
- ```Interval*Node``` - the command is executed after skipping the set call interval;
- ```Delayed*Node``` - the command is executed after a period of time; 

Each node has __methods__:

- ```SetSuccessor(Node successor) / SetSuccessor(TConvertibleType key, Node successor)``` - sets the node successor;
- ```SetExceptionSuccessor(ExceptionNode successor)``` - sets the exception node successor;
- ```SetDelay(int delay)``` - sets the delay after execution;
- ```SetSingle(bool single)``` - sets the parameter that controls the command creation;
- ```SetTag(string tag)``` - sets the tag that used the node handling;

Exception node has specific __methods__:

-```SetRepeat(int count)``` - sets the repetition count of the parent node;

#### Commands:

This library has 4 main __commands__:

- ```ConsistentCommand``` - the execution method has the signature: ```Func<Task>```;
- ```ConditionCommand``` - the execution method has the signature: ```Func<Task<bool>>```;
- ```ConvertibleCommand<T>``` - the execution method has the signature: ```Func<Task<T>>```, ```where T: struct, IConvertible```;
- ```ExceptionCommand``` - the execution method has the signature: ```Func<Task>```;

Each main commands has additional commands:

- ```Functional*Command``` - the constructor accept the * delegate;
- ```Callback*Command<T>``` - the command has a special method ```T Call()```, which invoke the acepted callback;

#### Invokers

This library has 2 main invokers:

- ```AutomaticInvoker```
- ```HandInvoker```

To start handling nodes, you need call method ```SetStart(Node node)``` and call method ```Task Start()``` (```AutomaticInvoker```) or call method ```Task Execute()``` (```HandInvoker```).

### Sample


Let's calculate and get the factorial of the number:

```C#
public class FactorialCommand : CallbackConsistentCommand<int>
{
    private readonly int _number;

    private int _factorial;

    public FactorialCommand(int number,
                            params CommandCallbackDelegate<int>[] callbacks) 
        : base(callbacks)
    {
        _number = number;
    }

    protected override async Task Execute()
    {
        _factorial = Factorial(_number);
    }

    private int Factorial(int number)
    {
        if (number == 1) return 1;

        return number * Factorial(number - 1);
    }

    protected override int Call()
    {
        return _factorial;
    }
}

```

Let's create a graph and call it for handling.

```C#
using Nagornev.Graph.Commands;
using Nagornev.Graph.Invokers;
using Nagornev.Graph.Nodes;

int number = int.Parse(Console.ReadLine());

int result = 0;

AutomaticInvoker invoker = new AutomaticInvoker();

//Init
ConsistentNode calculateFactorialNode = new ConsistentNode(() =>
                                            new FactorialCommand(number,
                                                                 (factorial) => result = factorial));


ConsistentNode printFactorialNode = new ConsistentNode(() =>
                                        FunctionalConsistentCommand.Create(async () =>
                                            {
                                                Console.WriteLine($"Factorial {number} is {result}.");
                                            }));
//Connecting
calculateFactorialNode.SetSuccessor(printFactorialNode);

//Invoke
invoker.SetStart(calculateFactorialNode);
await invoker.Start();
```
