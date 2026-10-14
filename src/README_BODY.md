## Screenshots

### [WinForms]
![AsyncTask](Resources/Console.png)

### [WPF]
![AsyncTask](Resources/AsyncTask.png)

---

## Navigation

* [Background](#background)
* [Introduction](#introduction)
* [Overview](#overview)
* [Output](#output)
* [Other features](#other-features)
* [Usage](#usage)

---

## Background

There have been many instances throughout my career where incompetent developers try to release code to the company and its stakeholders which deadlock underlying dependency OSI layers, and hinder project performance and scheduling.
Those tasked to sustain code, will be frustrated debugging these efforts when trying to qualify responsibility of failure.

The goal was to develop an [API], using Task-based Asynchronous Pattern [TAP], that would be universal and simple to use in helping eliminate any '*concerns*' a.k.a cohesive areas of functionality, by wrapping amateur developers efforts with ease and addressing cross-cutting/horizontal concerns as well.
The goal was to reduce time & development costs in validating efforts for risk mitigation serving as a universal multi-functional paradigm.
Any developer can therefore use it to construct new code with the same underlying mechanics and reduce boilerplate code.

Some nice things about the [C#] language is that it is far superior to Java. It can detect within a certain tolerance what the type might be so that the type parameter can be omitted from its calling convention.
Note that both languages have Reflection built-in, which technically makes it is possible to drill down many OSI layers and pull out information used for dynamic / RT calling conventions.

Java has limitations and has nothing to offer of greater value which is described here for [Comparison].
In fact, our methodology for this article is to use '*generic programming*'; a feature set of Java not properly supported since reflection in Java cannot be used to construct new generic realizations nor allows generics directly for primitive types.

## Introduction

This article introduces an [API] which wraps processes asynchronously; supporting deadlock protection, timeout, cancellation requests, checkpointing, and a parametric polymorphic [MVC] design by convention.

![AsyncTask](Resources/Flow-Chart.png)

Callback support for the following delegates:
* OnAdd
* OnRemove
* OnComplete
* OnError
* OnCanceled
* OnTick
* OnTimeout

[AsyncTask] is compiled as a library packaged for the [NuGet] marketplace.  The project itself, supplies an application driver, test suite, and the library's source code.

### Model-View-Controller / Adapter Dynamically Linked Library ([MVC]/[MVA] [DLL])

Model-View-Controller ([MVC]) is an architectural pattern that separates application's data model and user interface views into separate components. This is what the definition says. However, to fully understand the [MVC] we have to introduce required terms and point benefits of [MVC].

![AsyncTask](Resources/mvc_process.svg)

The view is a user interface element (typically a window) that presents the data to the user and allows the user to change the data. It is a typical situation to have several views active at the same time (for example in the Multiple-Document Interface).

The controller is a connection point between the model and views: views register in the controller and reports changes in the model to them.

Forget about the [data model](Resources/DataModel.md) and focus on replacing this with an adapter.  Model–view–adapter [MVA] or mediating-controller [MVC] is a software architectural pattern and multitier architecture.
The model–view–adapter solves this rather differently from the model–view–controller by arranging model, adapter or mediating controller and view linearly without any connections whatsoever directly between model and view.

What do you call something that has connections directly related?   Dependancies!  If a dependancy breaks down, so does the rest of the system.  Think about it!  In an alternative scenario, you can use [DI] to replace additional constraints that may be binded as well.   This is where aspect oriented programming ([AOP]) will reduce the amount of boilerplate code which in effect also reduces the maintence efforts and cost along with the project size and complexity.

---

This API has several benefits, such as:

-- *[AsyncTask] helps you to know exactly what your code does.*
* Removes pitfalls, anti-patterns, and programming mistakes.
* Eliminate code redundancy.
* Reusable functional procedural code.
* Decrease RAD time.
* Optomizes performance of asynchronous code.
* Fix performance problems easily.
* Inject delegates in real time.
* Reduce boilerplate code.

-- *Flexible delegate injection for any situation.*
* Dependancy injection:\
  Design patterns that implement inversion of control for resolving dependencies.
* Labmda expressions:\
  An anonymous function that you can use to create delegates or expression tree types and supersede anonymous methods for writing inline code.
* Anonymous methods:\
  Constructs a way to pass a code block as a delegate parameter used in building up the *TaskEvent* class.
* Generic types:\
  Independant type parameters act as a placeholder for a specific type that a client specifies when they instantiate a variable of the generic type.

## Overview

* **Graphical user interface samples.**\
 A clever user-interface is included which offers a view to spawn and cancel tasks.

* **Async / Await support.**\
 Task monitoring and dispatching utilizes [TPL] found in [.NET]'s v4+ async [API] which is fully supported by every task is wrapped and invocated based on its delegate.  Tightly coupled integration of multi-threaded delegates with composite [TPL] mechanics to offer thread-safe immediate and explicit timeout and termination functionality.   

* **Create reusable parametric polymorphic / [lambda expressions] / FP methods in code.**\
 When running your tests, it might be that you want to create a sample of functional or regressive tasks.  Interchanging the delegates make it easy.  For example, switching contexts from an administrator area in your [CMS] / web-application.

* **Task Parallel Library ([TPL]) concurrent scheduling.**\
 [AsyncTask] uses a wrapping factory for the delegates supplied as its generic type. It doesn't matter whether your application uses multiple event handlers with specific listeners for each.  This [API] will consolidate those into a universal system regardless of type.   It will reduce the boilerplate code and overhead which will ultimately reduce the overall risk of failure since everything will funnel through the same sub-system.

## Output

![AsyncTask](Resources/Output.png)

## Other Features

- Unit Tests:\
  The unit test uses [NUnit] to help qualify the underlying [API].   Included is a sample that can be ran and tested againsted a variety of mock senario conditions.
- UI Application:\
  In addition to being able to use the [API] library as standalone, the project consists of a user-interface developed using [WPF].

## Usage

Create a `TaskList` to track the running tasks, then create each `AsyncTask` with the work to perform and configure it through `TaskInfo` and the event callbacks.  Call `Start()` to run it.

```csharp
using AsyncTask.Models;

var       taskList = new TaskList();
const int timeout  = 5;

var t1 = new AsyncTask.AsyncTask((task, args) =>
{
    do
    {
        task.TaskInfo.Token.ThrowIfCancellationRequested();
        Thread.Sleep(250);
    }
    while (args.Duration < TimeSpan.FromSeconds(2));
})
{
    TaskInfo = new TaskInfo
    {
        Name                   = "t1",
        Timeout                = TimeSpan.FromSeconds(timeout),
        TaskList               = taskList,
        PollInterval           = new(0, 0, 1),
        SynchronizationContext = SynchronizationContext.Current
    },
    OnAdd      = (asyncTask, _)    => Console.WriteLine($"Adding '{asyncTask.TaskInfo.Name}'."),
    OnRemove   = (asyncTask, _)    => Console.WriteLine($"Removing '{asyncTask.TaskInfo.Name}'."),
    OnComplete = (asyncTask, _)    => Console.WriteLine($"Completed '{asyncTask.TaskInfo.Name}'."),
    OnTick     = (asyncTask, args) => Console.WriteLine($"Duration:  {args.Duration:hh\\:mm\\:ss}"),
    OnTimeout  = (asyncTask, _)    => Console.WriteLine($"Timeout for '{asyncTask.TaskInfo.Name}'."),
    OnCanceled = (asyncTask, _)    => Console.WriteLine($"Canceled '{asyncTask.TaskInfo.Name}'.")
};
t1.Start();

// Wait
await Task.WhenAll(taskList.Keys.Cast<Task>());
```

- **Delegate:**  The work is supplied as a delegate receiving the running task and its arguments (`args.Duration` is the elapsed time).  Call `task.TaskInfo.Token.ThrowIfCancellationRequested()` to honor cancellation.
- **TaskInfo:**  `Name`, `Timeout`, `PollInterval` (how often `OnTick` fires), `TaskList` and the `SynchronizationContext` the callbacks are posted to.
- **Callbacks:**  `OnAdd`, `OnRemove`, `OnComplete`, `OnTick`, `OnTimeout` and `OnCanceled`.
- **Cancellation:**  Call `Cancel()` on the task to interrupt it, which raises `OnCanceled`.  A task running longer than `Timeout` raises `OnTimeout`.
