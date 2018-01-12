# Object Relational Mapped Monitor Extension

### Model-View-Controller / Adapter Dynamically Linked Library ([MVC]/A [DLL])

---

## Task-based Asynchronous Pattern ([TAP])

* CREATED BY:   [Latency McLaughlin]
* FRAMEWORK:    [.NET] v[4.7.1] & Core v[[2.0]](https://www.microsoft.com/net/download/windows)
* LANGUAGE:     [C#] (v7.0)
* GFX SUBSYS:   [WPF]
* SUPPORTS:     [Visual Studio] 2017, 2015, 2013, 2012, 2010, 2008
* UPDATED:      1/5/2018
* VERSION:      [2.3.2](https://www.nuget.org/packages/ORM-Monitor/2.2.0/)
* TAGS:         [API], [TAP], [TPL], [ORM], [MVC], [AMI], [.NET], [C#], [WPF], [Parametric Polymorphism]

### Screenshot
 <img src="https://github.com/Latency/ORM-Monitor/blob/master/ORM-Monitor.png?raw=true">

<hr>

## Navigation
* <a href="#background">Background</a>
* <a href="#introduction">Introduction</a>
* <a href="#overview">Overview</a>
* <a href="#installation">Installation</a>
* <a href="#using">Using the code</a>
* <a href="#other">Output</a>
* <a href="#other">Other features</a>
* <a href="#references">References</a>
* <a href="#license">License</a>

<hr>

<h2><a name="background">Background</a></h2>

There have been many instances throughout my career where incompetent developers try to release code to the company and its stakeholders which deadlock underlying dependency OSI layers which hinder project performance and scheduling.
Those tasked to sustain code as dependency will be frustrated debugging these efforts when trying to qualify responsibility of failure.

The goal was to develop an [API] that would be universal and simple to use in helping eliminate any '<i>concerns</i>' a.k.a cohesive areas of functionality, by wrapping amateur developers efforts with ease and addressing cross-cutting/horizontal concerns as well.
The goal was to reduce time & development costs in validating efforts for risk mitigation serving as a universal multi-functional paradigm.
Any developer can therefore use it to construct new code with the same underlying mechanics and reduce boilerplate code.

When we reference an <i>Object Relational Mapped</i> sub-system it means it is polymorphic and is generic by convention that doesn't depend on specific references to types.  Think of it as a universal template which can inject the types in question as its specified type parameter and let any of the mapping be done under the hood in a <i>dynamic</i> manor.

Some nice things about the C# language is that it is far superior to Java and can detect within a certain tolerance what the type might be so that the type parameter can be omitted from its calling convention.
Note that both languages have Reflection built-in, which technically makes it is possible to drill down many OSI layers and pull out information used for dynamic / RT calling conventions.

Java has limitations and has nothing to offer of greater value which is described here for [Comparison].
In fact, our methodology for this article is to use '<i>generic programming</i>'; a feature set of Java not properly supported since reflection in Java cannot be used to construct new generic realizations nor allows generics directly for primitive types.

<h2><a name="introduction">Introduction</a></h2>

This article introduces an [API] which wraps processes asynchronously; supporting deadlock protection, timeout, cancellation requests, checkpointing, and a parametric polymorphic [MVC] design by convention.

<p align="center">
 <img src="https://github.com/Latency/ORM-Monitor/blob/master/Flow-Chart.png?raw=true" alt="Flow Chart of ORM-Monitor">
</p>

Callback support for the following delegates:
* OnCompleted
* OnTimeout
* OnExit
* OnProgressChanged
* OnRunning
* OnCancellation

[ORM-Monitor] is compiled as a library packaged for the [NuGet] marketplace.  The project itself, supplies an application driver, test suite, and the library's source code.

Model-View-Controller ([MVC]) is an architectural pattern that separates application's data model and user interface views into separate components. This is what the definition says. However, to fully understand the [MVC] we have to introduce required terms and point benefits of [MVC].

<p align="center">
 <img src="http://www.ii.uni.wroc.pl/~wzychla/images/mvc.png" alt="A wiki image of MVC">
</p>

The view is a user interface element (typically a window) that presents the data to the user and allows the user to change the data. It is a typical situation to have several views active at the same time (for example in the Multiple-Document Interface).

The controller is a connection point between the model and views: views register in the controller and reports changes in the model to them.

Forget about the [data model](https://github.com/Latency/ORM-Monitor/blob/master/DataModel.md) and focus on replacing this with an adapter.  Model–view–adapter [MVA] or mediating-controller [MVC] is a software architectural pattern and multitier architecture.
The model–view–adapter solves this rather differently from the model–view–controller by arranging model, adapter or mediating controller and view linearly without any connections whatsoever directly between model and view.

What do you call something that has connections directly related?   Dependancies!  If a dependancy breaks down, so does the rest of the system.  Think about it!  In an alternative scenario, you can use [DI] to replace additional constraints that may be binded as well.   This is where aspect oriented programming ([AOP]) will reduce the amount of boilerplate code which in effect also reduces the maintence efforts and cost along with the project size and complexity.

---

This API has several benefits, such as:

-- *[ORM-Monitor] helps you to know exactly what your code does.*
* Removes pitfalls, anti-patterns, and programming mistakes.
* Eliminate code redundancy.
* Reusable functional procedural code.
* Decrease RAD time.
* Optomizes performance of asynchronous code.
* Fix performance problems easily.
* Inject delegates in real time.
* Reduce boilerplate code.

-- *Flexible delegate injection for any situation.*
* Dependancy injection:<br>
  Design patterns that implement inversion of control for resolving dependencies.
* Labmda expressions:<br>
  An anonymous function that you can use to create delegates or expression tree types and supersede anonymous methods for writing inline code.
* Anonymous methods:<br>
  Constructs a way to pass a code block as a delegate parameter used in building up the *TaskEvent* class.
* Generic types:<br>
  Independant type parameters act as a placeholder for a specific type that a client specifies when they instantiate a variable of the generic type.

<h2><a name="overview">Overview</a></h2>

* <b>Graphical user interface samples.</b><br>
 A clever user-interface is included which offers a view to spawn and cancel tasks.

* <b>Async / Await support.</b><br>
 Task monitoring and dispatching utilizes [TPL] found in [.NET]'s v4+ async [API] which is fully supported by every task is wrapped and invocated based on its delegate.  Tightly coupled integration of multi-threaded delegates with composite [TPL] mechanics to offer thread-safe immediate and explicit timeout and termination functionality.   

* <b>Create reusable parametric polymorphic / [lambda expression] / FP methods in code.</b><br>
 When running your tests, it might be that you want to create a sample of functional or regressive tasks.  Interchanging the delegates make it easy.  For example, switching contexts from an administrator area in your [CMS] / web-application.

* <b>Multiple O/R mapper framework concurrent scheduling.</b><br>
 [ORM-Monitor] uses a wrapping factory for the delegates supplied as its generic type. It doesn't matter whether your application uses multiple event handlers with specific listeners for each.  This [API] will consolidate those into a universal system regardless of type.   It will reduce the boilerplate code and overhead which will ultimately reduce the overall risk of failure since everything will funnel through the same sub-system.

<h2><a name="installation">Installation</a></h2>

This library can be installed from [NuGet]:

There are plug-ins that this project uses as dependency from [NuGet] that are built-in and merged into the [API].


<table>
  <tr>
    <th width="300" style="min-width:300px; max-width: 300px">Description</th>
    <th width="587" style="min-width:531px;" colspan="2">Assembly - GUI</th>
  </tr>
    <tr>
    <td>External References</td>
    <td><i>ORM-Monitor</i> - v2.3.2</td>
    <td><i>Microsoft.ExceptionMessageBox</i> - v11.0.5906.29906</td>
  </tr>
  <tr>
    <td>Exception logging & UI application hooks</td>
    <td><i>ReflectSoftware.Insight</i> - v5.7.1.1</td>
    <td><i>Newtonsoft.Json - JSON.NET</i> - v10.0.3</td>
  </tr>
</table>

<table>
  <tr>
    <th width="300" style="min-width:300px; max-width: 300px">Description</th>
    <th width="587" style="min-width:531px;" colspan="2">Assembly - DLL</th>
  </tr>
   <tr>
    <td>External References</td>
    <td><i>Microsoft.NETCore.App</i> - v2.0.0</td>
  </tr> 
  <tr>
    <td>API</td>
    <td><i>ORM-Monitor</i> - v2.3.2</td>
  </tr>
</table>

<table>
  <tr>
    <th width="300" style="min-width:300px; max-width: 300px">Description</th>
    <th width="587" style="min-width:531px;" colspan="3">Assembly - Tests</th>
  </tr>
  <tr>
    <td>External References</td>
    <td><i>ORM-Monitor</i> - v2.3.2</td>
    <td><i>Microsoft.NETCore.App</i> - v2.0.0</td>
  </tr>
  <tr>
    <td>Unit Testing</td>
    <td><i>NUnit</i> - v3.9.0</td>
    <td><i>NUnit3TestAdapter</i> - v3.9.0</td>
  </tr>
</table>


<h2><a name="using">Using The Code</a></h2>

There are four essential steps to using this:

&nbsp;1. Optional:<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Setup a `TaskEventArgs.Expression` delegate collection.<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Make any <i>delegate</i> use the expression, which is basically a wrapper for invocating a method<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;across boundary switching thread contexts.

```csharp
delegate void Expr(params string[] args);

var actionCollection = new List<Delegate>(
  new Delegate[] {
    (Action<string,string>)  ((name, status) => Debug.WriteLine($"@{name}: {status}")                 ),
    (Expr)                   (arg => Debug.WriteLine($"@{arg[0]}: Task ID({arg[1]}) has canceled.")   )
  }
);
```

&nbsp;2. Create a \`<i>TaskEvent</i>\` with type parameter matching the type from #1. (if specified)<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Using <i>object initialization</i>, assign the properties and specify the timeout<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(milliseconds / TimeSpan) whereby the default (-1) for infinate.<br>
&nbsp;3. Specify the `OnRunning` event handler delegate.  (<b>Required</b>)<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;There is an exception handler for this in the \`<i>AsyncMonitor</i>\` method if omitted.<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i><b>Optional</b></i>:<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Construct the `OnExit`, `OnProgressChanged`, `OnCanceled`, `OnCompleted`, and `OnTimeout`<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;event handler delegates similarly.<br>
&nbsp;4. Invoke the extension method.<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i>[TaskEvent.cs]</i>:   Extension methods for asynchronous routines.

### Usage:
```csharp
var t1 = new TaskEvent(timeout) {
  ...object initializers...
};
Task task = t1.AsyncMonitor();
```

### Example #1
```csharp
var timeout = Convert.ToDouble(Settings.TTL);
var actionCollection = new List<Delegate>(
  new Delegate[] {
    (Action<string,string>)  ((name, status) => Debug.WriteLine($"@{name}: {status}")                 ),
    (Expr)                   (arg => Debug.WriteLine($"@{arg[0]}: Task ID({arg[1]}) has canceled.")   )
  }
);

var t1 = new TaskEvent(timeout) {
  Name = "t1",
  Expression = actionCollection,
  OnRunning = (obj, tea) => {
    SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 2));
  },
  OnCompleted = (th, tea) => {
    tea.Expression[0].DynamicInvoke(tea.Name, Messages.Completed);
  },
  OnTimedout = (th, tea) => {
    tea.Expression[0].DynamicInvoke(tea.Name, Messages.Timeout);
    Assert.Fail($"timedout @{tea.Name}");
  },
  OnCanceled = (th, tea) => {
    tea.Expression[1].DynamicInvoke(new object[] { new[] { tea.Name, tea.Task.Id.ToString()}});
    Assert.Fail($"canceled @{tea.Name}");
  },
  OnExited = (th, tea) => {
    tea.Expression[0].DynamicInvoke(tea.Name, Messages.Exited);
    tasks.Remove(tea.Task);
  }
};

tasks.Add(t1.AsyncMonitor());
```

1. Run \`<i>.\GUI\\&lt;Release&gt;\TaskScheduler.exe</i>\`.
2. Click the `Start New Task` button in the window pane to spawn a new event.
3. Click the `Cancel` button to stop the event.
4. Click the `Remove` button to remove the row from the list.

<h2><a name="output">Output</a></h2>

<p align="center">
 <img src="https://github.com/Latency/ORM-Monitor/blob/master//Output.png?raw=true" alt="Output">
</p>

<h2><a name="other">Other Features</a></h2>

- Unit Tests:<br>
  The unit test uses [NUnit] to help qualify the underlying [API].   Included is a sample that can be ran and tested againsted a variety of mock senario conditions.
- UI Application:<br>
  In addition to being able to use the [API] library as standalone, the project consists of a user-interface I developed using [WPF].

<h2><a name="references">References</a></h2>

 [TPL], [.NET], [ORM], [IoC], [DI], [Generics], [Delegates], [EventHandlers], [Parametric Polymorphism]

<h2><a name="license">License</a></h2>

[GNU LESSER GENERAL PUBLIC LICENSE] - Version 3, 29 June 2007


[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job.)

   [GNU LESSER GENERAL PUBLIC LICENSE]: <http://www.gnu.org/licenses/lgpl-3.0.en.html>
   [Comparison]: <https://en.wikipedia.org/wiki/Comparison_of_C_Sharp_and_Java>
   [TaskEvent.cs]: <https://github.com/Latency/ORM-Monitor/blob/master/DLL/TaskEvent.cs>
   [NuGet]: <https://www.nuget.org/packages/ORM-Monitor/>
   [.NET]: <https://en.wikipedia.org/wiki/.NET_Framework/>
   [WPF]: <https://en.wikipedia.org/wiki/Windows_Presentation_Foundation/>
   [Visual Studio]: <https://en.wikipedia.org/wiki/Microsoft_Visual_Studio/>
   [Latency McLaughlin]: <https://www.linkedin.com/in/Latency/>
   [API]: <https://en.wikipedia.org/wiki/Application_programming_interface>
   [AOP]: <https://en.wikipedia.org/wiki/Aspect-oriented_programming>
   [Parametric Polymorphism]: <https://en.wikipedia.org/wiki/Parametric_polymorphism>
   [ORM-Monitor]: <https://github.com/Latency/ORM-Monitor/>
   [TAP]: <https://msdn.microsoft.com/en-us/library/hh873175(v=vs.110).aspx>
   [AMI]: <https://en.wikipedia.org/wiki/Asynchronous_method_invocation>
   [TPL]: <https://msdn.microsoft.com/en-us/library/dd460717(v=vs.110).aspx>
   [ORM]: <https://en.wikipedia.org/wiki/Object-relational_mapping>
   [C#]: <https://en.wikipedia.org/wiki/C_Sharp_(programming_language)>
   [DLL]: <https://en.wikipedia.org/wiki/Dynamic-link_library>
   [MVC]: <https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller>
   [MVA]: <https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93adapter>
   [CMS]: <https://en.wikipedia.org/wiki/Content_management_system>
   [IoC]: <https://msdn.microsoft.com/en-us/library/ff921087.aspx>
   [DI]: <https://en.wikipedia.org/wiki/Dependency_injection>
   [Generics]: <https://en.wikipedia.org/wiki/Generic_programming>
   [Delegates]: <https://msdn.microsoft.com/en-us/library/ms173171.aspx>
   [EventHandlers]: <https://msdn.microsoft.com/en-us/library/2z7x8ys3(v=vs.90).aspx>
   [NUnit]: <https://en.wikipedia.org/wiki/NUnit>
   [lambda expression]: <https://msdn.microsoft.com/en-us/library/bb397687.aspx>