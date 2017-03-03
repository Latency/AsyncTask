# Object Relational Mapped Monitor Extension

### Model-View-Controller dynamically linked library (MVC-DLL)

---

## Task-based Asynchronous Pattern (TAP)

* CREATED BY: Latency McLaughlin
* REVIEWS:    (2)
* FRAMEWORK:  .NET 4.6.1
* SUPPORTS:   Visual Studio 2015, 2013, 2012, 2010, 2008
* UPDATED:    2/13/2016
* VERSION:    1.0.6
* TAGS:       tasks, TAP, TPL, task scheduler, functional tests, ORM, .NET, C#, asynchronous, parametric polymorphism

### Screenshot <img src="https://github.com/Latency/ORM-Monitor/blob/master/ORM-Monitor.png?raw=true">

* <a href="#background">Background</a>
* <a href="#introduction">Introduction</a>
* <a href="#overview">Overview</a>
* <a href="#installation">Installation</a>
* <a href="#using">Using the code</a>
* <a href="#other">Other features</a>
* <a href="#references">References</a>
* <a href="#license">License</a>

<hr>

<a name="background"><h2>Background</h2></a>
There have been many instances throughout my career where incompetent developers try to release code to the company and its stakeholders which deadlock underlying dependency OSI layers which hinder project performance and scheduling.  Those tasked to sustain code as dependency will be frustrated debugging these efforts when trying to qualify responsibility of failure.

The goal was to develop an API that would be universal and simple to use in helping eliminate any concerns by wrapping amateur developers efforts with ease.
The goal was to reduce time & development costs in validating efforts for risk mitigation serving as a universal multi-functional paradigm.
Any developer can therefore use it to construct new code with the same underlying mechanics and reduce boilerplate code.

When we reference an <i>Object Relational Mapped</i> sub-system it means it is polymorphic and is generic by convention that doesn't depend on specific references to types.  Think of it as a universal template which can inject the types in question as its specified type parameter and let any of the mapping be done under the hood in a <i>dynamic</i> manor.

Some nice things about the C# language is that it can detect within a certain tolerance what the type might be so that the type parameter can be omitted from its calling convention.
Also, that the language has Reflection built-in so we can drill down to many layers and pull out information used for dynamic / RT calling conventions.

<a name="introduction"><h2>Introduction</h2></a>
This article provides an application programming interface (API) which allows one to wrap processes for asynchronous invocation supporting deadlock protection, timeout, cancellation, TTL, and parametric polymorphic MVC design by convention.

Callback support for OnCompleted, OnTimeout, OnExit, OnProgressChanged, OnRunning, & OnCancellation delegates.

It was compiled with C# v6.0 as an extension library and supplying an application driver and test suite for viewing and testing the build.

Model-View-Controller (MVC) is an architectural pattern that separates application's data model and user interface views into separate components. This is what the definition says. However, to fully understand the MVC we have to introduce required terms and point benefits of MVC.

<p align="center">
 <img src="http://www.ii.uni.wroc.pl/~wzychla/images/mvc.png" alt="A wiki image of MVC">
</p>

The data model is a set of data structures that lay the base for the businnes logic of an application. In typical object-oriented application, the data model is built of client-side classes and collections. The data model typically is somehow stored into a Database Management System, however how the data is exactly stored is not a concern of MVC.

The view is a user interface element (typically a window) that presents the data to the user and allows the user to change the data. It is a typical situation to have several views active at the same time (for example in the Multiple-Document Interface).

The controller is a connection point between the model and views: views register in the controller and reports changes in the model to them.

---

This API has several benefits, such as:

-- *ORM Monitor Extension helps you to know exactly what your code does.*
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

<a name="overview"><h2>Overview</h2></a>
* <b>A clever user-interface included which offers an object listview to spawn tasks and cancel events.</b><br>
 The user-interface model-view driver consists of a 3rd party enhancement ObjectlistView API for enhanced list-view displays.

* <b>Async / Await support.</b><br>
 Task Scheduler/ORM code which utilizes TPL in .NET's async API is fully supported by ORM Task Scheduling and every task is wrapped and invocated based on its delegate.  Tightly coupled integration of multi-threaded delegates with composite TPL mechanics to offer thread-safe immediate and explicit timeout and termination functionality.   

* <b>Create reusable parametric polymorphic / lambda expression / FP methods in code.</b><br>
 When running your tests, it might be that you want to create a sample for a variety of tasks. To make this easy, ORM Task Scheduler offers a simple way to create a snapshot from code and start profiling right away. You can also combine this with the production profiling feature to create a snapshot from within your application when you enable profiling in production. For example, this can be done from an administrator area in your web-application.

* <b>Multiple O/R mapper framework concurrent scheduling.</b><br>
 ORM Monitor Extension uses a wrapping factory for the delegates supplied as its object type. It doesn't matter whether your application uses multiple event handlers with specific listeners for each.  This API will consolidate those into a universal system regardless of type.   It will reduce the boilerplate code and overhead which will ultimately reduce the overall risk of failure since everything will funnel through the same sub-system.

<a name="installation"><h2>Installation</h2></a>
This library can be installed with NuGet:  https://www.nuget.org/packages/ORM-Monitor/

There are plug-ins that this project uses as dependency from NuGet that are built-in and merged into the API.


<table>
  <tr>
    <th width="300" style="min-width:300px; max-width: 300px">Description</th>
    <th width="587" style="min-width:531px;" colspan="2">Assembly - GUI</th>
  </tr>
  <tr>
    <td>Telerik UI for WinForms SDK</td>
    <td colspan="2">DevCraft 2015</td>
  </tr>
  <tr>
    <td>External References</td>
    <td><i>Microsoft.ExceptionMessageBox</i> - v11.0.2100.60</td>
  </tr>
  <tr>
    <td>3rd Party Wrapper for .NET ListView</td>
    <td colspan="2"><i>ObjectListView.Official</i> - ObjectListView v2.9.1</td>
  </tr>
</table>

<table>
  <tr>
    <th width="300" style="min-width:300px; max-width: 300px">Description</th>
    <th width="587" style="min-width:531px;" colspan="4">Assembly - DLL</th>
  </tr>
  <tr>
    <td>Exception logging &amp; UI application hooks</td>
    <td><i>ReflectSoftware.Insight</i> - v5.6.1.2</td>
    <td><i>Newtonsoft.Json</i> - JSON.NET v9.0.1</td>
  </tr>
</table>

<table>
  <tr>
    <th width="300" style="min-width:300px; max-width: 300px">Description</th>
    <th width="587" style="min-width:531px;">Assembly - Tests</th>
  </tr>
  <tr>
    <td>External References</td>
    <td><i>Microsoft.ExceptionMessageBox</i> - v11.0.2100.60</td>
  </tr>
  <tr>
    <td>Unit Testing</td>
    <td><i>NUnit</i> - v3.5.0</td>
  </tr>
</table>

<a name="using"><h2>Using the code</h2></a>
There are four essential steps to using this:

&nbsp;1. Optional: Setup an action delegate with ... parameters.<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Make any <i>Control</i> use the method pulled in from [Forms1.cs](GUI/WinForms/Form1.cs) which is basically a wrapper for invocating a method across<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;boundary switching thread contexts.

```csharp
   Action<Control, MethodInvoker> action = (ctl, a) => {
     if (ctl.InvokeRequired)
       ctl.Invoke(a);
     else
       a();
   };
```

&nbsp;2. Create a \`<i>TaskEvent</i>\` with type parameter matching the type from #1. (if specified)<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Using <i>object initialization</i>, specify the timeout for 5 seconds which is shorter than the default.<br>
&nbsp;3. Specify the `OnRunning` event handler delegate.  (Required)<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;There is an exception handler for this in the \`<i>AsyncMonitor</i>\` method if omitted.<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i>Optional</i>:<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Construct the `OnCompleted` and `OnTimeout` event handler delegates similarly.<br>
&nbsp;4. Invoke the extension method.<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i>[Controllers/Async-IO.cs](DLL/Controllers/Async-IO.cs)</i>:   Extension methods for asynchronous routines.
```csharp
   public static async void AsyncMonitor<T>(this T expression, TaskEvent<T> t) where T : class { ... }
```
   Usage:
```csharp
   action.AsyncMonitor(t)
```
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Or since it is an extension method, it can be called explicitly since static:
```csharp
   Async_IO.AsyncMonitor(null, t)
```

### Example #1
```csharp
const int TTL = 5000;
var @t = new TaskEvent<MyType>(TTL) {
  Name = "t0"
};
// ---------------------------------
@t.MainAction.OnRunning += (th, tea) => {
  // Poll for 1 second
  if (tea.Token.HasValue && !tea.Token.Value.IsCancellationRequested)
    SpinWait.SpinUntil(() => false, new TimeSpan(0, 0, 1));
};
@t.PostAction.OnCompleted += (th, tea) => {
  if (tea.Expression != null)
    tea.Expression(richTextBox1, delegate { richTextBox1.Text += "Completed\n"; });
  else
    richTextBox1.Text += "Completed\n";     // Cross-Threaded call to UI thread will throw exception.
};
@t.TimeoutAction.OnTimeout += (th, tea) => {
  // Is the same as above without the else clause using null propagation matching signature from Action<T1, T2>.Invoke Method in namespace System (System.Runtime.dll)
  tea.Expression?.Invoke(richTextBox1, delegate { richTextBox1.Text += "Timed out\n"; });
};
// ---------------------------------
action.AsyncMonitor(@t);   // The extension method to use.
```

1. Run \`<i>Task Scheduler.exe</i>\`.
2. Click the `Start New Task` button in the window pane to spawn a new event.
3. Click the `Cancel` button to stop the event.
4. Click the `Remove` button to remove the event's line from the list view.

<a name="other"><h2>Other features</h2></a>
- Unit Tests:<br>
  The unit test uses NUnit to help qualify the underlying API.   Included is a sample that can be ran and tested againsted a variety of mock senario conditions.
- UI Application:<br>
  In addition to being able to use the API library as standalone, the project consists of two user-interfaces I developed using Telerik's RAD tools and the other with ordinary stock controls.

<a name="references"><h2>References</h2></a>
 TPL, .NET 4.x, ORM, Dependency Injection, Generics, Delegates, EventHandlers, Parametric Polymorphism

<a name="license"><h2>License</h2></a>
[GNU LESSER GENERAL PUBLIC LICENSE] - Version 3, 29 June 2007


[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job.)

   [GNU LESSER GENERAL PUBLIC LICENSE]: <http://www.gnu.org/licenses/lgpl-3.0.en.html>
